using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum EnemyType { Normal, Special, Boss }
    public EnemyType type;

    [Header("몬스터 스탯")]
    public int maxHp = 10;
    public int damage = 5;
    public float moveSpeed = 1.5f;

    int currentHp;
    Transform playerTarget;
    Rigidbody2D rb;
    Animator anim;
    bool isDead = false;

    public GameObject[] dropItems;

    void Start()
    {
        currentHp = maxHp;

        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) playerTarget = p.transform;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (isDead) return;
        if (playerTarget == null) return;

        // 플레이어 추적
        Vector2 dir = (playerTarget.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);

        // 방향 전환
        if (dir.x != 0) transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 1);
    }

    //무기에 맞았을때(Trigger)
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return; // 죽은 상태면 무시

        if (collision.CompareTag("Bullet"))
        {
            Bullet bulletScript = collision.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                TakeDamage(bulletScript.damage);

                //관통이 아니면 총알 삭제
                if (!bulletScript.isPenetrate) Destroy(collision.gameObject);
            }
        }
    }

    //플레이어와 박치기
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Playermove>().ChangeHP(-damage);
        }
    }

    //데미지 받는 함수
    public void TakeDamage(int dmg)
    {
        if (isDead) return;

        currentHp -= dmg;
        if (currentHp > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            Die();
        }
    }

    //사망 처리 함수
    void Die()
    {
        isDead = true;

        anim.SetTrigger("Dead");

        Collider2D[] colliders = GetComponents<Collider2D>();
        foreach (Collider2D col in colliders) col.enabled = false;

        //경험치 부여
        if (playerTarget != null)
            playerTarget.GetComponent<Playermove>().GetExp(1);

        //특수 몬스터 아이템 드랍
        if (type == EnemyType.Special) DropItem();

        //보스 사망 알림
        if (type == EnemyType.Boss)
        {
            if (GameManager.instance != null)
                GameManager.instance.OnBossDead();
        }

        //0.5초 뒤에 삭제 (애니메이션 재생 시간 확보)
        Destroy(gameObject, 0.5f);
    }

    void DropItem()
    {
        // 안전장치
        if (dropItems.Length < 3) return;

        int rand = UnityEngine.Random.Range(0, 100);

        if (rand < 10) Instantiate(dropItems[0], transform.position, Quaternion.identity);
        else if (rand < 30) Instantiate(dropItems[1], transform.position, Quaternion.identity);
        else Instantiate(dropItems[2], transform.position, Quaternion.identity);
    }
}