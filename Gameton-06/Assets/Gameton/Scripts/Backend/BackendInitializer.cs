using UnityEngine;
using BackEnd;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 초기화 및 연결 담당 클래스
    /// </summary>
    public class BackendInitializer
    {
        // 초기화 성공 여부
        public bool IsInitialized { get; private set; }

        // 초기화 이벤트 델리게이트
        public delegate void InitializeEvent(bool isSuccess);
        public event InitializeEvent OnInitialized;

        /// <summary>
        /// 뒤끝 서버 초기화
        /// </summary>
        public void Initialize()
        {
            var bro = Backend.Initialize();

            if (bro.IsSuccess())
            {
                Debug.Log("뒤끝 서버 초기화 성공: " + bro);
                IsInitialized = true;
                OnInitialized?.Invoke(true);
            }
            else
            {
                Debug.LogError("뒤끝 서버 초기화 실패: " + bro);
                IsInitialized = false;
                OnInitialized?.Invoke(false);
            }
        }

        /// <summary>
        /// 서버 상태 확인
        /// </summary>
        public bool CheckServerStatus()
        {
            var bro = Backend.Utils.GetServerStatus();
            if (bro.StatusCode == 2)
            {
                Debug.LogWarning("서버 점검 중입니다.");
                return false;
            }
            return true;
        }
    }
}