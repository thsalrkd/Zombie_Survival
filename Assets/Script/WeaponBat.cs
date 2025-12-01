using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBat : MonoBehaviour
{
    [Header("삽 스펙")]
    public int damage = 5;       // 공격력
    public float coolTime = 3.0f; // 쿨타임

    [Header("크기 설정")]
    // 이 숫자를 조절하면 삽 크기가 바뀜
    public float scaleMultiplier = 3.0f;

    [Header("프리팹 및 설정")]
    public GameObject swingEffectPrefab; // 이펙트 프리팹 연결
    public float spawnOffset = 0.5f;     // 캐릭터 중심에서 얼마나 떨어진 곳에 생성할지
    public float currentTime = 0.0f;

    // 내부 변수
    Transform playerTransform;

    void Start()
    {
        // Player를 찾아서 저장
        playerTransform = transform.parent;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

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
        // 플레이어가 보는 방향 확인 (1: 오른쪽, -1: 왼쪽)
        float facingDirection = Mathf.Sign(playerTransform.localScale.x);

        // 생성 위치 계산 (보는 방향 쪽으로 약간 앞에서 생성)
        Vector3 spawnPos = playerTransform.position + new Vector3(facingDirection * spawnOffset, 0, 0);

        // 이펙트 생성
        GameObject effect = Instantiate(swingEffectPrefab, spawnPos, Quaternion.identity);

        // 이펙트 좌우 반전 (Scale X 뒤집기)
        Vector3 newScale = effect.transform.localScale * scaleMultiplier;
        newScale.x = Mathf.Abs(newScale.x) * facingDirection;
        effect.transform.localScale = newScale;

        // 회전 움직임 초기화 (방향 전달)
        // SwingMovement에게 "나 지금 오른쪽(1)이야" 혹은 "왼쪽(-1)이야"라고 알려줌.
        SwingMovement swingMove = effect.GetComponent<SwingMovement>();
        if (swingMove != null)
        {
            swingMove.Init(facingDirection);
        }

        // 데미지 정보 전달
        Bullet b = effect.GetComponent<Bullet>();
        if (b != null)
        {
            Vector2 dir = new Vector2(facingDirection, 0);
            // Init(방향, 데미지, 관통O, 근접O)
            b.Init(dir, damage, true, true);
        }

        // 0.3초 뒤 삭제 (애니메이션 끝나면 사라짐)
        Destroy(effect, 0.3f);
    }

    // 레벨업 함수 (Bat)
    public void LevelUp(float damageRate, float coolTimeRate)
    {
        this.damage += (int)damageRate; // 데미지 1 증가
        this.coolTime -= coolTimeRate;  // 쿨타임 0.5 감소

        // 근접 무기 속도 제한 (너무 빠르면 모션이 꼬일 수 있음)
        if (this.coolTime < 0.3f) this.coolTime = 0.3f;

        Debug.Log("방망이 강화! 데미지: " + this.damage + ", 쿨타임: " + this.coolTime);
    }
}