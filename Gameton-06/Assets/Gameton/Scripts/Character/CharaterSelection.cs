using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharaterSelection : MonoBehaviour
    {
        public CharaterCreateUI characterCreateUI;

        public void OnSelectBlackCat()
        {
            characterCreateUI.SelectCharacter("BlackCat");
        }

        public void OnSelectWhiteCat()
        {
            characterCreateUI.SelectCharacter("WhiteCat");
        }
    }
}
