using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { HP, Exp, Stage, Time, Score}

    public InfoType type;

    Text myText;
    Slider mySlider;

    void Awake()
    {
      myText = GetComponent<Text>();
      mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.HP:

                break;
            case InfoType.Exp:

                break;
            case InfoType.Stage:
                
                break;
            case InfoType.Time:

                break;
            case InfoType.Score:

                break;

        }
    }
}
