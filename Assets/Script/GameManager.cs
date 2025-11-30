using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // 나중에 씬 이동 등을 위해 필요

public class GameManager : MonoBehaviour
{
    // 어디서든 접근 가능하게 싱글톤 처리
    public static GameManager instance;
    public Playermove Player;

    [Header("스테이지 설정")]
    public int currentStage = 1;      // 현재 스테이지 (1~6)
    public float stageTimer = 0f;     // 현재 스테이지 진행 시간
    public float stageDuration = 60f; // 스테이지 당 시간 (1분)
    public bool isBossStage = false;  // 현재 보스전 중인가?

    [Header("적 프리팹 설정")]
    public GameObject[] round1Mobs;   // 1라운드(1~3스테이지) 잡몹들
    public GameObject[] round2Mobs;   // 2라운드(4~6스테이지) 잡몹들
    public GameObject boss1Prefab;    // 1라운드 보스 (스테이지 3)
    public GameObject boss2Prefab;    // 2라운드 보스 (스테이지 6)

    [Header("스폰 설정")]
    public float spawnInterval = 1.5f; // 스폰 간격
    public float spawnRadius = 12.0f;  // 스폰 반경

    // 내부 변수
    float spawnTimer = 0f;
    public Transform player;

    [Header("UI 관련 설정")]
    public bool isLive;
    public Result uiResult;

    void Awake()
    {
        instance = this; // 나 자신을 전역 변수에 할당
    }

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;
    }

    public void GameStart()
    {  
        Resume();
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive=false; 
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Over();
        Stop();
    }

    public void GameClear()
    {
        Debug.Log("게임 클리어! 축하합니다!");
        StartCoroutine(GameClearRoutine());
    }

    IEnumerator GameClearRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        uiResult.gameObject.SetActive(true);
        uiResult.Clear();
        Stop();
    }

    public void GameRetry()
    {
        SceneManager.LoadScene("Character 1");
    }

    void Update()
    {
        if (!isLive)
            return;

        if (player == null) return;

        // --- 스테이지 관리 로직 ---
        if (!isBossStage)
        {
            // 보스 스테이지가 아닐 때는 시간이 흐름
            stageTimer += Time.deltaTime;

            // 1분이 지나면 다음 스테이지로
            if (stageTimer >= stageDuration)
            {
                NextStage();
            }
        }

        // --- 적 스폰 로직 ---
        spawnTimer += Time.deltaTime;

        // 보스가 이미 소환된 상태(isBossStage)에서는 잡몹을 더 뽑을지 말지 결정
        if (spawnTimer > spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }

    void NextStage()
    {
        currentStage++; // 스테이지 증가
        stageTimer = 0f; // 타이머 초기화
        Debug.Log("스테이지 이동! 현재 스테이지: " + currentStage);

        // 스테이지 3 진입 -> 보스 1 출현
        if (currentStage == 3)
        {
            StartBossBattle(boss1Prefab);
        }
        // 스테이지 6 진입 -> 보스 2 출현
        else if (currentStage == 6)
        {
            StartBossBattle(boss2Prefab);
        }
        // 스테이지 4 진입 -> 2라운드 시작 (난이도 상승 등)
        else if (currentStage == 4)
        {
            spawnInterval = 1.0f; // 스폰 속도 빨라짐
            Debug.Log("2라운드 시작! 도시 맵 분위기!");
        }
    }

    void StartBossBattle(GameObject bossPrefab)
    {
        isBossStage = true; // 타이머 정지

        // 보스 소환 (플레이어 근처 좀 멀리)
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        Debug.Log("보스 출현!! 처치해야 다음으로 넘어갑니다.");
    }

    // 적(Boss)이 죽었을 때 호출하는 함수
    public void OnBossDead()
    {
        if (isBossStage)
        {
            isBossStage = false; // 보스전 끝
            Debug.Log("보스 처치 완료!");

            if (currentStage == 3)
            {
                // 3스테이지 보스를 잡으면 4스테이지로 바로 진입
                NextStage();
            }
            else if (currentStage == 6)
            {
                // 6스테이지 보스를 잡으면 게임 클리어

                GameClear();
            }
        }
    }

    void SpawnEnemy()
    {
        // 현재 라운드에 맞는 잡몹 배열 선택
        GameObject[] currentMobs = (currentStage <= 3) ? round1Mobs : round2Mobs;

        if (currentMobs.Length == 0) return;

        // 랜덤 위치 계산
        Vector2 randomPoint = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = player.position + new Vector3(randomPoint.x, randomPoint.y, 0);

        // 랜덤 몹 선택
        int randIdx = Random.Range(0, currentMobs.Length);
        Instantiate(currentMobs[randIdx], spawnPos, Quaternion.identity);
    }

    /*void GameClear()
    {
        Debug.Log("게임 클리어! 축하합니다!");
        Time.timeScale = 0; // 게임 정지
        // 여기에 클리어 UI 띄우기
    }*/

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;

    }
}