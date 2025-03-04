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
            string id = skillId.ToLower();
            return LoadAsset<Sprite>($"UI/Skill Icon/skill_icon_{id}", out result);
        }

        public bool LoadPlayerIcon(string playerType, FaceStatue status, out Sprite result)
        {
            var playerColor = playerType == "b" ? "Black" : "White";
            return LoadAsset<Sprite>($"Player/Character Face/{playerColor}_{status}", out result);
        }

        public bool LoadMonsterWaveIcon(int wave, out Sprite result)
        {
            return LoadAsset<Sprite>($"UI/Monster Portrait/wave{wave}_monster", out result);
        }

        public bool LoadRankPawIcon(int rank, out Sprite result)
        {
            if (rank < 4)
            {
                // Rank 1, 2, 3일 때 실행할 코드
                return LoadAsset<Sprite>($"UI/Ranking Paw/paw_{rank}th", out result);
            }
            else if (rank >= 4 && rank <= 20)
            {
                // Rank 4~20일 때 실행할 코드
                return LoadAsset<Sprite>($"UI/Ranking Paw/paw_4th", out result);
            }
            else if (rank >= 21 && rank <= 50)
            {
                // Rank 21~50일 때 실행할 코드
                return LoadAsset<Sprite>($"UI/Ranking Paw/paw_5th", out result);
            }
            else
            {
                // Rank 51 이상일 때 실행할 코드
                return LoadAsset<Sprite>($"UI/Ranking Paw/paw_6th", out result);
            }
        }

        public bool LoadMyRankBoxImage(out Sprite result)
        {
            return LoadAsset<Sprite>($"UI/Ranking Paw/my_rank_box", out result);
        }
    }
}
