using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class UIBase : MonoBehaviour
    {
        public virtual void Show()
        {
            Instantiate(Resources.Load("EventSystem/Prefabs/TON.EventSystem"));
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
