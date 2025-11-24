using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int bulletPrefabId = 1;
    public float damage = 10f;
    public float attackCooltime = 0.5f;
    public float scanRange = 10f;

    float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer > attackCooltime)
        {
            timer = 0;
            Fire();
        }
    }

    void Fire()
    {
        Transform target = FindNearestEnemy();
        if (target == null) return;

        // 3D 위치(Vector3)를 2D(Vector2)로 변환하여 방향 계산 (Z축 무시)
        Vector2 myPos = (Vector2)transform.position;
        Vector2 targetPos = (Vector2)target.position;
        Vector2 dir = (targetPos - myPos).normalized;

        // 풀링으로 총알 생성
        GameObject bullet = GameManager.Instance.pool.Get(bulletPrefabId);
        bullet.transform.position = transform.position;

        // 회전 (Atan2 사용, 회전축은 Z축이므로 Vector3.forward 사용 필수)
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 총알 초기화
        bullet.GetComponent<Bullet>().Init(damage, dir);
    }

    Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearest = null;
        float minDistance = scanRange;

        foreach (GameObject enemyObj in enemies)
        {
            if (!enemyObj.activeSelf) continue; // 죽은 적은 무시

            // 거리 계산도 Vector2로 변환해서 수행
            float dist = Vector2.Distance((Vector2)transform.position, (Vector2)enemyObj.transform.position);

            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = enemyObj.transform;
            }
        }
        return nearest;
    }
}