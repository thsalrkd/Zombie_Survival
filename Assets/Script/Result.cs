using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public GameObject[] titles;

    public void Over()
    {
        titles[0].SetActive(true);
        titles[1].SetActive(false);
    }

    public void Clear()
    {
        titles[1].SetActive(true);
        titles[0].SetActive(false);
    }
}
