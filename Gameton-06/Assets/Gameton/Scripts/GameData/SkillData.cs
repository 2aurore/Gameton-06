using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    [System.Serializable]
    public class SkillData
    {
        // 스킬 아이디
        public string id;
        // 스킬 이름
        public string name;
        // 스킬 mp 소모량
        public int mpConsumption;
        // 스킬 데미지
        public float damage;
        // 스킬 쿨타임
        public int coolDown;
        // 스킬 장착 슬롯 번호
        public int slotNumber;
        // 스킬 사용을 위해 필요한 캐릭터 레벨
        public int requiredLevel;
        // 스킬이 한번에 타격할 수 있는 몬스터의 수
        public int maxHitCount;
    }
}
