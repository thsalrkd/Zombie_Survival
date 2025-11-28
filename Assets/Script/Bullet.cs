using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("투사체 속성")]
    public float speed = 10f;    // 날아가는 속도
    public int damage = 3;       // 데미지
    public bool isPenetrate = false; // 관통 여부 (true면 적을 뚫고 지나감)
    public bool isMelee = false; // 근접 무기 여부 (true면 앞으로 날아가지 않음)

    // 초기화 함수: 생성되자마자 이 함수를 호출해 정보를 셋팅함
    public void Init(Vector2 dir, int dmg, bool penetrate, bool melee = false)
    {
        damage = dmg;
        isPenetrate = penetrate;
        isMelee = melee;

        // 날아가는 방향(Z축 회전) 설정
        // 아크탄젠트(y, x)로 각도를 구해서 회전시킴
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        // 5초 뒤에 자동 삭제 (메모리 관리)
        Destroy(gameObject, 5.0f);
    }

    void Update()
    {
        // 근접 무기(방망이 등)가 아니면 앞으로 계속 날아감
        if (!isMelee)
        {
            // 자신의 오른쪽(로컬 좌표계 기준 앞)으로 이동
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }
    }

    // 화면 밖으로 나가면 삭제 (최적화)
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}