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
            public List<T> items;
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

                // T가 List<> 형태인지 검사
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    // JSON을 감싸는 래퍼 추가하여 변환
                    string wrappedJson = "{\"items\":" + jsonText + "}";
                    Type itemType = typeof(T).GetGenericArguments()[0]; // 리스트의 요소 타입 가져오기
                    Type wrapperType = typeof(Wrapper<>).MakeGenericType(itemType); // Wrapper<T> 생성
                    object wrapperInstance = JsonUtility.FromJson(wrappedJson, wrapperType); // 변환 실행
                    return (T)wrapperType.GetField("items").GetValue(wrapperInstance); // 리스트 추출
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
        public static void SaveToFile<T>(T data, string filePath)
        {
            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
            Debug.Log($"데이터 저장 완료: {filePath}");
        }
    }
}
