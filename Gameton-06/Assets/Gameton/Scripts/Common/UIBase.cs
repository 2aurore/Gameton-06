using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TON
{
    public class UIBase : MonoBehaviour
    {
        private static EventSystem globalEventSystem;

        public virtual void Show()
        {
            // 🔹 실행 중인 모든 씬에서 EventSystem 확인 (DontDestroyOnLoad 포함)
            if (globalEventSystem == null)
            {
                globalEventSystem = FindExistingEventSystem();
                if (globalEventSystem == null)
                {
                    // 🔹 없으면 새로운 EventSystem 생성
                    GameObject obj = Instantiate(Resources.Load<GameObject>("EventSystem/Prefabs/TON.EventSystem"));
                    globalEventSystem = obj.GetComponent<EventSystem>();

                    // 🔹 새로 만든 EventSystem을 DontDestroyOnLoad로 유지
                    DontDestroyOnLoad(obj);
                }
            }

            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }

        private EventSystem FindExistingEventSystem()
        {
            // 🔹 모든 씬을 포함하여 EventSystem 검색 (씬 이동해도 유지되는 객체 포함)
            EventSystem[] eventSystems = Resources.FindObjectsOfTypeAll<EventSystem>();
            return eventSystems.Length > 0 ? eventSystems[0] : null;
        }
    }
}
