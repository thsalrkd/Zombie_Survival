using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("보스 프리팹 설정")]
    // 잡몹들은 EnemySpawner에서 관리하므로 여기서 제거
    public GameObject boss1Prefab;    // 1라운드 보스 (스테이지 3)
    public GameObject boss2Prefab;    // 2라운드 보스 (스테이지 6)

    // 내부 변수
    public Transform player;

    [Header("UI 관련 설정")]
    public bool isLive;
    public Result uiResult;

    // 스포너 제어용 변수
    EnemySpawner enemySpawner;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) player = p.transform;

        // 씬에 있는 EnemySpawner를 찾아둠
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // 현재 씬 다시 로드
    }

    void Update()
    {
        if (!isLive)
            return;

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

        // 적 스폰 로직은 여기서 삭제됨 (EnemySpawner가 수행함)
    }

    void NextStage()
    {
        // 보스전이 끝나고 다음 스테이지로 넘어갈 때 타이머 초기화
        if (currentStage < 6)
        {
            currentStage++;
            stageTimer = 0f;
            Debug.Log("스테이지 이동! 현재 스테이지: " + currentStage);
        }

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
        // 스테이지 4 진입 -> 배경 변경 및 난이도 상승 (EnemySpawner가 알아서 처리함)
        else if (currentStage == 4)
        {
            Debug.Log("2라운드 시작! 도시 맵 분위기!");
            // 여기서 배경(Ground) 이미지를 도시로 바꾸는 코드를 넣을 수 있습니다.
        }
    }

    void StartBossBattle(GameObject bossPrefab)
    {
        isBossStage = true; // 타이머 정지

        // 보스전 때는 잡몹 스폰 중지
        if (enemySpawner != null) enemySpawner.enabled = false;

        // 보스 소환 (플레이어 근처 좀 멀리)
        Vector2 spawnPos = (Vector2)player.position + Random.insideUnitCircle.normalized * 10f;
        Instantiate(bossPrefab, spawnPos, Quaternion.identity);

        Debug.Log("보스 출현!! 처치해야 다음으로 넘어갑니다.");
    }

    // 적(Boss)이 죽었을 때 호출하는 함수 (Enemy.cs에서 호출됨)
    public void OnBossDead()
    {
        if (isBossStage)
        {
            isBossStage = false; // 보스전 끝
            Debug.Log("보스 처치 완료!");

            // 보스가 죽었으니 다시 잡몹 스폰 시작
            if (enemySpawner != null) enemySpawner.enabled = true;

            if (currentStage == 3)
            {
                NextStage();
            }
            else if (currentStage == 6)
            {
                // 6스테이지 보스를 잡으면 게임 클리어
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