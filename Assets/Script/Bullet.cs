using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float speed = 10f;
    public int per = 0; // 관통력

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, Vector2 dir)
    {
        this.damage = damage;
        this.per = 0; // 관통력 초기화 (0이면 1회 타격 후 삭제)
        rb.velocity = dir * speed; // 2D 물리 속도 적용
    }

    void OnEnable()
    {
        Invoke("DisableBullet", 5f);
    }

    void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 관통 로직
        if (collision.CompareTag("Enemy"))
        {
            per--;
            if (per < 0)
            {
                rb.velocity = Vector2.zero;
                gameObject.SetActive(false);
            }
        }
    }
}