using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    //게임 결과 화면에서 활성화할 타이틀 오브젝트 배열
    //title[0]=게임 오버 UI
    //title[1]=게임 클리어 UI
    public GameObject[] titles;

    //게임 오버 시 호출
    //게임 오버 UI 활성화, 게임 클리어 UI 비활성화
    public void Over()
    {
        titles[0].SetActive(true);
        titles[1].SetActive(false);
    }

    //게임 클리어 시 호출
    //게임 오버 UI 비활성화, 게임 클리어 UI 활성화
    public void Clear()
    {
        titles[1].SetActive(true);
        titles[0].SetActive(false);
    }
}
