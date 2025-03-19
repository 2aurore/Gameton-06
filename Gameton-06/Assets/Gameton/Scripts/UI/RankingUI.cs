using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TON
{
    public class RankingUI : UIBase
    {
        public static RankingUI Instance => UIManager.Singleton.GetUI<RankingUI>(UIList.RankingUI);

        // 전체 랭킹 리스트를 스크롤 형식으로 구현하기 위함
        public ScrollRect scrollRect;
        public RankBoxItem rankBoxPrefab;
        public List<RectTransform> uiPrefabList = new List<RectTransform>();
        public List<RankBoxItem> createRankList = new List<RankBoxItem>();

        [SerializeField] private GameObject errorPopup;
        [SerializeField] private GameObject pawImage;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI rankNumber;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI playTimeText;

        private List<ClearData> rankList = new List<ClearData>();

        private void OnEnable()
        {
            // 랭킹 불러오기 오류 팝업 기본 상태
            errorPopup.SetActive(false);

            SetRankList();
            SetMyRankData();
        }

        private void SetMyRankData()
        {
            ClearData TOP_RECORD = StageManager.Singleton.TOP_RECORD;
            int myRankNumber = StageManager.Singleton.GetMyRankNumber();

            playerName.text = TOP_RECORD.nickname;
            rankNumber.text = myRankNumber > -1 ? $"{myRankNumber + 1} th" : "Not Record";
            waveText.text = $"{TOP_RECORD.wave}";
            scoreText.text = $"{TOP_RECORD.score}";

            int minutes = Mathf.FloorToInt(TOP_RECORD.playTime / 60f);
            int seconds = Mathf.FloorToInt(TOP_RECORD.playTime % 60f);
            playTimeText.text = $"{minutes:0}m {seconds:0}s";

            SetPawIcon(myRankNumber + 1);
        }

        private void SetRankList()
        {
            // 이미 기존에 UI가 생성되어 있다면 삭제
            if (createRankList.Count > 0)
            {
                createRankList.Clear();
            }
            if (uiPrefabList.Count > 0)
            {
                uiPrefabList.Clear();
            }

            rankList = StageManager.Singleton.RankList;

            if (rankList.Count == 0)
            {
                errorPopup.SetActive(true);
                return;
            }

            float y = 0;
            for (int i = 0; i < rankList.Count; i++)
            {
                ClearData clearData = rankList[i];
                RankBoxItem rankBox = Instantiate(rankBoxPrefab, scrollRect.content);

                rankBox.gameObject.SetActive(true);
                rankBox.Initalize(i + 1, clearData);
                createRankList.Add(rankBox);

                RectTransform rectTransform = rankBox.GetComponent<RectTransform>();

                uiPrefabList.Add(rectTransform);
                uiPrefabList[i].anchoredPosition = new Vector2(0f, -y);
                y += uiPrefabList[i].sizeDelta.y + 10f;
            }

            scrollRect.content.sizeDelta = new Vector2(scrollRect.content.sizeDelta.x, y);
        }

        private void SetPawIcon(int rank)
        {
            if (AssetManager.Singleton.LoadRankPawIcon(rank, out Sprite loadedPawImage))
            {
                pawImage.GetComponent<Image>().sprite = loadedPawImage;
            }
        }

        public void OnClickCloseButton()
        {
            UIManager.Hide<RankingUI>(UIList.RankingUI);
        }
    }
}
