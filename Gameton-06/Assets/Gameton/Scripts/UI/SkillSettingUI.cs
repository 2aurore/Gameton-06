using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class SkillSettingUI : UIBase
    {
        public static SkillSettingUI Instance => UIManager.Singleton.GetUI<SkillSettingUI>(UIList.SkillSettingUI);

        public List<SkillSettingUI_SkillSlot> slots;

        public void Start()
        {

        }

    }
}
