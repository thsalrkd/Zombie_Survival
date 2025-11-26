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
    public GameObject[] dropItems;

    void Start()
    {
        currentHp = maxHp;
        GameObject p = GameObject.FindWithTag("Player");
        if (p != null) playerTarget = p.transform;
    }

    void Update()
    {
        if (playerTarget == null) return;
        Vector2 dir = (playerTarget.position - transform.position).normalized;
        transform.Translate(dir * moveSpeed * Time.deltaTime);
        if (dir.x != 0) transform.localScale = new Vector3(dir.x > 0 ? 1 : -1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            Bullet bulletScript = collision.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                TakeDamage(bulletScript.damage);
                if (!bulletScript.isPenetrate) Destroy(collision.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Playermove>().ChangeHP(-damage);
        }
    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;
        if (currentHp <= 0) Die();
    }

    void Die()
    {
        if (playerTarget != null)
            playerTarget.GetComponent<Playermove>().GetExp(1);

        if (type == EnemyType.Special) DropItem();

        // ★★★ 추가된 부분: 보스가 죽으면 게임 매니저에게 알림 ★★★
        if (type == EnemyType.Boss)
        {
            // GameManager가 있다면 보스 처치 함수 호출
            if (GameManager.instance != null)
                GameManager.instance.OnBossDead();
        }

        Destroy(gameObject);
    }

    void DropItem()
    {
        if (dropItems.Length < 3) return;
        int rand = Random.Range(0, 100);
        if (rand < 10) Instantiate(dropItems[0], transform.position, Quaternion.identity);
        else if (rand < 30) Instantiate(dropItems[1], transform.position, Quaternion.identity);
        else Instantiate(dropItems[2], transform.position, Quaternion.identity);
    }
}