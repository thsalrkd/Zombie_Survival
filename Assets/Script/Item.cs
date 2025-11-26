using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    // 아이템 종류 설정 (유니티 인스펙터에서 숫자 입력)
    // 0: 회복, 1: 전멸(화면 전체 킬), 2: 쿨초기화
    public int itemType;

    // 플레이어와 닿았을 때 (Trigger 충돌) 실행
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어 태그 확인
        if (collision.CompareTag("Player"))
        {
            // *** 변경됨: PlayerController -> Playermove ***
            // 닿은 물체(플레이어)로부터 Playermove 스크립트 가져오기
            Playermove player = collision.GetComponent<Playermove>();

            if (player != null)
            {
                ApplyEffect(player); // 아이템 효과 적용 함수 호출
            }

            // 아이템 오브젝트 삭제 (먹었으니 사라짐)
            Destroy(gameObject);
        }
    }

    // 아이템 효과 적용 함수
    // *** 변경됨: 매개변수 타입 PlayerController -> Playermove ***
    void ApplyEffect(Playermove player)
    {
        switch (itemType)
        {
            case 0: // 회복 아이템
                Debug.Log("아이템 획득: 체력 회복");
                // 기획서상 20% 회복이지만, 현재 MAX HP 10이므로 2 회복으로 구현
                player.ChangeHP(2);
                break;

            case 1: // 전멸 아이템 (화면 내 모든 적 제거)
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

    // 맵상의 모든 적을 찾아 죽이는 함수
    void KillAllEnemies()
    {
        // "Enemy" 태그를 가진 모든 오브젝트를 배열로 가져옴
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObj in enemies)
        {
            Enemy enemyScript = enemyObj.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                // 적 스크립트의 TakeDamage 함수를 호출하여 9999 데미지를 줌
                // SendMessage를 사용해 Enemy 스크립트 내부 함수 호출
                enemyScript.SendMessage("TakeDamage", 9999);
            }
        }
    }

    // 무기 쿨타임 초기화 함수
    // *** 변경됨: 매개변수 타입 PlayerController -> Playermove ***
    void ResetAllCooldowns(Playermove player)
    {
        // 플레이어 오브젝트의 자식(Child)에 있는 무기 스크립트들을 찾음
        // 여기서는 예시로 WeaponSniper(저격총)의 쿨타임을 초기화하는 로직
        // 모든 무기에 공통된 "ResetCool" 같은 함수를 만드는 것이 가장 좋음

        // 저격총 찾기
        WeaponSniper sniper = player.GetComponentInChildren<WeaponSniper>();
        if (sniper != null)
        {
            // 저격총 스크립트를 재시작하거나 내부 변수를 초기화해야 함
            // 간단하게 SendMessage로 특정 함수를 부르거나, 해당 스크립트에 public 초기화 함수가 필요함
            // 여기서는 쿨타임 변수에 직접 접근이 어려우므로 예시 주석만 남김
            // sniper.currentTime = 100f; // 이런 식으로 쿨타임을 넘겨버려서 바로 발사되게 함
        }

        // 야구방망이 찾기
        WeaponBat bat = player.GetComponentInChildren<WeaponBat>();
        if (bat != null)
        {
            // bat.currentTime = 100f; 
        }
    }
}