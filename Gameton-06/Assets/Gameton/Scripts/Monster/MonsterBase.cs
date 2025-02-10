using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using Assets.PixelFantasy.PixelMonsters.Common.Scripts;
using Unity.VisualScripting;
using UnityEngine;
using DamageCalculator = TON.DamageCalculator;
using Vector3 = UnityEngine.Vector3;

namespace TON
{
    public class MonsterBase : MonoBehaviour, IDamage
    {
        public int id;  // 적 고유 ID
        public float currentHP = 100;  // HP

        // public string name; // 몬스터 명 or 프리팹 명
        public string monsterType;  // 몬스터 타입 ex : melee, ranged
        
        // public int damage;  // 공격력
        public float speed = 2; // 이동속도
        
        [SerializeField]
        private SpriteRenderer _spriteRenderer;
        private Animator _animator;

        private Vector3 _direction;
        private bool _isWalking;
        private bool _isHit;
        private bool _isDetect;
        private float _currentTime;

        [SerializeField]
        private int _idleTime = 2;

        [SerializeField]
        private int walkingTime = 2;

        [SerializeField]
        private GameObject _target;
        
        [SerializeField]
        private Collider2D _collider;

        public float defencePower;
        
        public bool IsWalking
        {
            get => _isWalking;
            set => _isWalking = value;
        }

        public bool IsDetect
        {
            get => _isDetect;
            set => _isDetect = value;
        }

        public Dictionary<string, Monster> dicMonster = new Dictionary<string, Monster>(); // 초기화

        [System.Serializable]
        public class Monster
        {
            public int id;
            public string name;
            public int level;
            public int hp;
            public int attackPower;
            public int defencePoser;
        }
        
        // Start is called before the first frame update
        void Start()
        {
            
            ReadCSV();

            // dicMonster 사용 예시
            if (dicMonster.ContainsKey("1")) // 키 존재 확인
            {
                // Debug.Log(dicMonster["1"].name);
            }
            
            _currentTime = Time.realtimeSinceStartup;

            _animator = GetComponent<Animator>();

            _direction = new Vector3(1, 0, 0);

            _spriteRenderer.flipX = !(_direction.x > 0);
            _collider = GetComponent<Collider2D>();
            
            // TODO: 몬스터 방어력 임시값
            defencePower = 10f;
        }

        private void ReadCSV()
        {
            TextAsset csvFile = Resources.Load<TextAsset>("Monster");

            if (csvFile == null)
            {
                Debug.LogError("CSV 파일 로드 실패: Monster");
                return;
            }

            StringReader reader = new StringReader(csvFile.text);
            string line;
            bool isFirstLine = true; // 첫 번째 줄은 헤더로 건너뛰기

            while ((line = reader.ReadLine()) != null)
            {
                if (isFirstLine)
                {
                    isFirstLine = false;
                    continue; // 헤더 건너뛰기
                }

                var splitData = line.Split(',');

                Monster monster = new Monster();

                // int.TryParse를 사용하여 안전하게 정수형으로 변환
                if (int.TryParse(splitData[0], out monster.id)) { }
                else { Debug.LogError("id 필드 정수 변환 실패: " + splitData[0]); }

                monster.name = splitData[1];

                if (int.TryParse(splitData[2], out monster.level)) { }
                else { Debug.LogError("level 필드 정수 변환 실패: " + splitData[2]); }

                if (int.TryParse(splitData[3], out monster.hp)) { }
                else { Debug.LogError("hp 필드 정수 변환 실패: " + splitData[3]); }

                if (int.TryParse(splitData[4], out monster.attackPower)) { }
                else { Debug.LogError("attackPower 필드 정수 변환 실패: " + splitData[4]); }

                if (int.TryParse(splitData[5], out monster.defencePoser)) { }
                else { Debug.LogError("defencePoser 필드 정수 변환 실패: " + splitData[5]); }

                dicMonster.Add(monster.id.ToString(), monster); // id를 키로 사용
            }
        }

        // Update is called once per frame
        void Update()
        {
            // todo : 시야를 벗어났으면 idle 전환
            
            if (_isWalking)
            {
                // walking 상태에서 walkingTime을 초과할 경우 idle 애니메이션 재생
                transform.Translate(_direction * speed * Time.deltaTime);

                if (Time.realtimeSinceStartup - _currentTime >= walkingTime)
                {
                    _isWalking = false;
                    _currentTime = Time.realtimeSinceStartup;
                }
            }
            else
            {
                // 지금 기다렸던 시간이 idleTime을 초과할 경우에 walk 애니메이션 재생
                if (Time.realtimeSinceStartup - _currentTime >= _idleTime)
                {
                    // 초기화
                    _currentTime = Time.realtimeSinceStartup;

                    if (_isWalking == false)
                    {
                        _direction *= -1;
                        _spriteRenderer.flipX = !(_direction.x > 0);
                    }

                    _isWalking = true;
                }
            }
            
            // 걷기 애니메이션으로 변경
            _animator.SetBool("Walk", _isWalking); // 걷기 애니메이션
        }

        public void ApplyDamage(float damage)
        {
            // 몬스터 피해 값 코드
            // 체력이 감소되는 부분
            // 몬스터 죽고 파괴되는 부분
            // 피격됐을때 애니메이션

            float prevHP = currentHP;
            currentHP -= damage;

            if (prevHP > 0 && currentHP <= 0)
            {
                // _animator.SetBool("Die", true);  // 몬스터 죽는 애니메이션 트리거
                Destroy(gameObject);  // 몬스터 파괴
            }
            else if (prevHP > 0 && currentHP > 0)
            {
                // _animator.SetBool("Hit", true); // 피격 애니메이션
            }
        }
        
        public void Attack(GameObject player)
        {
            _animator.SetTrigger("Attack");
            // 임시 반영 수정 예정
            DamageCalculator damageCalculator = new DamageCalculator();

            float baseAttack = 30f;
            float equipmentAttack = 10f;
            float defense = 0.1f;

            // 기본 데미지 계산 (치명타 없음)
            float damage = damageCalculator.CalculateBaseDamage(baseAttack, equipmentAttack, defense);

            Debug.Log($" 몬스터 공격! 최종 데미지: {damage}");
        }

        public void Detect(GameObject target)
        {
            if (target == null) return; // 타겟이 null이면 함수 종료

            // 타겟의 위치에서 내 현재 위치를 빼서 방향 벡터를 구함
            UnityEngine.Vector2 direction = target.transform.position - transform.position;

            // 방향 벡터의 크기가 0이 아니면 (즉, 타겟이 존재하면) 이동
            if (direction.magnitude > 0)
            {
                // 방향 벡터를 정규화하여 길이를 1로 만들고, 속도를 곱하여 이동
                transform.Translate(direction.normalized * speed * Time.deltaTime);

                // 걷기 애니메이션 재생
                _animator.SetBool("Walk", true);

                // 타겟 방향으로 몬스터 이미지 뒤집기
                _spriteRenderer.flipX = direction.x < 0; 
            }
            else
            {
                // 타겟과 몬스터가 같은 위치에 있으면 걷기 애니메이션 중지
                _animator.SetBool("Walk", false);
            }
        }
    }
    
}
