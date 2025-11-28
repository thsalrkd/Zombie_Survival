using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    Transform playerTransform;

    void Start()
    {
        // "Player" 태그를 가진 오브젝트 찾기
        GameObject playerObj = GameObject.FindWithTag("Player");

        if (playerObj != null)
        {
            playerTransform = playerObj.transform;
            // Z축(-10) 유지를 위해 현재 카메라 위치와 플레이어 위치 차이를 저장
            offset = transform.position - playerTransform.position;
        }

    }

    void LateUpdate()
    {
        // ★ 플레이어를 못 찾았으면 아무것도 하지 마라 (에러 방지)
        if (playerTransform == null) return;

        Vector3 desiredPosition = playerTransform.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라의 Z축은 보통 -10이어야 함. 
        // offset에 이미 Z값 차이가 들어있으므로 그대로 적용하면 됨.
        transform.position = smoothedPosition;
    }
}