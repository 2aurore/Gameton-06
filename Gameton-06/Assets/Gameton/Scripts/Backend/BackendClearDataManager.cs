using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 게임 클리어 데이터 관리 담당 클래스
    /// </summary>
    public class BackendClearDataManager
    {
        // 테이블 이름 상수
        private const string CLEAR_TABLE = "CLEAR_DATA";

        /// <summary>
        /// 캐릭터 게임 클리어 시 row 삽입
        /// </summary>
        public void InsertInitData(ClearData clearData)
        {
            Param param = new Param();
            param.Add("score", clearData.score);
            param.Add("wave", clearData.wave);
            param.Add("play_time", clearData.playTime);
            param.Add("nickname", PlayerDataManager.Singleton.player.name);

            Backend.PlayerData.InsertData(CLEAR_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("BackendClearDataManager 데이터 삽입 성공");
                }
                else
                {
                    Debug.LogError("BackendClearDataManager 데이터 삽입 실패: " + callback.ToString());
                }

            });
        }

    }
}
