using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
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
    }
}
