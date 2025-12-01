using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("투사체 속성")]
    public float speed = 10f;    // 날아가는 속도
    public int damage = 3;       // 데미지
    public bool isPenetrate = false; // 관통 여부 (true면 적을 뚫고 지나감)
    public bool isMelee = false; // 근접 무기 여부

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
        if (!GameManager.instance.isLive)
            return;

        // 근접 무기(방망이)가 아닐 때만 앞으로 날아감
        // 방망이는 휘두르는 모션 스크립트(SwingMovement)가 따로 움직여줌
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