using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;

public class Playermove : MonoBehaviour
{
    public Vector2 inputVector;
    [SerializeField]
    float PlayerSpeed;

    Rigidbody2D rigid;
    SpriteRenderer sprite;
    Animator animator;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    { 

    }

    // Update is called once per frame
    void Update()
    {
        inputVector.x = Input.GetAxisRaw("Horizontal");
        inputVector.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        /*
        //힘을 줄 때
        rigid.AddForce(inputVector);

        //속도 제어
        rigid.velocity = inputVector;
        */
        Vector2 movement = inputVector.normalized * PlayerSpeed * Time.fixedDeltaTime;

        //위치 이동
        rigid.MovePosition(rigid.position + movement);


    }

    void LateUpdate()
    {
        if (inputVector.x > 0)
        {
            sprite.flipX = false;
        }
        else if (inputVector.x < 0)
        {
            sprite.flipX = true;
        }

        animator.SetFloat("Speed", inputVector.magnitude);
    }
}
