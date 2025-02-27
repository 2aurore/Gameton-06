using System;
using System.Collections;
using UnityEngine;
using BackEnd;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 인증(로그인/회원가입) 담당 클래스
    /// </summary>
    public class BackendAuthManager
    {
        private const string PASSWORD = "O-xmP0-=rp&kCq^";
        private const string CUSTOM_ID_KEY = "BackendCustomID";
        private const int MAX_RETRY_COUNT = 1;

        private int signUpRetryCount = 0;

        // 로그인 상태
        public bool IsLoggedIn { get; private set; }

        // 인증 이벤트 델리게이트
        public delegate void AuthEvent(bool isSuccess, string message);
        public event AuthEvent OnLoginComplete;
        public event AuthEvent OnSignUpComplete;

        /// <summary>
        /// 자동 로그인 시도
        /// </summary>
        public void TryAutoLogin()
        {
            string customID = GetOrCreateCustomID();

            Backend.BMember.CustomLogin(customID, PASSWORD, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("자동 로그인 성공!");
                    IsLoggedIn = true;
                    OnLoginComplete?.Invoke(true, "로그인 성공");
                }
                else
                {
                    Debug.Log("로그인 실패, 회원가입 시도: " + callback.GetMessage());
                    signUpRetryCount = 0;
                    TryAutoSignUp(customID);
                }
            });
        }

        /// <summary>
        /// 자동 회원가입 시도
        /// </summary>
        private void TryAutoSignUp(string customID)
        {
            Backend.BMember.CustomSignUp(customID, PASSWORD, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("자동 회원가입 성공!");
                    IsLoggedIn = true;
                    OnSignUpComplete?.Invoke(true, "회원가입 성공");
                }
                else
                {
                    Debug.LogError("자동 회원가입 실패: " + callback.GetMessage());
                    HandleSignUpError();
                }
            });
        }

        /// <summary>
        /// 커스텀 ID 생성 또는 가져오기
        /// </summary>
        private string GetOrCreateCustomID(bool forceNew = false)
        {
            string savedID = PlayerPrefs.GetString(CUSTOM_ID_KEY, string.Empty);

            if (string.IsNullOrEmpty(savedID) || forceNew)
            {
                string deviceID = SystemInfo.deviceUniqueIdentifier;
                string appID = Application.identifier;
                savedID = $"{appID}_{deviceID}_{DateTime.Now.Ticks}";

                PlayerPrefs.SetString(CUSTOM_ID_KEY, savedID);
                PlayerPrefs.Save();
            }

            return savedID;
        }

        /// <summary>
        /// 닉네임 변경
        /// </summary>
        public void ChangeNickname(string newNickname, Action<bool, string> callback)
        {
            if (string.IsNullOrEmpty(newNickname) || newNickname.Length < 2)
            {
                callback?.Invoke(false, "닉네임은 2자 이상이어야 합니다.");
                return;
            }

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

        /// <summary>
        /// 회원가입 오류 처리
        /// </summary>
        private void HandleSignUpError()
        {
            if (signUpRetryCount < MAX_RETRY_COUNT)
            {
                signUpRetryCount++;
                Debug.Log($"회원가입 재시도 중... ({signUpRetryCount}/{MAX_RETRY_COUNT})");

                // 이벤트 호출로 외부에서 회원가입 재시도 처리하도록 함
                OnSignUpComplete?.Invoke(false, "재시도 중");

                // 새 ID로 재시도
                string newCustomID = GetOrCreateCustomID(true);
                TryAutoSignUp(newCustomID);
            }
            else
            {
                OnSignUpComplete?.Invoke(false, "최대 재시도 횟수 초과");
            }
        }

        /// <summary>
        /// 로그아웃
        /// </summary>
        public void Logout()
        {
            Backend.BMember.Logout();
            IsLoggedIn = false;
        }
    }
}