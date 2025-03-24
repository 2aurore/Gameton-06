using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class IntroStoryDataManager
    {
        public List<IntroStoryData> introStories { get; private set; }

        public void Initialize()
        {
            LoadIntroStoryData();
        }

        private void LoadIntroStoryData()
        {
            introStories = JSONLoader.LoadFromResources<List<IntroStoryData>>("intro_story");
            if (introStories == null)
            {
                introStories = new List<IntroStoryData>();
                Debug.LogError("스토리 데이터 로드 실패");
            }
        }
    }
}
