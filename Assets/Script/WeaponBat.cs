using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBat : MonoBehaviour
{
    [Header("야구방망이 스펙")]
    public int damage = 8;
    public float coolTime = 1.5f;

    [Header("프리팹 및 설정")]
    public GameObject swingEffectPrefab; // 휘두르는 이펙트(부채꼴 이미지) 프리팹
    public float attackRange = 2.0f;     // 공격 거리

    float currentTime = 0.0f;
    Transform playerTransform;

    void Start()
    {
        playerTransform = transform.parent;
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        if (currentTime > coolTime)
        {
            currentTime = 0.0f;
            Swing();
        }
    }

    void Swing()
    {
        // 플레이어가 바라보는 방향 (Scale X로 판단)
        // scale.x가 1이면 오른쪽, -1이면 왼쪽
        float directionX = playerTransform.localScale.x;
        Vector2 dir = new Vector2(directionX, 0);

        // 1. 시각적 이펙트 생성 (플레이어 조금 앞)
        Vector3 spawnPos = playerTransform.position + (Vector3)(dir * 0.5f);
        GameObject effect = Instantiate(swingEffectPrefab, spawnPos, Quaternion.identity);

        // 이펙트 좌우 반전 및 초기화
        effect.transform.localScale = new Vector3(directionX, 1, 1);

        // 이펙트 오브젝트에도 Bullet 스크립트를 붙여서 충돌 처리를 하게 만듦
        // Init(방향, 데미지, 관통O, 근접O) -> 근접이므로 날아가지 않음
        // 관통을 true로 해야 범위 내 모든 적이 맞음
        Bullet b = effect.GetComponent<Bullet>();
        if (b != null) b.Init(dir, damage, true, true);

        // 0.3초 뒤 이펙트 삭제 (짧게 휘두르는 느낌)
        Destroy(effect, 0.3f);
    }
}