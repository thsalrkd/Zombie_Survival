using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    public GameObject[] enemyPrefabs; // 적 프리팹들 (0:일반, 1:특수 등)
    public float spawnInterval = 2.0f; // 적이 생성되는 간격 (초)
    public float spawnRadius = 10.0f;  // 플레이어로부터 떨어진 거리 (화면 밖)

    [Header("게임 난이도 (시간 경과에 따라)")]
    public float timeAfterSpawn = 0f;  // 게임 시작 후 흐른 시간

    // 내부 변수
    float timer = 0f;
    Transform player;

    void Start()
    {
        // 플레이어 위치 찾기 (매 프레임 찾으면 무거우니까 시작할 때 한 번)
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

            // (선택사항) 시간이 지날수록 스폰 속도를 조금씩 빠르게
            // 0.5초 밑으로는 안 떨어지게 제한
            if (spawnInterval > 0.5f)
            {
                spawnInterval -= 0.01f;
            }
        }
    }

    void SpawnEnemy()
    {
        // 1. 생성 위치 계산 (플레이어 주변 원형 테두리 어딘가)
        // insideUnitCircle은 원 안의 랜덤 점, normalized하면 원의 테두리 점이 됨
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomPoint.x, randomPoint.y, 0);

        // 2. 어떤 적을 소환할지 결정
        // 기본은 0번(일반 좀비), 가끔 1번(특수 좀비)
        GameObject enemyToSpawn = enemyPrefabs[0];

        // 프리팹이 2개 이상 등록되어 있고, 20% 확률로 특수몹 소환
        if (enemyPrefabs.Length > 1 && Random.Range(0, 100) < 20)
        {
            enemyToSpawn = enemyPrefabs[1];
        }

        // 3. 적 생성
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