using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class CharaterSelection : MonoBehaviour
    {
        public CharaterCreateUI characterCreateUI;

        public void OnSelectMaleCat()
        {
            characterCreateUI.SelectCharacter("MaleCat");
        }

        public void OnSelectFemaleCat()
        {
            characterCreateUI.SelectCharacter("FemaleCat");
        }
    }
}
