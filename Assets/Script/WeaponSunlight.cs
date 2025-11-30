using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSunlight : MonoBehaviour
{
    [Header("태양빛 스펙")]
    public int damage = 2;
    public float tickRate = 0.5f;

    [Header("현재 적용된 공격 범위 (자동 계산)")]
    [SerializeField] float currentRange;

    float timer = 0f;

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        currentRange = transform.localScale.x / 4f;

        timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            timer = 0f;
            DealDamageToArea();
        }
    }

    void DealDamageToArea()
    {
        // ★ 계산된 currentRange를 사용해 공격
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, currentRange);

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

    public void LevelUp(float damageRate, float rangeRate)
    {
        this.damage += (int)damageRate;

        // ★ 레벨업 시: "범위(반지름)"가 늘어나야 하므로
        // 실제 크기(Scale, 지름)는 [증가량 x 2] 만큼 커져야 합니다.
        transform.localScale += Vector3.one * (rangeRate * 2f);

        Debug.Log("햇빛 강화! 데미지: " + this.damage + ", 현재 범위(반지름): " + currentRange);
    }

    // Scene 창에서 빨간 원 확인용
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos도 현재 스케일 기준으로 그립니다.
        Gizmos.DrawWireSphere(transform.position, transform.localScale.x / 4f);
    }
}