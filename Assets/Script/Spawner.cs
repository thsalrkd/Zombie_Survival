using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // 소환 타이머
    float timer;
    // 소환 레벨 (웨이브 난이도 조절용)
    int level;

    // 초기 소환 주기 (0.5초마다 소환)
    public float spawnTime = 0.5f;

    void Update()
    {
        // 게임 시간이 흐름에 따라 난이도 조절
        // GameManager의 gameTime을 가져와서 계산
        if (GameManager.Instance.gameTime > 60f) spawnTime = 0.3f; // 1분 뒤 빨라짐
        if (GameManager.Instance.gameTime > 120f) spawnTime = 0.1f; // 2분 뒤 더 빨라짐

        timer += Time.deltaTime;

        if (timer > spawnTime)
        {
            timer = 0;
            Spawn();
        }
    }

    void Spawn()
    {
        // 풀 매니저에서 적 가져오기 (Index 0: 좀비)
        GameObject enemy = GameManager.Instance.pool.Get(0);

        // 스폰 위치 설정 (플레이어 주변 랜덤 좌표)
        // insideUnitCircle은 반지름 1인 원 안의 랜덤 좌표를 줌 -> 20을 곱해서 넓게 퍼트림
        Vector2 spawnPos = GameManager.Instance.player.position;

        // 플레이어 위치 + 랜덤 방향 * 거리(반지름)
        // 화면 밖에서 나와야 하므로 최소 거리를 더해줌
        Vector2 randomDir = Random.insideUnitCircle.normalized;
        spawnPos += randomDir * Random.Range(10f, 15f);

        enemy.transform.position = spawnPos;

        // 적 스탯을 게임 시간에 따라 강화시킬 수도 있음 (옵션)
        enemy.GetComponent<Enemy>().maxHp += GameManager.Instance.gameTime * 0.1f;
    }
}