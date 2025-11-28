using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Playermove player;

    private void Awake()
    {
        instance = this;

    }
}
