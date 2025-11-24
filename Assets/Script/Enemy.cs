using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2.5f;
    public float hp = 20f;
    public float maxHp = 20f;

    // 타겟(플레이어)
    Rigidbody2D target;
    bool isLive = true;

    Rigidbody2D rb;
    SpriteRenderer spriter;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        // 활성화될 때마다 타겟과 체력 초기화
        if (GameManager.Instance.player != null)
        {
            target = GameManager.Instance.player.GetComponent<Rigidbody2D>();
        }
        hp = maxHp;
        isLive = true;
    }

    void FixedUpdate()
    {
        if (!isLive || target == null) return;

        // 2D 벡터 연산
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        if (!isLive || target == null) return;

        // 플레이어 위치에 따라 좌우 반전
        spriter.flipX = target.position.x < rb.position.x;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isLive) return;

        if (collision.CompareTag("Bullet"))
        {
            float damage = collision.GetComponent<Bullet>().damage;
            hp -= damage;

            // 총알 처리 (Bullet 내 관통 로직에 맡기거나 여기서 비활성화)
            // collision.gameObject.SetActive(false); 

            if (hp <= 0)
            {
                Dead();
            }
        }
    }

    void Dead()
    {
        isLive = false;

        // 아이템 드랍 (Index 2: 경험치 보석)
        GameObject expGem = GameManager.Instance.pool.Get(2);
        expGem.transform.position = transform.position;

        // 적 비활성화
        gameObject.SetActive(false);

        // 킬 카운트
        if (GameManager.Instance != null)
            GameManager.Instance.killCount++;
    }
}