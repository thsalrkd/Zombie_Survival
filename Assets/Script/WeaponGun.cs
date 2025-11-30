using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGun : MonoBehaviour
{
    [Header("±ÇÃÑ ½ºÆå")]
    public int damage = 4;
    public float coolTime = 0.5f;
    public float bulletSpeed = 15f;

    [Header("ÇÁ¸®ÆÕ ¿¬°á")]
    public GameObject bulletPrefab;

    float currentTime = 0.0f;
    Transform playerTransform;

    void Start()
    {
        playerTransform = transform.parent;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;

        currentTime += Time.deltaTime;

        if (currentTime > coolTime)
        {
            currentTime = 0.0f;
            Fire();
        }
    }

    void Fire()
    {
        GameObject target = FindNearestEnemy();

        if (target != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Vector2 dir = (target.transform.position - transform.position).normalized;
                
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.Init(dir, damage, false, false);
                bulletScript.speed = bulletSpeed;
            }
        }
    }

    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearest = null;
        float minDist = Mathf.Infinity;

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
        this.damage += (int)damageRate;
        this.coolTime -= coolTimeRate;
        if (this.coolTime < 0.1f) this.coolTime = 0.1f;
    }
}