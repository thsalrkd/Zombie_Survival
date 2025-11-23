using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float Speed;
    public Rigidbody2D target;

    bool isLive = true;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        if (!isLive || GameManager.Instance.player == null)
            return;

        Vector2 directionVector = (Vector2)GameManager.Instance.player.position - rigid.position;
        Vector2 nextVector = directionVector.normalized * Speed * Time.fixedDeltaTime;

        rigid.MovePosition(rigid.position + nextVector);
        rigid.velocity = Vector2.zero;




    }

    void LateUpdate()
    {
        if (!isLive || GameManager.Instance.player == null)
            return;

        spriter.flipX = GameManager.Instance.player.position.x < rigid.position.x;

    }
}
