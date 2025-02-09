using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TON
{
    using System.Collections.Generic;
    using System.IO;
    using UnityEngine;

    public class MonsterDataManager : MonoBehaviour
    {
        public static MonsterDataManager Instance { get; private set; }
        public Dictionary<int, MonsterData> monsterDataDict = new Dictionary<int, MonsterData>();

        // void Awake()
        // {
        //     if (Instance == null)
        //     {
        //         Instance = this;
        //     }
        //     else
        //     {
        //         Destroy(gameObject);
        //         return;
        //     }
        //
        //     LoadMonsterData("monster_data.csv"); // CSV 파일 이름
        // }

        // void LoadMonsterData(string fileName)
        // {
        //     List<string[]> data = ReadCSV(fileName);
        //
        //     // 첫 번째 행은 헤더이므로 건너뜀
        //     for (int i = 1; i < data.Count; i++)
        //     {
        //         string[] row = data[i];
        //         MonsterData monsterData = new MonsterData();
        //
        //         monsterData.id = int.Parse(row[0]);
        //         monsterData.name = row[1];
        //         monsterData.attackPower = int.Parse(row[2]);
        //         monsterData.health = int.Parse(row[3]);
        //         monsterData.speed = float.Parse(row[4]);
        //
        //         monsterDataDict.Add(monsterData.id, monsterData);
        //     }
        // }
        //
        // List<string[]> ReadCSV(string fileName)
        // {
        //     List<string[]> data = new List<string[]>();
        //     TextAsset textAsset = Resources.Load<TextAsset>(fileName); // Resources 폴더에서 파일 로드
        //
        //     using (StringReader reader = new StringReader(textAsset.text))
        //     {
        //         while (!reader.EndOfStream)
        //         {
        //             string line = reader.ReadLine();
        //             string[] row = line.Split(',');
        //             data.Add(row);
        //         }
        //     }
        //
        //     return data;
        // }
    }
}
    
// 몬스터 데이터 클래스
public class MonsterData
{
    public int id;
    public string name;
    public int attackPower;
    public int health;
    public float speed;
}