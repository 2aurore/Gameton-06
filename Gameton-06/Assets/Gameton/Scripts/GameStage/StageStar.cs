using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class StageStar : MonoBehaviour
    {
        public GameObject fillStar;
        public GameObject emptyStar;

        public void SetStar(bool isFill)
        {
            fillStar.SetActive(isFill);
            emptyStar.SetActive(!isFill);
        }
    }
}
