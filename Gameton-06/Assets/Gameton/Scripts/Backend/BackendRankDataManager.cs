using System.Collections;
using System.Collections.Generic;
using BackEnd;
using UnityEngine;

namespace TON
{
    /// <summary>
    /// 뒤끝 서버 게임 클리어 데이터 관리 담당 클래스
    /// </summary>
    public class BackendRankDataManager
    {
        // 테이블 이름 상수
        private const string RANK_TABLE = "RANK_DATA";


        // 내 게임 클리어 데이터 조회
        public void LoadMyRankData(System.Action<ClearData> onComplete)
        {
            ClearData clearData = new ClearData();

            Backend.PlayerData.GetMyData(RANK_TABLE, callback =>
            {
                if (callback.IsSuccess() == false)
                {
                    Debug.Log("데이터 읽기 중에 문제가 발생했습니다 : " + callback.ToString());
                    onComplete?.Invoke(null);
                    return;
                }

                // 불러오기에는 성공했으나 데이터가 존재하지 않는 경우
                if (callback.IsSuccess() && callback.FlattenRows().Count <= 0)
                {
                    Debug.Log("데이터가 존재하지 않습니다");
                    InsertInitData(clearData, () =>
                    {
                        // 초기 데이터 삽입 후 다시 데이터를 불러옴
                        LoadDataAfterInsert(onComplete);
                    });
                    return;
                }

                // 1개 이상 데이터를 불러온 경우
                if (callback.FlattenRows().Count > 0)
                {
                    clearData.nickname = callback.FlattenRows()[0]["nickname"].ToString();
                    clearData.score = int.Parse(callback.FlattenRows()[0]["score"].ToString());
                    clearData.wave = int.Parse(callback.FlattenRows()[0]["wave"].ToString());
                    clearData.playTime = float.Parse(callback.FlattenRows()[0]["play_time"].ToString());

                    onComplete?.Invoke(clearData); // 성공 시 데이터 반환
                }
            });
        }

        // 데이터 삽입 후 다시 불러오는 메소드
        private void LoadDataAfterInsert(System.Action<ClearData> onComplete)
        {
            Backend.PlayerData.GetMyData(RANK_TABLE, callback =>
            {
                ClearData clearData = new ClearData();

                if (callback.IsSuccess() && callback.FlattenRows().Count > 0)
                {
                    clearData.nickname = callback.FlattenRows()[0]["nickname"].ToString();
                    clearData.score = int.Parse(callback.FlattenRows()[0]["score"].ToString());
                    clearData.wave = int.Parse(callback.FlattenRows()[0]["wave"].ToString());
                    clearData.playTime = float.Parse(callback.FlattenRows()[0]["play_time"].ToString());
                }

                onComplete?.Invoke(clearData);
            });
        }

        /// <summary>
        /// 캐릭터 초기 생성 시 row 삽입
        /// </summary>
        private void InsertInitData(ClearData clearData, System.Action onComplete = null)
        {
            Param param = new Param();
            param.Add("score", clearData.score);
            param.Add("wave", clearData.wave);
            param.Add("play_time", clearData.playTime);
            param.Add("nickname", PlayerDataManager.Singleton.player.name);

            Backend.PlayerData.InsertData(RANK_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("초기 랭크 데이터 삽입 성공");
                }
                else
                {
                    Debug.LogError("초기 랭크 데이터 삽입 실패: " + callback.ToString());
                }

                onComplete?.Invoke();
            });
        }

        /// <summary>
        /// 플레이 랭크 정보 업데이트
        /// </summary>
        private void UpdateRankData(ClearData clearData, System.Action onComplete = null)
        {
            Param param = new Param();
            param.Add("score", clearData.score);
            param.Add("wave", clearData.wave);
            param.Add("play_time", clearData.playTime);
            param.Add("nickname", clearData.nickname);

            Backend.PlayerData.UpdateMyLatestData(RANK_TABLE, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("랭크 정보 업데이트 성공");
                    onComplete?.Invoke();
                }
                else
                {
                    Debug.LogError("랭크 정보 업데이트 실패: " + callback.ToString());
                }

            });
        }

        /// <summary>
        /// 캐릭터 플레이 랭크 정보 비교 후 업데이트 메소드 실행
        /// </summary>
        public void CheckUpdateRankData(ClearData clearData, System.Action onComplete = null)
        {
            // 먼저 현재 테이블에서 최근 데이터를 가져옵니다
            Backend.GameData.GetMyData(RANK_TABLE, new Where(), 1, callback =>
            {
                if (callback.IsSuccess())
                {
                    // 기존 데이터가 있는지 확인
                    if (callback.FlattenRows().Count > 0)
                    {
                        // 최근 데이터 추출
                        var latestData = callback.FlattenRows()[0];
                        int latestWave = int.Parse(latestData["wave"].ToString());
                        long latestScore = long.Parse(latestData["score"].ToString());
                        float latestPlayTime = float.Parse(latestData["play_time"].ToString());

                        // 신규 데이터와 비교하여 업데이트 여부 결정
                        bool shouldUpdate = false;

                        // 업데이트 조건 설정 (예: 웨이브가 높거나, 웨이브가 같으면서 점수가 높거나, 웨이브와 점수가 같으면서 플레이 시간이 짧을 때)
                        if (clearData.wave > latestWave)
                        {
                            shouldUpdate = true;
                        }
                        else if (clearData.wave == latestWave && clearData.score > latestScore)
                        {
                            shouldUpdate = true;
                        }
                        else if (clearData.wave == latestWave && clearData.score == latestScore && clearData.playTime < latestPlayTime)
                        {
                            shouldUpdate = true;
                        }

                        if (shouldUpdate)
                        {
                            UpdateRankData(clearData, onComplete);
                        }
                        else
                        {
                            Debug.Log("기존 기록이 더 좋아 업데이트하지 않음");
                        }
                    }
                    else
                    {
                        // 기존 데이터가 없으면 무조건 삽입
                        InsertInitData(clearData, onComplete);
                    }
                }
                else
                {
                    Debug.LogError("기존 데이터 조회 실패: " + callback.ToString());
                }
            });
        }


        /// <summary>
        /// 플레이어 랭킹 리스트 불러오기
        /// </summary>
        public void GetRankData(System.Action<LitJson.JsonData> onComplete = null)
        {
            // 가져올 필드 지정 (모든 필드를 가져오려면 null 사용)
            string[] select = new string[] { "nickname", "wave", "score", "play_time" };

            // 결과 제한 수 (100개)
            int limit = 100;

            // 첫 번째 정렬 기준 (wave 내림차순)
            string firstKey = "wave";

            // 데이터 비동기 요청
            Backend.GameData.Get(RANK_TABLE, new Where(), select, limit, firstKey, TableSortOrder.DESC, bro =>
            {
                // 요청 성공 확인
                if (bro.IsSuccess())
                {
                    // 데이터 처리
                    LitJson.JsonData rankData = bro.GetReturnValuetoJSON()["rows"];
                    Debug.Log("가져온 데이터 수: " + rankData.Count);

                    onComplete?.Invoke(rankData);
                }
                else
                {
                    // 오류 처리
                    Debug.LogError($"랭크 데이터 가져오기 실패: {bro.GetMessage()}");
                    Debug.LogError($"랭크 데이터 가져오기 실패: {bro.GetErrorCode()}");
                    Debug.LogError($"랭크 데이터 가져오기 실패: {bro.GetStatusCode()}");
                }
            });

        }

    }
}
