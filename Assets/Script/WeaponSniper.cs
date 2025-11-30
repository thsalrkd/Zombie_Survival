using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSniper : MonoBehaviour
{
    [Header("저격총 스펙")]
    public int damage = 7;        // 높은 데미지
    public float coolTime = 5.0f;  // 긴 쿨타임
    public float bulletSpeed = 20f;// 빠른 탄속

    [Header("프리팹 연결")]
    public GameObject bulletPrefab; // 총알 프리팹 

    // 내부 변수
    float currentTime = 0.0f;     // 쿨타임 계산용
    Transform playerTransform;    // 플레이어 위치

    void Start()
    {
        // 플레이어의 자식 오브젝트로 있다고 가정
        playerTransform = transform.parent;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        // 쿨타임 계산
        currentTime += Time.deltaTime;

        // 쿨타임이 찼다면 발사
        if (currentTime > coolTime)
        {
            currentTime = 0.0f; // 타이머 초기화
            Fire();
        }
    }

    void Fire()
    {
        // 가장 가까운 적 찾기
        GameObject target = FindNearestEnemy();

        if (target != null)
        {
            // 총알 생성 (플레이어 위치에서)
            GameObject bullet = Instantiate(bulletPrefab, playerTransform.position, Quaternion.identity);

            // 타겟 방향 계산
            Vector2 dir = (target.transform.position - playerTransform.position).normalized;

            // 총알 초기화 (방향, 데미지, 관통여부=true)
            // Bullet 스크립트의 Init 함수 호출: Init(방향, 데미지, 관통O, 근접X)
            bullet.GetComponent<Bullet>().Init(dir, damage, true, false);

            // 총알 속도 덮어쓰기
            bullet.GetComponent<Bullet>().speed = bulletSpeed;
        }
    }

    // 가장 가까운 적을 찾는 헬퍼 함수
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity; // 초기값은 무한대

        foreach (GameObject enemy in enemies)
        {
            float dist = Vector3.Distance(transform.position, enemy.transform.position);
            if (dist < minDist)
            {
                minDist = dist;
                nearest = enemy;
            }
        }
        return nearest;
    }

    public void LevelUp(float damageRate, float coolTimeRate)
    {
        this.damage += (int)damageRate; // 데미지 1 증가
        this.coolTime -= coolTimeRate;  // 쿨타임 1.0 감소

        // 저격총은 너무 빠르면 안되므로 0.5초 제한
        if (this.coolTime < 0.5f) this.coolTime = 0.5f;

        Debug.Log("저격총 강화! 데미지: " + this.damage + ", 쿨타임: " + this.coolTime);
    }
}