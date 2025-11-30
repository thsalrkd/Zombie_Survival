using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    [Header("플레이어 스탯")]
    public float moveSpeed = 3f;
    public int maxHP = 10;
    public int currentHP;

    [Header("무적 설정")]
    public float invincibleTime = 1.0f; // 피격 후 1초간 무적
    bool isInvincible = false;          // 현재 무적 상태인지

    [Header("레벨 시스템")]
    public int level = 1;
    public int currentExp = 0;
    public int[] expTable = { 10, 20, 40, 70, 110, 160, 220, 290, 370 };

    Rigidbody2D rb;
    public Vector2 movement;
    Animator animator;
    SpriteRenderer spriteRenderer; // 스프라이트 직접 뒤집기용
    public bool isDead = false;
    public LevelUp uiLevelUp;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // 컴포넌트 가져오기
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (isDead) return;

        // --- 이동 입력 ---
        float moveX = 0;
        float moveY = 0;

        if (Input.GetKey(KeyCode.W)) moveY = 1;
        if (Input.GetKey(KeyCode.S)) moveY = -1;
        if (Input.GetKey(KeyCode.A)) moveX = -1;
        if (Input.GetKey(KeyCode.D)) moveX = 1;

        movement = new Vector2(moveX, moveY).normalized;
        transform.Translate(movement * Time.deltaTime * moveSpeed);

        // --- 애니메이션 및 방향 전환 ---
        if (movement.magnitude > 0.1f)
        {
            animator.SetBool("isRun", true);

            // 왼쪽(-1) 이동 시
            if (moveX < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1); // 몸체 전체(무기 포함) 뒤집기
            }
            // 오른쪽(1) 이동 시
            else if (moveX > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else
        {
            animator.SetBool("isRun", false);
        }
    }

    public void GetExp(int amount)
    {
        currentExp += amount;
        if (level < 10 && currentExp >= expTable[level - 1])
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp -= expTable[level - 1];
        level++;
        Debug.Log("레벨 업!");
        uiLevelUp.Show();
    }

    // 체력 변경 함수 (무적 로직 추가됨)
    public void ChangeHP(int amount)
    {
        // 1. 데미지를 입는 상황(amount가 음수)이고, 이미 무적 상태라면 -> 데미지 무시
        if (amount < 0 && isInvincible) return;

        // 2. 데미지 적용
        currentHP += amount;

        // 3. 데미지를 입었다면 무적 시간 발동
        if (amount < 0)
        {
            StartCoroutine(InvincibilityRoutine());
        }

        // 체력 제한 및 사망 처리
        if (currentHP > maxHP) currentHP = maxHP;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            Debug.Log("사망!");
            // 여기에 게임오버 UI 호출 추가
            GameManager.instance.GameOver();
        }
    }

    // 무적 코루틴 (일정 시간 대기 후 무적 해제)
    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // 시각적 효과: 반투명하게 깜빡임
        if (spriteRenderer != null) spriteRenderer.color = new Color(1, 1, 1, 0.5f);

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
        if (spriteRenderer != null) spriteRenderer.color = new Color(1, 1, 1, 1f);
    }
}