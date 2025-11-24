using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 4f;
    Vector2 inputVec;

    Rigidbody2D rb;
    SpriteRenderer spriter;
    Animator animator;

    [Header("플레이어 스탯")]
    public float maxHP = 100;
    public float currentHP;

    [Header("레벨 시스템")]
    public int level = 1;
    public float currentExp = 0;
    public float maxExp = 10;

    public bool isDead = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHP = maxHP;
        level = 1;
        maxExp = 10;

        // GameManager에 내 자신 연결
        if (GameManager.Instance != null)
        {
            GameManager.Instance.player = this.transform;
        }
    }

    void Update()
    {
        if (isDead) return;

        inputVec.x = Input.GetAxisRaw("Horizontal");
        inputVec.y = Input.GetAxisRaw("Vertical");

        // 방향 전환
        if (inputVec.x != 0)
        {
            spriter.flipX = inputVec.x < 0;
        }

        // 애니메이션
        animator.SetFloat("Speed", inputVec.magnitude);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        Vector2 nextVec = inputVec.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
    }

    // 경험치 획득
    public void GetExp(float expAmount)
    {
        currentExp += expAmount;
        if (currentExp >= maxExp)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        level++;
        currentExp = 0;

        // 레벨업 테이블
        switch (level)
        {
            case 2: maxExp = 20; break;
            case 3: maxExp = 40; break;
            case 4: maxExp = 70; break;
            case 5: maxExp = 110; break;
            case 6: maxExp = 160; break;
            case 7: maxExp = 220; break;
            case 8: maxExp = 290; break;
            case 9: maxExp = 370; break;
            case 10: maxExp = 460; break;
        }
    }

    // 충돌 처리
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            currentHP -= 10;
            if (currentHP <= 0)
            {
                isDead = true;
                rb.velocity = Vector2.zero;
                // 게임오버 로직 추가
            }
        }
    }
}