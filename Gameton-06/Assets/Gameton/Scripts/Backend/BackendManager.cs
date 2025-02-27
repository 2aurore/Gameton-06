using System;
using UnityEngine;
using BackEnd;

namespace TON
{
    /// <summary>
    /// 랭킹 데이터 관리를 위한 뒤끝 서버 매니저
    /// </summary>
    public class BackendManager : SingletonBase<BackendManager>
    {
        // 하위 매니저 인스턴스
        private BackendInitializer initializer;
        private BackendAuthManager authManager;

        // 초기화 완료 여부
        public bool IsInitialized => initializer?.IsInitialized ?? false;
        public bool IsLoggedIn => authManager?.IsLoggedIn ?? false;

        // 에러 발생 시 콜백
        public event Action<string> OnErrorOccurred;

        public void Initalize()
        {
            // 각 매니저 초기화
            initializer = new BackendInitializer();
            authManager = new BackendAuthManager();

            // 이벤트 등록
            initializer.OnInitialized += HandleInitializeResult;
            authManager.OnLoginComplete += HandleLoginResult;
            authManager.OnSignUpComplete += HandleSignUpResult;

            // 초기화 시작
            initializer.Initialize();
        }

        // 초기화 결과 처리
        private void HandleInitializeResult(bool isSuccess)
        {
            if (isSuccess)
            {
                // 서버 상태 확인 후 로그인 시도
                if (initializer.CheckServerStatus())
                {
                    authManager.TryAutoLogin();
                }
                else
                {
                    ShowErrorAndQuitGame("서버 점검 중입니다. 잠시 후 다시 시도해주세요.");
                }
            }
            else
            {
                ShowErrorAndQuitGame("서버 연결에 실패했습니다. 네트워크 상태를 확인하고 다시 시도해주세요.");
            }
        }

        // 로그인 결과 처리
        private void HandleLoginResult(bool isSuccess, string message)
        {
            if (isSuccess)
            {
                Debug.Log("로그인 성공!");
                // 게임 시작 로직은 Main.cs에서 처리
            }
            else
            {
                Debug.LogWarning("로그인 실패: " + message);
                // 로그인 실패는 회원가입 시도로 이어지므로 별도 처리 안함
            }
        }

        // 회원가입 결과 처리
        private void HandleSignUpResult(bool isSuccess, string message)
        {
            if (isSuccess)
            {
                Debug.Log("회원가입 성공!");
                // 게임 시작 로직은 Main.cs에서 처리
            }
            else if (message == "최대 재시도 횟수 초과")
            {
                ShowErrorAndQuitGame("서버 연결에 실패했습니다. 네트워크 상태를 확인하고 다시 시도해주세요.");
            }
        }

        // 닉네임 변경 함수 (외부에서 호출)
        public void ChangeNickname(string newNickname, Action<bool, string> callback)
        {
            authManager.ChangeNickname(newNickname, callback);
        }

        // 로그아웃 함수 (필요 시 추가)
        public void Logout()
        {
            authManager.Logout();
        }

        private void ShowErrorAndQuitGame(string errorMessage)
        {
            Debug.LogError("게임 종료: " + errorMessage);
            OnErrorOccurred?.Invoke(errorMessage);

            // 팝업 UI 표시 후 게임 종료 로직
            ShowErrorPopup(errorMessage, () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            });
        }

        private void ShowErrorPopup(string message, Action onConfirm)
        {
            // TODO: 에러 팝업 UI 구현
            // ErrorPopupManager.Instance.ShowPopup(message, onConfirm);

            // 임시 구현 (실제로는 UI 구현 필요)
            Debug.LogError("에러 팝업: " + message);
            onConfirm?.Invoke(); // 바로 콜백 실행 (실제로는 사용자 확인 후 실행)
        }
    }
}