using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using LitJson;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 데이터 관리 담당 클래스
    /// </summary>
    public class BackendCashDataManager
    {
        // 테이블 이름 상수
        private const string CASH_TABLE = "CASH_DATA";


        // PlayerDataManager에서 재화 데이터 로드
        public void LoadMyCashData(System.Action<CashData> onComplete)
        {
            CashData cashData = new CashData();

            Backend.PlayerData.GetMyData(CASH_TABLE, callback =>
            {
                if (callback.IsSuccess() == false)
                {
                    Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + callback.ToString());
                    onComplete?.Invoke(cashData); // 에러 상황에서도 기본 데이터 반환
                    return;
                }

                // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
                if (callback.IsSuccess() && callback.FlattenRows().Count <= 0)
                {
                    Debug.Log("데이터가 존재하지 않습니다");
                    InsertInitData(() =>
                    {
                        // 초기 데이터 삽입 후 다시 데이터를 불러옴
                        LoadDataAfterInsert(onComplete);
                    });
                    return;
                }

                // 1개 이상 데이터를 불러온 경우
                if (callback.FlattenRows().Count > 0)
                {
                    cashData.fish = int.Parse(callback.FlattenRows()[0]["fish"].ToString());
                    cashData.gold = int.Parse(callback.FlattenRows()[0]["gold"].ToString());
                    onComplete?.Invoke(cashData); // 성공 시 데이터 반환
                }
            });
        }

        // 데이터 삽입 후 다시 불러오는 메소드
        private void LoadDataAfterInsert(System.Action<CashData> onComplete)
        {
            Backend.PlayerData.GetMyData(CASH_TABLE, callback =>
            {
                CashData cashData = new CashData();

                if (callback.IsSuccess() && callback.FlattenRows().Count > 0)
                {
                    cashData.fish = int.Parse(callback.FlattenRows()[0]["fish"].ToString());
                    cashData.gold = int.Parse(callback.FlattenRows()[0]["gold"].ToString());
                }

                onComplete?.Invoke(cashData);
            });
        }

        /// <summary>
        /// 캐릭터 초기 생성 시 row 삽입
        /// </summary>
        public void InsertInitData(System.Action onComplete = null)
        {
            Param param = new Param();
            param.Add("gold", 0);
            param.Add("fish", 0);

            Backend.PlayerData.InsertData(CASH_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("초기 데이터 삽입 성공");
                }
                else
                {
                    Debug.LogError("초기 데이터 삽입 실패: " + callback.ToString());
                }

                onComplete?.Invoke();
            });
        }

        public void UpdateFishData(int fish, System.Action<CashData> onComplete = null)
        {
            Param param = new Param();
            param.Add("fish", fish);

            Backend.PlayerData.UpdateMyLatestData(CASH_TABLE, param, callback =>
            {
                CashData updatedData = new CashData { fish = fish };
                onComplete?.Invoke(updatedData);
            });
        }

        public void UpdateGoldData(int gold, System.Action<CashData> onComplete = null)
        {
            Param param = new Param();
            param.Add("gold", gold);

            Backend.PlayerData.UpdateMyLatestData(CASH_TABLE, param, callback =>
            {
                CashData updatedData = new CashData { gold = gold };
                onComplete?.Invoke(updatedData);
            });
        }
    }
}