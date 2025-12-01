using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 0: 회복, 1: 전멸(화면 전체 킬), 2: 쿨초기화
    public int itemType;

    // 플레이어와 닿았을 때 (Trigger 충돌) 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그 확인
        if (collision.CompareTag("Player"))
        {
            // 닿은 물체(플레이어)로부터 Playermove 스크립트 가져오기
            Playermove player = collision.GetComponent<Playermove>();

            if (player != null)
            {
                ApplyEffect(player); // 아이템 효과 적용 함수 호출
            }

            // 아이템 오브젝트 삭제
            Destroy(gameObject);
        }
    }

    // 아이템 효과 적용 함수
    void ApplyEffect(Playermove player)
    {
        switch (itemType)
        {
            case 0: // 회복 아이템
                Debug.Log("아이템 획득: 체력 회복");
                player.ChangeHP(20);
                break;

            case 1: // 전멸 아이템 (화면 내 모든 적 제거, 보스 제외)
                Debug.Log("아이템 획득: 적 전멸");
                KillAllEnemies();
                break;

            case 2: // 쿨타임 초기화 아이템
                Debug.Log("아이템 획득: 쿨타임 초기화");
                // 플레이어와 관련된 모든 무기 쿨타임 초기화
                ResetAllCooldowns(player);
                break;
        }
    }

    // 맵상의 모든 적을 찾아 죽이는 함수, 보스 제외
    void KillAllEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemies)
        {
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                //보스면 죽이지 않고 넘어감
                if (enemyScript.type == Enemy.EnemyType.Boss)
                    continue;

                enemyScript.TakeDamage(9999);
            }
        }
    }

    // 무기 쿨타임 초기화 함수
    void ResetAllCooldowns(Playermove player)
    {
        // 권총 (WeaponGun) 찾기 및 쿨타임 초기화
        // 권총은 기본 무기라 보통 항상 있지만, 안전하게 null 체크
        WeaponGun gun = player.GetComponentInChildren<WeaponGun>();
        if (gun != null)
        {
            // 현재 시간을 쿨타임보다 크게 설정하여 즉시 발사되게 함
            gun.currentTime = gun.coolTime + 0.1f;
            Debug.Log("권총 쿨타임 초기화 완료");
        }

        // 저격총 (WeaponSniper) 찾기
        WeaponSniper sniper = player.GetComponentInChildren<WeaponSniper>();
        if (sniper != null)
        {
            //저격총도 동일하게 처리
            sniper.currentTime = sniper.coolTime + 0.1f;
            Debug.Log("저격총 쿨타임 초기화 완료");
        }

        // 삽 (WeaponBat) 찾기
        WeaponBat bat = player.GetComponentInChildren<WeaponBat>();
        if (bat != null)
        {
            //삽도 동일하게 처리
            bat.currentTime = bat.coolTime + 0.1f;
            Debug.Log("방망이 쿨타임 초기화 완료");
        }
    }
}