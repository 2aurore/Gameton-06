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
                    StartGame();
                }
                else
                {
                    // 로그인 실패 시 자동 회원가입 시도
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
                    StartGame();
                }
                else
                {
                    Debug.LogError("자동 회원가입 실패: " + callback.GetMessage());
                    // 재시도 또는 오류 처리
                    HandleSignUpError();
                }
            });
        }

        private string GetOrCreateCustomID()
        {
            // PlayerPrefs에서 저장된 ID 확인
            string savedID = PlayerPrefs.GetString("BackendCustomID", string.Empty);

            if (string.IsNullOrEmpty(savedID))
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

        private void StartGame()
        {
            // 로그인 성공 후 게임 시작 로직
            // 예: 메인 게임 씬 로드
        }

        private void HandleSignUpError()
        {
            // 회원가입 실패 처리
            // 재시도 또는 다른 방법 사용
        }

    }
}
