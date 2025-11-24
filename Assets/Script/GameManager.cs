using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 싱글톤 접근
    public static GameManager Instance;

    // 플레이어 변수
    public Transform player;
    // 오브젝트 풀 매니저
    public PoolManager pool;

    // 게임 데이터
    public float gameTime;
    public float maxGameTime = 600f; // 10분 제한
    public int killCount = 0;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        gameTime += Time.deltaTime;

        // 게임 종료 조건 (시간 초과 등)
        if (gameTime > maxGameTime)
        {
            // 게임 승리/종료 로직
            // gameTime = maxGameTime;
        }
    }

    // 플레이어가 죽거나 게임이 끝났을 때 처리할 함수들 추가 가능
}