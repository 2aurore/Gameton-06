using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class UIBase : MonoBehaviour
    {
        public Object eventSystem;
        public virtual void Show()
        {
            // eventSystem이 현재 씬에 존재하지 않는 경우 
            if (eventSystem == null)
            {
                // UI EventSystem 정상 동작 되도록 GameObject Load
                eventSystem = Instantiate(Resources.Load("EventSystem/Prefabs/TON.EventSystem"));
            }

            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
