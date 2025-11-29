using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSunlight : MonoBehaviour
{
    [Header("태양빛 스펙")]
    public int damage = 2;         // 틱당 데미지
    public float tickRate = 0.5f;  // 데미지 간격
    public float range = 3.0f;     // 공격 범위 (반지름)

    float timer = 0f;

    void Start()
    {
        // 범위 시각화를 위해 Collider 크기를 코드에서 맞춤
        CircleCollider2D col = GetComponent<CircleCollider2D>();
        if (col != null) range = col.radius * transform.lossyScale.x;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;

        if (timer >= tickRate)
        {
            timer = 0f;
            DealDamageToArea();
        }
    }

    void DealDamageToArea()
    {
        // 내 위치(transform.position) 주변 range 반경 내의 모든 콜라이더를 가져옴
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range);

        foreach (Collider2D hit in hits)
        {
            // "Enemy" 태그를 가진 녀석만 골라서 데미지 줌
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

    // 에디터에서 범위를 눈으로 보기 위한 기능
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}