using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TON
{
    public class JSONLoader : MonoBehaviour
    {
        private const string DATA_PATH = "GameData/";

        /// <summary> Resources 폴더에서 JSON 파일을 읽어 특정 데이터 타입으로 변환하는 함수 </summary>
        public static T LoadFromResources<T>(string fileName)
        {
            string path = DATA_PATH + $"{fileName}";
            TextAsset jsonFile = Resources.Load<TextAsset>(path);
            if (jsonFile != null)
            {
                return JsonUtility.FromJson<T>(jsonFile.text);
            }
            else
            {
                Debug.LogError($"JSON 파일을 찾을 수 없습니다: {fileName}");
                return default; // 기본값 반환
            }
        }

        /// <summary> Application.persistentDataPath에서 JSON 파일을 읽어 특정 데이터 타입으로 변환하는 함수 </summary>
        public static T LoadFromFile<T>(string fileName)
        {
            string filePath = Application.persistentDataPath + "/" + fileName;

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(json);
            }
            else
            {
                Debug.LogError($"파일을 찾을 수 없습니다: {filePath}");
                return default;
            }
        }

        /// <summary> 특정 데이터를 JSON 형식으로 저장하는 함수 </summary>
        public static void SaveToFile<T>(T data, string filePath)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"데이터 저장 완료: {filePath}");
        }
    }
}
