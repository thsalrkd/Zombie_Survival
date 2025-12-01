using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    //2D Collider가 트리거 영역에서 나갈 때 호출되는 메서드
    private void OnTriggerExit2D(Collider2D collision)
    {
        //충돌한 오브젝트의 태그가 Area가 아닐 때
        if (!collision.CompareTag("Area"))
            return;
        
        //플레이어 위치와 현재 오브젝트 위치 계산
        Vector3 PlayerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        //플레이어와 현재 오브젝트의 x, y 좌표 차이 절대값 계산
        float diffX = Mathf.Abs(PlayerPos.x - myPos.x);
        float diffY = Mathf.Abs(PlayerPos.y - myPos.y);

        //플레이어의 이동 방향
        Vector3 PlayerDir = GameManager.instance.Player.movement;

        //이동 방향 x, y를 음수면 -1, 양수면 1로 변환
        float dirX = PlayerDir.x < 0 ? -1 : 1;
        float dirY = PlayerDir.y <0 ? -1 : 1;

        //오브젝트 태그가 Ground일 경우
        if (transform.CompareTag("Ground"))
        {
            //x축 차이가 y축 차이보다 크면 x축 방향으로 40만큼 이동
            //40은 맵 크기
            if (diffX > diffY)
                transform.Translate(Vector3.right * dirX * 40);
            //x축 차이가 y축 차이보다 작으면 y축 방향으로 40만큼 이동
            else if (diffX < diffY)
                transform.Translate(Vector3.up * dirY * 40);
        }

    }
}
