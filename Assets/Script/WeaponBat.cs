using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBat : MonoBehaviour
{
    [Header("야구방망이 스펙")]
    public int damage = 8;       // 공격력
    public float coolTime = 1.5f; // 공격 속도 (쿨타임)

    [Header("프리팹 및 설정")]
    public GameObject swingEffectPrefab; // 이펙트 프리팹 연결
    public float spawnOffset = 0.5f;     // 캐릭터 중심에서 얼마나 떨어진 곳에 생성할지

    // 내부 변수
    float currentTime = 0.0f;
    Transform playerTransform;

    void Start()
    {
        // 내 부모(Player)를 찾아서 저장
        playerTransform = transform.parent;
    }

    void Update()
    {
        // 쿨타임 계산
        currentTime += Time.deltaTime;

        if (currentTime > coolTime)
        {
            currentTime = 0.0f;
            Swing(); // 공격 실행
        }
    }

    void Swing()
    {
        // 1. 플레이어가 보는 방향 확인 (1: 오른쪽, -1: 왼쪽)
        float facingDirection = Mathf.Sign(playerTransform.localScale.x);

        // 2. 생성 위치 계산 (보는 방향 쪽으로 약간 앞에서 생성)
        Vector3 spawnPos = playerTransform.position + new Vector3(facingDirection * spawnOffset, 0, 0);

        // 3. 이펙트 생성
        GameObject effect = Instantiate(swingEffectPrefab, spawnPos, Quaternion.identity);

        // 4. 이펙트 좌우 반전 (Scale X 뒤집기)
        Vector3 newScale = effect.transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * facingDirection;
        effect.transform.localScale = newScale;

        // 5. 회전 움직임 초기화 (방향 전달)
        // SwingMovement에게 "나 지금 오른쪽(1)이야" 혹은 "왼쪽(-1)이야"라고 알려줌.
        SwingMovement swingMove = effect.GetComponent<SwingMovement>();
        if (swingMove != null)
        {
            swingMove.Init(facingDirection);
        }

        // 6. 데미지 정보 전달
        Bullet b = effect.GetComponent<Bullet>();
        if (b != null)
        {
            Vector2 dir = new Vector2(facingDirection, 0);
            // Init(방향, 데미지, 관통O, 근접O)
            b.Init(dir, damage, true, true);
        }

        // 7. 0.3초 뒤 삭제 (애니메이션 끝나면 사라짐)
        Destroy(effect, 0.3f);
    }
}