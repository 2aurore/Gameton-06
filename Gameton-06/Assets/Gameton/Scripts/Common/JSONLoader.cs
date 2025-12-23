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

                // JSON인지 확인 (배열이면 직접 변환)
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

        /// <summary> Application.persistentDataPath 내의 파일 경로 생성성 </summary>
        private static string GetPersistentPath(string fileName)
        {
            return Path.Combine(Application.persistentDataPath, fileName + ".json");
        }

        /// <summary> Resources.Load 로 읽어온 파일을 persistentDataPath 경로에 저장 </summary>
        public static void SaveJsonToPersistentData(string fileName)
        {
            if (fileName.EndsWith(".json"))
            {
                fileName = fileName.Replace(".json", ""); // 확장자 제거
            }

            string persistentPath = GetPersistentPath(fileName);

            // persistentDataPath에 파일이 있는지 체크
            // Android에서는 파일이 이미 존재하면 덮어쓰지 않도록 함
            if (File.Exists(persistentPath))
            {
                Debug.Log($"⚠ {fileName}.json 파일이 이미 존재합니다. 덮어쓰지 않습니다. ({persistentPath})");
                return;
            }
            // Resources에서 JSON 불러오기
            string path = DATA_PATH + fileName; // Resources 폴더 내 경로
            TextAsset jsonFile = Resources.Load<TextAsset>(path);

            if (jsonFile != null)
            {
                File.WriteAllText(persistentPath, jsonFile.text);
                Debug.Log($"JSON 저장 완료: {persistentPath}");
            }
            else
            {
                Debug.LogError($"Resources에서 JSON 파일을 찾을 수 없음: {path}");
                File.WriteAllText(persistentPath, "[]");
            }
        }



        /// <summary> persistentDataPath 경로의 파일 읽어오기 </summary>
        public static T LoadJsonFromPersistentData<T>(string fileName)
        {
            string path = GetPersistentPath(fileName);
            // Debug.Log($"LoadJsonFromPersistentData : {path}");

            if (!File.Exists(path))
            {
                Debug.LogError($"파일을 찾을 수 없습니다: {path}");
                return default;
            }

            string jsonText = File.ReadAllText(path);
            Debug.Log($"JSON 로드: {jsonText}");

            // 리스트(JSON 배열)인지 확인
            if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
            {
                if (jsonText.StartsWith("["))
                {
                    return JsonUtility.FromJson<Wrapper<T>>(WrapArray(jsonText)).items;
                }
                else
                {
                    Wrapper<T> wrapperInstance = JsonUtility.FromJson<Wrapper<T>>(jsonText);
                    return wrapperInstance.items;
                }
            }

            // 일반 객체 변환
            return JsonUtility.FromJson<T>(jsonText);
        }

        /// <summary> persistentDataPath 경로의 파일 데이터 업데이트 </summary>
        public static bool SaveUpdatedJsonToPersistentData<T>(T updatedData, string fileName)
        {
            try
            {
                string path = GetPersistentPath(fileName);
                string json;

                // 리스트인지 확인 후 JSON 변환
                if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
                {
                    Wrapper<T> wrapper = new Wrapper<T> { items = updatedData };
                    json = JsonUtility.ToJson(wrapper, true);
                }
                else
                {
                    json = JsonUtility.ToJson(updatedData, true);
                }

                // 파일 저장
                File.WriteAllText(path, json);
                Debug.Log($"JSON 데이터 업데이트 완료: {path}");

                return true; // 저장 성공
            }
            catch (Exception ex)
            {
                Debug.LogError($"JSON 저장 실패: {ex.Message}");
                return false; // 저장 실패
            }
        }


    }
}
