using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwingMovement : MonoBehaviour
{
    [Header("휘두르기 속도")]
    public float swingSpeed = 800f;

    [Header("시작 각도 설정 (각각 조절 가능!)")]
    public float rightStartAngle = 20f;  // 오른쪽 볼 때 시작 각도
    public float leftStartAngle = 20f;   // 왼쪽 볼 때 시작 각도

    // Init에서 '지금 어느 쪽인지(dir)'를 받아서 각도를 다르게 적용
    public void Init(float dir)
    {
        float targetAngle = 0f;

        if (dir > 0) // 오른쪽 볼 때
        {
            targetAngle = rightStartAngle;
        }
        else // 왼쪽 볼 때
        {
            targetAngle = leftStartAngle;
        }

        // 회전 적용
        transform.localRotation = Quaternion.Euler(0, 0, targetAngle);
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        // 무조건 내려치기 (-Speed)
        transform.Rotate(0, 0, -swingSpeed * Time.deltaTime);
    }
}