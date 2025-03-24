using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TON
{

    public class CameraFollow : MonoBehaviour
    {
        private CinemachineVirtualCamera vcam;

        void Awake()
        {
            vcam = GetComponent<CinemachineVirtualCamera>();
            if (vcam == null)
            {
                Debug.LogError("Cinemachine Virtual Camera component is missing!");
            }
        }

        public void SetFollowTarget(Transform target)
        {
            if (vcam != null)
            {
                vcam.Follow = target;
            }
        }
    }

}
