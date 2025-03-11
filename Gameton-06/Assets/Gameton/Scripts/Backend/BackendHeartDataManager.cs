using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 유저 입장권(하트) 데이터 관리 담당 클래스
    /// </summary>
    public class BackendHeartDataManager
    {
        private const string HEART_TABLE = "HEART_DATA";

        public void LoadMyHeartData(System.Action<HeartData> onComplete)
        {
            HeartData heartData = new HeartData();

            Backend.PlayerData.GetMyData(HEART_TABLE, callback =>
            {
                if (callback.IsSuccess() == false)
                {
                    Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + callback.ToString());
                    Main.Singleton.SystemQuit();
                    return;
                }

                // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
                if (callback.IsSuccess() && callback.FlattenRows().Count <= 0)
                {
                    Debug.Log("데이터가 존재하지 않습니다");
                    onComplete.Invoke(null);
                    return;
                }

                // 정상적으로 플레이어 데이터를 불러온 경우
                if (callback.FlattenRows().Count > 0)
                {
                    heartData.currentHearts = int.Parse(callback.FlattenRows()[0]["currentHearts"].ToString());

                    heartData.lastHeartTime = callback.FlattenRows()[0]["lastHeartTime"].ToString();
                    onComplete?.Invoke(heartData); // 성공 시 데이터 반환
                }
            });
        }


        /// <summary>
        /// 캐릭터 초기 생성 시 row 삽입
        /// </summary>
        public void CreateNewPlayerHeart(HeartData heartData, System.Action<bool> onComplete = null)
        {
            Param param = new Param();
            param.Add("currentHearts", heartData.currentHearts);
            param.Add("lastHeartTime");


            Backend.PlayerData.InsertData(HEART_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("하트 입장권 생성 성공");
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.LogError("하트 입장권 생성 실패: " + callback.ToString());
                    onComplete?.Invoke(false);
                }

            });
        }

        public void UpdatePlayerHeartData(HeartData heartData, System.Action<bool> onComplete = null)
        {
            Param param = new Param();
            param.Add("currentHearts", heartData.currentHearts);
            param.Add("lastHeartTime", $"{heartData.lastHeartTime}");

            Backend.PlayerData.UpdateMyLatestData(HEART_TABLE, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("하트 입장권 업데이트 성공");
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.LogError("하트 입장권 업데이트 실패: " + callback.ToString());
                    onComplete?.Invoke(false);
                }
            });
        }

    }
}
