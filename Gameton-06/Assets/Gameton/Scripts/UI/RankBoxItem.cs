using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace TON
{
    public class RankBoxItem : MonoBehaviour
    {
        [SerializeField] private GameObject rankBoxImage;
        [SerializeField] private GameObject pawImage;
        [SerializeField] private TextMeshProUGUI playerName;
        [SerializeField] private TextMeshProUGUI rankNumber;
        [SerializeField] private TextMeshProUGUI waveText;
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI playTimeText;


        public void Initalize(int rank, ClearData clearData)
        {
            playerName.text = clearData.nickname;
            rankNumber.text = $"{rank} th";
            waveText.text = $"{clearData.wave}";
            scoreText.text = $"{clearData.score}";

            int minutes = Mathf.FloorToInt(clearData.playTime / 60f);
            int seconds = Mathf.FloorToInt(clearData.playTime % 60f);
            playTimeText.text = $"{minutes:0}m {seconds:0}s";

            SetPawIcon(rank);

            if (clearData.nickname == PlayerDataManager.Singleton.player.name)
            {
                SetMyRankBoxImage();
            }
        }

        private void SetMyRankBoxImage()
        {
            Sprite loadedMyBoxImage = null;
            Assert.IsTrue(AssetManager.Singleton.LoadMyRankBoxImage(out loadedMyBoxImage));
            rankBoxImage.GetComponent<Image>().sprite = loadedMyBoxImage;
        }

        private void SetPawIcon(int rank)
        {
            Sprite loadedPawImage = null;
            Assert.IsTrue(AssetManager.Singleton.LoadRankPawIcon(rank, out loadedPawImage));
            pawImage.GetComponent<Image>().sprite = loadedPawImage;
        }
    }
}
