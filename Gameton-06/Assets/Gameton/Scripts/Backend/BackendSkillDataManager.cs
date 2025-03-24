using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 사용자 스킬 슬롯 데이터 관리 담당 클래스
    /// </summary>
    public class BackendSkillDataManager
    {
        // 테이블 이름 상수
        private const string SKILL_TABLE = "USER_SKILL_DATA";

        // 내 스킬 슬롯롯 데이터 조회
        public void LoadMySkillData(System.Action<UserSkillData> onComplete)
        {
            UserSkillData userSkillData = new UserSkillData();

            Backend.PlayerData.GetMyData(SKILL_TABLE, callback =>
            {
                if (callback.IsSuccess() == false)
                {
                    Debug.Log("스킬 슬롯 정보 읽기 중에 문제가 발생했습니다 : " + callback.ToString());
                    onComplete?.Invoke(userSkillData);
                    return;
                }

                // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
                if (callback.IsSuccess() && callback.FlattenRows().Count <= 0)
                {
                    Debug.Log("데이터가 존재하지 않습니다");
                    InsertInitData(userSkillData, () =>
                    {
                        // 초기 데이터 삽입 후 다시 데이터를 불러옴
                        LoadDataAfterInsert(onComplete);
                    });
                    return;
                }

                if (callback.IsSuccess())
                {
                    // 데이터를 SkillData 객체로 변환합니다.
                    userSkillData.slot_1 = callback.FlattenRows()[0]["slot_1"].ToString() ?? string.Empty;
                    userSkillData.slot_2 = callback.FlattenRows()[0]["slot_2"].ToString() ?? string.Empty;
                    userSkillData.slot_3 = callback.FlattenRows()[0]["slot_3"].ToString() ?? string.Empty;
                    onComplete?.Invoke(userSkillData);
                }
                else
                {
                    Debug.LogError("Failed to load skill data: " + callback.GetMessage());
                    onComplete?.Invoke(null);
                }
            });

        }

        // 데이터 삽입 후 다시 불러오는 메소드
        private void LoadDataAfterInsert(System.Action<UserSkillData> onComplete)
        {
            Backend.PlayerData.GetMyData(SKILL_TABLE, callback =>
            {
                UserSkillData skillData = new UserSkillData();

                if (callback.IsSuccess() && callback.FlattenRows().Count > 0)
                {
                    skillData.slot_1 = callback.FlattenRows()[0]["slot_1"].ToString() ?? string.Empty;
                    skillData.slot_2 = callback.FlattenRows()[0]["slot_2"].ToString() ?? string.Empty;
                    skillData.slot_3 = callback.FlattenRows()[0]["slot_3"].ToString() ?? string.Empty;
                }

                onComplete?.Invoke(skillData);
            });
        }


        /// <summary>
        /// 스킬 슬롯 데이터 초기 row 삽입
        /// </summary>
        private void InsertInitData(UserSkillData skillData, System.Action onComplete = null)
        {
            if (PlayerDataManager.Singleton.player == null)
            {
                return;
            }

            Param param = new Param();
            param.Add("slot_1", skillData.slot_1);
            param.Add("slot_2", skillData.slot_2);
            param.Add("slot_3", skillData.slot_3);

            Backend.PlayerData.InsertData(SKILL_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("초기 스킬 데이터 삽입 성공");
                }
                else
                {
                    Debug.LogError("초기 스킬 데이터 삽입 실패: " + callback.ToString());
                }

                onComplete?.Invoke();
            });
        }

        /// <summary>
        /// 스킬 슬롯 정보 업데이트
        /// </summary>
        public void UpdateSkillData(UserSkillData skillData, System.Action onComplete = null)
        {
            Param param = new Param();
            param.Add("slot_1", skillData.slot_1 ?? string.Empty);
            param.Add("slot_2", skillData.slot_2 ?? string.Empty);
            param.Add("slot_3", skillData.slot_3 ?? string.Empty);

            Backend.PlayerData.UpdateMyLatestData(SKILL_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("스킬 정보 업데이트 성공");
                    onComplete?.Invoke();
                }
                else
                {
                    Debug.LogError("스킬 정보 업데이트 실패: " + callback.ToString());
                }

            });
        }
    }
}
