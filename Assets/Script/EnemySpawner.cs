using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject[] enemyPrefabs; // 적 프리팹

    public float spawnInterval = 2.0f; // 적이 생성되는 간격 (초)
    public float spawnRadius = 10.0f;  // 플레이어로부터 떨어진 거리 (화면 밖)

    [Header("게임 난이도 (시간 경과에 따라)")]
    public float timeAfterSpawn = 0f;  // 게임 시작 후 흐른 시간

    // 내부 변수
    float timer = 0f;
    Transform player;

    void Start()
    {
        // 플레이어 위치 찾기
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (player == null) return; // 플레이어가 죽으면 스폰 중단

        timer += Time.deltaTime;
        timeAfterSpawn += Time.deltaTime;

        // 스폰 주기가 되면 적 생성
        if (timer > spawnInterval)
        {
            timer = 0f;
            SpawnEnemy();

            // 시간이 지날수록 스폰 속도를 조금씩 빠르게
            // 0.5초 밑으로는 안 떨어지게 제한
            if (spawnInterval > 0.5f)
            {
                spawnInterval -= 0.01f;
            }
        }
    }

    void SpawnEnemy()
    {
        // 생성 위치 계산 (플레이어 주변 원형 테두리 어딘가)
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomPoint.x, randomPoint.y, 0);

        int baseIndex = 0;    // 숲 잡몹
        int specialIndex = 1; // 숲 특수몹

        if (GameManager.instance.currentStage >= 4)
        {
            baseIndex = 2;    // 도시 잡몹
            specialIndex = 3; // 도시 특수몹
        }

        // 어떤 적을 소환할지 결정
        // 기본은 0번(일반몹), 가끔 1번(특수몹)
        GameObject enemyToSpawn = enemyPrefabs[baseIndex];

        //3%확률로 특수몹
        if (Random.Range(0, 100) < 3)
        {
            enemyToSpawn = enemyPrefabs[specialIndex];
        }

        // 적 생성
        Instantiate(enemyToSpawn, spawnPos, Quaternion.identity);
    }

    // 에디터에서 스폰 범위를 눈으로 보기 위한 기능 (노란 원)
    private void OnDrawGizmos()
    {
        if (player != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(player.position, spawnRadius);
        }
    }
} 