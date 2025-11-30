using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playermove : MonoBehaviour
{
    [Header("플레이어 스탯")]
    public float moveSpeed = 3f;
    public int maxHP = 10;
    public int currentHP;

    // 점수 표시를 위한 처치 수
    public int killCount = 0;

    [Header("무적 설정")]
    public float invincibleTime = 1.0f;
    bool isInvincible = false;

    [Header("레벨 시스템")]
    public int level = 1;
    public int currentExp = 0;
    public int[] expTable = { 10, 20, 40, 70, 110, 160, 220, 290, 370 };

    Rigidbody2D rb;
    public Vector2 movement;
    Animator animator;
    SpriteRenderer spriteRenderer;
    public bool isDead = false;
    public LevelUp uiLevelUp;

    void Start()
    {
        currentHP = maxHP;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        if (isDead) return;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        // 스테이지 4 이상 Y축 이동 제한 로직은 추후 추가
        movement = new Vector2(x, y).normalized;

        if (x != 0)
            spriteRenderer.flipX = x < 0;

        if (animator.GetBool("isRun") != (movement != Vector2.zero))
        {
            animator.SetBool("isRun", movement != Vector2.zero);
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.instance.isLive || isDead)
            return;

        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    public void GetExp(int amount)
    {
        currentExp += amount;
        killCount += amount;

        if (level < 10 && currentExp >= expTable[level - 1])
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentExp -= expTable[level - 1];
        level++;
        uiLevelUp.Show();
    }

    public void ChangeHP(int amount)
    {
        if (amount < 0 && isInvincible) return;

        currentHP += amount;

        if (amount < 0) StartCoroutine(InvincibilityRoutine());

        if (currentHP > maxHP) currentHP = maxHP;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isDead = true;
            GameManager.instance.GameOver();
        }
    }

    IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    // ★ 수정됨: 무기 슬롯 카운트 (총 3개 제한용)
    public int GetActiveWeaponCount()
    {
        int count = 0;

        // 1. 기본 무기 (Gun) - 이것도 슬롯 1개 차지함
        if (GetComponentInChildren<WeaponGun>(true).gameObject.activeSelf) count++;

        // 2. 추가 무기들 (Bat, Sniper, Sunlight)
        if (GetComponentInChildren<WeaponBat>(true).gameObject.activeSelf) count++;
        if (GetComponentInChildren<WeaponSniper>(true).gameObject.activeSelf) count++;
        if (GetComponentInChildren<WeaponSunlight>(true).gameObject.activeSelf) count++;

        // Shoe, Glove는 무기가 아니므로 카운트 안 함

        return count;
    }
    // 모든 무기 쿨타임 감소 함수 (장갑 아이템용)
    public void ReduceCooldownAllWeapons(float amount)
    {
        // 1. 권총
        WeaponGun gun = GetComponentInChildren<WeaponGun>(true);
        if (gun != null) gun.coolTime = Mathf.Max(0.1f, gun.coolTime - amount);

        // 2. 방망이
        WeaponBat bat = GetComponentInChildren<WeaponBat>(true);
        if (bat != null) bat.coolTime = Mathf.Max(0.5f, bat.coolTime - amount);

        // 3. 저격총
        WeaponSniper sniper = GetComponentInChildren<WeaponSniper>(true);
        if (sniper != null) sniper.coolTime = Mathf.Max(1.0f, sniper.coolTime - amount);

    }
}