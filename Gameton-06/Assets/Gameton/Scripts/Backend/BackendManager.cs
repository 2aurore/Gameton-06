using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 뒤끝 SDK namespace 추가
using BackEnd;

namespace TON
{
    /// <summary>
    /// 랭킹 데이터 관리를 위한 뒤끝 서버 매니저
    /// </summary>
    public class BackendManager : SingletonBase<BackendManager>
    {
        private string PASSWORD = "O-xmP0-=rp&kCq^";

        private int signUpRetryCount = 0;   // 로그인 재시도 횟수
        private const int MAX_RETRY_COUNT = 1; // 최대 재시도 횟수

        public void Initalize()
        {
            var bro = Backend.Initialize(); // 뒤끝 초기화

            // 뒤끝 초기화에 대한 응답값
            if (bro.IsSuccess())
            {
                Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
                HandleBackendCallback();
            }
            else
            {
                Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생
            }
        }

        private void HandleBackendCallback()
        {
            // 서버 초기화 완료 후 자동 로그인 시도
            TryAutoLogin();
        }

        private void TryAutoLogin()
        {
            // 저장된 계정 정보 확인
            string customID = GetOrCreateCustomID();

            // 자동 로그인 시도
            Backend.BMember.CustomLogin(customID, PASSWORD, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("자동 로그인 성공!");
                    // 게임 시작 로직
                    // Main.cs 에서 처리하고 있기 때문에 별도 로직 처리하지 않음음
                }
                else
                {
                    // 로그인 실패 시 자동 회원가입 시도
                    signUpRetryCount = 0; // 회원가입 시도 전 카운트 초기화
                    TryAutoSignUp(customID);
                }
            });
        }

        private void TryAutoSignUp(string customID)
        {
            Backend.BMember.CustomSignUp(customID, PASSWORD, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("자동 회원가입 성공!");
                    // 게임 시작 로직
                    // Main.cs 에서 처리하고 있기 때문에 별도 로직 처리하지 않음음
                }
                else
                {
                    Debug.LogError("자동 회원가입 실패: " + callback.GetMessage());
                    // 재시도 또는 오류 처리
                    HandleSignUpError();
                }
            });
        }

        private string GetOrCreateCustomID(bool forceNew = false)
        {
            // PlayerPrefs에서 저장된 ID 확인
            string savedID = PlayerPrefs.GetString("BackendCustomID", string.Empty);

            if (string.IsNullOrEmpty(savedID) || forceNew)
            {
                // 새 ID 생성 (디바이스 고유 ID 활용)
                string deviceID = SystemInfo.deviceUniqueIdentifier;
                string appID = Application.identifier;
                savedID = $"{appID}_{deviceID}_{System.DateTime.Now.Ticks}";

                // 생성된 ID 저장
                PlayerPrefs.SetString("BackendCustomID", savedID);
                PlayerPrefs.Save();
            }

            return savedID;
        }

        // 캐릭터 닉네임 생성 시 유저 데이터에 닉네임 변경 적용용
        // 닉네임 변경 메소드
        public void ChangeNickname(string newNickname, System.Action<bool, string> callback)
        {
            // 닉네임 형식 검증
            if (string.IsNullOrEmpty(newNickname) || newNickname.Length < 2)
            {
                callback?.Invoke(false, "닉네임은 2자 이상이어야 합니다.");
                return;
            }

            // 뒤끝 서버 닉네임 변경 요청
            Backend.BMember.UpdateNickname(newNickname, bro =>
            {
                if (bro.IsSuccess())
                {
                    Debug.Log("닉네임 변경 성공: " + newNickname);
                    callback?.Invoke(true, "닉네임이 변경되었습니다.");
                }
                else
                {
                    Debug.LogError("닉네임 변경 실패: " + bro.GetMessage());
                    callback?.Invoke(false, "닉네임 변경 실패: " + bro.GetMessage());
                }
            });
        }


        private void HandleSignUpError()
        {
            // 회원가입 실패 처리
            // 재시도 횟수 확인
            if (signUpRetryCount < MAX_RETRY_COUNT)
            {
                signUpRetryCount++;
                Debug.Log($"회원가입 재시도 중... ({signUpRetryCount}/{MAX_RETRY_COUNT})");

                // 약간의 딜레이 후 재시도 (네트워크 상태 개선 기회 제공)
                Invoke(nameof(RetrySignUp), 2.0f);
            }
            else
            {
                // 최대 재시도 횟수 초과 - 게임 종료
                ShowErrorAndQuitGame("서버 연결에 실패했습니다. 네트워크 상태를 확인하고 다시 시도해주세요.");
            }
        }

        private void RetrySignUp()
        {
            // 새로운 customID 생성 시도 (기존 ID에 문제가 있을 수 있음)
            string newCustomID = GetOrCreateCustomID(true);
            TryAutoSignUp(newCustomID);
        }

        private void ShowErrorAndQuitGame(string errorMessage)
        {
            // 에러 메시지 표시 (UI 구현 필요)
            Debug.LogError("게임 종료: " + errorMessage);

            // 팝업 UI 표시 후 게임 종료 로직
            ShowErrorPopup(errorMessage, () =>
            {
                // 팝업 확인 후 게임 종료
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }

        private void ShowErrorPopup(string message, System.Action onConfirm)
        {
            // TODO: 에러 팝업 UI 구현
            // ErrorPopupManager.Instance.ShowPopup(message, onConfirm);

            // 임시 구현 (실제로는 UI 구현 필요)
            Debug.LogError("에러 팝업: " + message);
            onConfirm?.Invoke(); // 바로 콜백 실행 (실제로는 사용자 확인 후 실행)
        }

    }
}
