using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 유저 데이터 관리 담당 클래스
    /// </summary>
    public class BackendPlayerDataManager
    {
        // 테이블 이름 상수
        private const string USER_TABLE = "USER_DATA";

        // PlayerDataManager에서 플레이어 데이터 로드
        public void LoadMyPlayerData(System.Action<PlayerData> onComplete)
        {
            PlayerData playerData = new PlayerData();

            Backend.PlayerData.GetMyData(USER_TABLE, callback =>
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
                    playerData.type = callback.FlattenRows()[0]["type"].ToString();
                    playerData.name = callback.FlattenRows()[0]["name"].ToString();
                    playerData.level = int.Parse(callback.FlattenRows()[0]["level"].ToString());
                    playerData.experience = int.Parse(callback.FlattenRows()[0]["experience"].ToString());
                    playerData.hp = int.Parse(callback.FlattenRows()[0]["hp"].ToString());
                    playerData.mp = int.Parse(callback.FlattenRows()[0]["mp"].ToString());
                    playerData.attackPower = float.Parse(callback.FlattenRows()[0]["attackPower"].ToString());
                    playerData.defensivePower = float.Parse(callback.FlattenRows()[0]["defensivePower"].ToString());
                    playerData.critical = int.Parse(callback.FlattenRows()[0]["critical"].ToString());
                    onComplete?.Invoke(playerData); // 성공 시 데이터 반환
                }
            });
        }


        /// <summary>
        /// 캐릭터 초기 생성 시 row 삽입
        /// </summary>
        public void CreateNewPlayer(PlayerData playerData, System.Action<bool> onComplete = null)
        {
            Param param = new Param();
            param.Add("type", playerData.type);
            param.Add("name", playerData.name);
            param.Add("level", playerData.level);
            param.Add("experience", playerData.experience);
            param.Add("hp", playerData.hp);
            param.Add("mp", playerData.mp);
            param.Add("attackPower", playerData.attackPower);
            param.Add("defensivePower", playerData.defensivePower);
            param.Add("critical", playerData.critical);

            Backend.PlayerData.InsertData(USER_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("캐릭터 생성 성공");
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.LogError("캐릭터 생성 실패: " + callback.ToString());
                    onComplete?.Invoke(false);
                }

            });
        }


        public void UpdatePlayerData(PlayerData playerData, System.Action<bool> onComplete = null)
        {
            Param param = new Param();
            param.Add("level", playerData.level);
            param.Add("experience", playerData.experience);
            param.Add("hp", playerData.hp);
            param.Add("mp", playerData.mp);
            param.Add("attackPower", playerData.attackPower);
            param.Add("defensivePower", playerData.defensivePower);

            Backend.PlayerData.UpdateMyLatestData(USER_TABLE, param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("캐릭터 업데이트 성공");
                    onComplete?.Invoke(true);
                }
                else
                {
                    Debug.LogError("캐릭터 업데이트 실패: " + callback.ToString());
                    onComplete?.Invoke(false);
                }
            });
        }
    }
}
