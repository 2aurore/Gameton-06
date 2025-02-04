using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class SwipeController : MonoBehaviour
    {
        [SerializeField] int maxPage;

        [SerializeField] int currentPage;
        [SerializeField] Vector2 targetPos;
        [SerializeField] Vector2 pageStep;
        [SerializeField] RectTransform levelPagesRect;
        [SerializeField] float tweenTime;
        [SerializeField] LeanTweenType tweenType;


        private void Awake()
        {
            currentPage = 1;
            targetPos = levelPagesRect.localPosition;
        }

        public void Next()
        {
            if (currentPage < maxPage)
            {
                currentPage++;
                targetPos += pageStep;
                MovePage();
            }
        }

        public void Previous()
        {
            if (currentPage > 1)
            {
                currentPage--;
                targetPos -= pageStep;
                MovePage();
            }
        }

        public void MovePage()
        {
            levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);
        }
    }
}
