using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour
{
    public float expAmount = 10f;

    Transform playerTarget;
    public float magnetSpeed = 8f;
    public float magnetRange = 3f;

    void OnEnable()
    {
        if (GameManager.Instance.player != null)
        {
            playerTarget = GameManager.Instance.player;
        }
    }

    void Update()
    {
        if (playerTarget == null) return;

        // Vector2로 거리 계산
        float distance = Vector2.Distance(transform.position, playerTarget.position);

        if (distance < magnetRange)
        {
            // Vector2.MoveTowards 사용
            transform.position = Vector2.MoveTowards(transform.position, playerTarget.position, magnetSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Playermove player = collision.GetComponent<Playermove>();
            if (player != null)
            {
                player.GetExp(expAmount);
            }
            gameObject.SetActive(false);
        }
    }
}