using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TON
{
    public class ControllerUI : UIBase
    {
        public static ControllerUI Instance => UIManager.Singleton.GetUI<ControllerUI>(UIList.ControllerUI);

        /// <summary> 조이스틱에서 컨트롤된 x 값을 Player에서 사용할 수 있도록 객체화 </summary>
        public VariableJoystick joystick;
        public CharacterBase linkedCharactor { get; set; }


        public void OnClickJumpButton()
        {
            linkedCharactor.Jump();
        }

        public void OnClickAttackButton()
        {
            linkedCharactor.Attack();
        }
    }
}
