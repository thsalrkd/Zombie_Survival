using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSunlight : MonoBehaviour
{
    [Header("태양빛 스펙")]
    public int damage = 2;         // 기본 공격력 2
    public float tickRate = 0.5f;  // 데미지 주기 (0.5초마다 타격) - 고정값
    public float range = 3.0f;     // 기본 범위 (반지름 3)

    float timer = 0f;

    void Start()
    {
        // 게임 시작 시 현재 설정된 범위(3.0)에 맞춰 크기 조절
        UpdateSize();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        // 쿨타임(재장전) 개념이 아니라, '지속 피해 주기' 타이머가 돕니다.
        timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            timer = 0f;
            DealDamageToArea();
        }
    }

    void DealDamageToArea()
    {
        // 내 위치 주변 range 반경 내의 적 감지
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Enemy"))
            {
                Enemy enemyScript = hit.GetComponent<Enemy>();
                if (enemyScript != null)
                {
                    enemyScript.TakeDamage(damage);
                }
            }
        }
    }

    // ★ 레벨업 함수 (데미지 +1, 범위 +0.5)
    // 쿨타임 데이터는 받아오지만 쓰지 않습니다 (X)
    public void LevelUp(float damageRate, float rangeRate)
    {
        this.damage += (int)damageRate; // 데미지 1 증가
        this.range += rangeRate;        // 범위 0.5 증가

        // 범위가 늘어났으니 눈에 보이는 크기도 키워줌.
        UpdateSize();

        Debug.Log("태양빛 강화! 데미지: " + this.damage + ", 범위: " + this.range);
    }

    // 범위(Range)에 맞춰서 오브젝트 크기를 조절하는 함수
    void UpdateSize()
    {
        // Range는 '반지름'이므로, Scale(지름)은 2배가 되어야 함.
        // 원본 스프라이트가 1x1 크기라고 가정할 때 공식입니다.
        transform.localScale = Vector3.one * range * 2;
    }

    // 에디터에서 범위 확인용 (노란 원)
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}