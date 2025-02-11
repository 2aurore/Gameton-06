using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace TON
{
    public class JSONLoader : MonoBehaviour
    {
        private const string DATA_PATH = "GameData/";

        // 제네릭 리스트를 감싸기 위한 래퍼 클래스
        [Serializable]
        private class Wrapper<T>
        {
            public T items;
        }

        /// <summary> JSON 배열을 강제로 Wrapper<T> 형태로 감싸기 위한 함수 </summary>
        private static string WrapArray(string jsonArray)
        {
            return "{\"items\":" + jsonArray + "}";
        }


        /// <summary> Resources 폴더에서 JSON 파일을 읽어 특정 데이터 타입으로 변환하는 함수 </summary>
        public static T LoadFromResources<T>(string fileName)
        {

            if (fileName.EndsWith(".json"))
            {
                fileName = fileName.Replace(".json", ""); // 확장자 제거
            }

            string path = DATA_PATH + $"{fileName}";
            TextAsset jsonFile = Resources.Load<TextAsset>(path);

            if (jsonFile != null)
            {
                string jsonText = jsonFile.text;

                // 🎯 [배열] JSON인지 확인 (배열이면 직접 변환)
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    if (jsonText.StartsWith("["))
                    {
                        return JsonUtility.FromJson<Wrapper<T>>(WrapArray(jsonText)).items;
                    }
                    else
                    {
                        // JSON이 Wrapper<T>로 감싸져 있는지 확인 후 변환
                        Wrapper<T> wrapperInstance = JsonUtility.FromJson<Wrapper<T>>(jsonText);
                        return wrapperInstance.items;
                    }
                }
                // 일반 객체 변환
                return JsonUtility.FromJson<T>(jsonText);
            }
            else
            {
                Debug.LogError($"JSON 파일을 찾을 수 없습니다: {path}");
                return default;
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
        public static void SaveToFile<T>(T data, string fileName)
        {
            string path = $"Assets/Gameton/Resources/{DATA_PATH}{fileName}.json";
            string json;

            // [리스트] 형식인지 확인
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                // 리스트 데이터를 감싸는 래퍼 클래스를 사용하여 JSON 변환
                Wrapper<T> wrapper = new Wrapper<T> { items = data };
                json = JsonUtility.ToJson(wrapper, true);
            }
            else
            {
                // 일반 객체는 그대로 JSON 변환
                json = JsonUtility.ToJson(data, true);
            }
            Debug.Log("SaveToFile ::: " + json);

            File.WriteAllText(path, json);
            Debug.Log($"파일 저장 성공 ::: {fileName}.json");
        }

    }
}
