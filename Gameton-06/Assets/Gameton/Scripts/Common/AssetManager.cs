using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    public enum FaceStatue
    {
        Idle,
        Angry,
        Sad,
        Oh,
        Smile
    }

    public class AssetManager : SingletonBase<AssetManager>
    {

        public bool LoadAsset<T>(string path, out T result) where T : UnityEngine.Object
        {
            result = Resources.Load<T>(path);
            return result != null;
        }

        public bool LoadSkillIcon(string skillId, out Sprite result)
        {
            return LoadAsset<Sprite>($"UI/Skill Icon/skill_icon_{skillId}", out result);
        }

        public bool LoadPlayerIcon(string playerType, FaceStatue status, out Sprite result)
        {
            var playerColor = playerType == "b" ? "Black" : "White";
            return LoadAsset<Sprite>($"Player/Character Face/{playerColor}_{status}", out result);
        }

        public bool LoadStageIcon(string stageId, out Sprite result)
        {
            return LoadAsset<Sprite>($"UI/Stage/stage_{stageId}", out result);
        }
    }
}
