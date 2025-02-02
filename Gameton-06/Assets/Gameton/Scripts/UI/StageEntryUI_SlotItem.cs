using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class StageEntryUI_SlotItem : MonoBehaviour
    {
        [SerializeField] private int index;
        private StageEntryUI stageEntryUI;

        private void Start()
        {
            // 상위 오브젝트에서 CharacterSelectUI 찾기
            stageEntryUI = FindObjectOfType<StageEntryUI>();

            // 버튼 클릭 이벤트 등록
            GetComponent<Button>().onClick.AddListener(OnClickStage);
        }

        public void OnClickStage()
        {
            stageEntryUI.OnClickStageButton(index);
        }
    }
}
