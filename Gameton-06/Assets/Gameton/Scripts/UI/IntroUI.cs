using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public class IntroUI : UIBase
    {
        public List<IntroStoryData> introStories = new List<IntroStoryData>();

        private IntroStoryDataManager introStoryDataManager;

        private void OnEnable()
        {
            introStoryDataManager = new IntroStoryDataManager();

            introStories = introStoryDataManager.introStories;
        }
    }
}
