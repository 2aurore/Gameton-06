using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 사용자 소모 아이템 데이터 관리 담당 클래스
    /// </summary>
    public class BackendItemDataManager
    {
        // 테이블 이름 상수
        private const string USER_ITEM_TABLE = "USER_ITEM_DATA";

        public void LoadMyItemData(System.Action<UserItemData> onComplete)
        {
            UserItemData userItemData = new UserItemData();

            Backend.PlayerData.GetMyData(USER_ITEM_TABLE, callback =>
            {
                if (callback.IsSuccess() == false)
                {
                    Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + callback.ToString());
                    onComplete?.Invoke(userItemData); // 에러 상황에서 기본 객체 반환
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
                    userItemData.hpPotion = int.Parse(callback.FlattenRows()[0]["hp_potion"].ToString());
                    userItemData.mpPotion = int.Parse(callback.FlattenRows()[0]["mp_potion"].ToString());
                    onComplete?.Invoke(userItemData); // 성공 시 데이터 반환
                }
            });
        }

        // 데이터 삽입 후 다시 불러오는 메소드
        private void LoadDataAfterInsert(System.Action<UserItemData> onComplete)
        {
            Backend.PlayerData.GetMyData(USER_ITEM_TABLE, callback =>
            {
                UserItemData userItemData = new UserItemData();

                if (callback.IsSuccess() && callback.FlattenRows().Count > 0)
                {
                    userItemData.hpPotion = int.Parse(callback.FlattenRows()[0]["hp_potion"].ToString());
                    userItemData.mpPotion = int.Parse(callback.FlattenRows()[0]["mp_potion"].ToString());
                }

                onComplete?.Invoke(userItemData);
            });
        }

        /// <summary>
        /// 캐릭터 초기 생성 시 row 삽입
        /// </summary>
        public void InsertInitData(System.Action onComplete = null)
        {
            Param param = new Param();
            param.Add("hp_potion", 5);
            param.Add("mp_potion", 5);

            Backend.PlayerData.InsertData(USER_ITEM_TABLE, param, callback =>
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

    }
}
