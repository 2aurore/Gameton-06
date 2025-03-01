using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TON
{
    public class SoundManager : MonoBehaviour
    {
        public AudioSource bgSound;
        
        public static SoundManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(instance);
                // 씬 로드 이벤트에 리스너 등록
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬이 바뀔 때마다 배경 음악 종료
            BgSoundPlay(null);
        }

        public void SFXPlay(string sfxName, AudioClip clip)
        {
            GameObject go = new GameObject(sfxName + "Sound");
            AudioSource audiosource = go.AddComponent<AudioSource>();

            audiosource.clip = clip;
            audiosource.Play();
            
            Destroy(go, clip.length);
        }

        public void BgSoundPlay(AudioClip clip)
        {
            bgSound.clip = clip;
            bgSound.loop = true;
            bgSound.volume = 1f;
            bgSound.Play();
        }
    }
}
