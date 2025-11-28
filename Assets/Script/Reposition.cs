using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;
        
        Vector3 PlayerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;
        float diffX = Mathf.Abs(PlayerPos.x - myPos.x);
        float diffY = Mathf.Abs(PlayerPos.y - myPos.y);

        Vector3 PlayerDir = GameManager.instance.Player.movement;
        float dirX = PlayerDir.x < 0 ? -1 : 1;
        float dirY = PlayerDir.y <0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                if (diffX > diffY)
                {
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if (diffX < diffY)
                {
                    transform.Translate(Vector3.up * dirY * 40);
                }
                    break;

            case "Enemy":
                break;
        }

    }
}
