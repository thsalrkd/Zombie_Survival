using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Playermove Player;

    [Header("스테이지 설정")]
    public int currentStage = 1;
    public float stageTimer = 0f;
    public float stageDuration = 60f; // 1분
    public bool isBossStage = false;

    [Header("보스 프리팹 설정")]
    public GameObject boss1Prefab;    // 3스테이지 종료 후 등장
    public GameObject boss2Prefab;    // 6스테이지 종료 후 등장

    public Transform player;

    [Header("UI 관련 설정")]
    public bool isLive;
    public Result uiResult;

    EnemySpawner enemySpawner;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;

        enemySpawner = FindObjectOfType<EnemySpawner>();
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
        isLive = false;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Update()
    {
        if (!isLive)
            return;

        // 보스전이 아닐 때만 시간 흐름
        if (!isBossStage)
        {
            stageTimer += Time.deltaTime;

            // ★ 1분이 지났을 때의 행동 결정
            if (stageTimer >= stageDuration)
            {
                CheckStageEnd();
            }
        }
    }

    // ★ 1분이 끝났을 때 무슨 일이 일어날지 결정하는 함수
    void CheckStageEnd()
    {
        // 3스테이지가 끝났다면 -> 보스 1 소환
        if (currentStage == 3)
        {
            StartBossBattle(boss1Prefab);
        }
        // 6스테이지가 끝났다면 -> 보스 2 소환
        else if (currentStage == 6)
        {
            StartBossBattle(boss2Prefab);
        }
        // 그 외 스테이지(1,2,4,5)가 끝났다면 -> 다음 스테이지로 이동
        else
        {
            NextStage();
        }
    }

    void NextStage()
    {
        // 스테이지 증가 및 타이머 초기화
        if (currentStage < 6)
        {
            currentStage++;
            stageTimer = 0f;
            Debug.Log("다음 스테이지 진입! 현재: " + currentStage);
        }

        // 4스테이지 진입 시 (도시 맵 등) 처리
        if (currentStage == 4)
        {
            Debug.Log("2라운드 시작! 도시 맵 분위기!");
        }
    }

    void StartBossBattle(GameObject bossPrefab)
    {
        isBossStage = true; // 타이머 정지
        stageTimer = 0f;    // 타이머 초기화 (보기 좋게)

        // ★ 보스전 시작! 잡몹 스폰 중지
        if (enemySpawner != null) enemySpawner.enabled = false;

        // 보스 소환 (플레이어 주변)
        if (bossPrefab != null)
        {
            Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 5f; // 너무 멀지 않게 5f로 수정
            Instantiate(bossPrefab, spawnPos, Quaternion.identity);
            Debug.Log("보스 출현!");
        }
        else
        {
            Debug.LogError("보스 프리팹이 연결되지 않았습니다! Inspector를 확인하세요.");
        }
    }

    // 보스가 죽었을 때 호출 (Enemy.cs에서 호출)
    public void OnBossDead()
    {
        if (isBossStage)
        {
            isBossStage = false; // 보스전 끝
            Debug.Log("보스 처치 완료!");

            // ★ 보스를 잡았으니 다시 잡몹 나오게 켬
            if (enemySpawner != null) enemySpawner.enabled = true;

            if (currentStage == 3)
            {
                // 3스테이지 보스 잡음 -> 4스테이지로 이동
                NextStage();
            }
            else if (currentStage == 6)
            {
                // 6스테이지 보스 잡음 -> 게임 클리어
                GameClear();
            }
        }
    }

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