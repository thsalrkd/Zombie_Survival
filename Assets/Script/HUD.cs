using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public enum InfoType { HP, Exp, Stage, Time, Score, Level}

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
                float curHP = GameManager.instance.Player.currentHP;
                float maxHP = GameManager.instance.Player.maxHP;
                mySlider.value = curHP / maxHP;
                break;
            case InfoType.Exp:
                float curExp = GameManager.instance.Player.currentExp;
                int index = GameManager.instance.Player.level - 1;
                if (index < 0)
                {
                    index = 0;
                }
                else if (index >= GameManager.instance.Player.expTable.Length)
                {
                    index = GameManager.instance.Player.expTable.Length - 1;
                }

                float maxExp = GameManager.instance.Player.expTable[index];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Stage:
                int curStage=GameManager.instance.currentStage;
                myText.text = string.Format("Stage {0:F0}", curStage);
                break;
            case InfoType.Time:
                float time = GameManager.instance.stageTimer;
                int min = Mathf.FloorToInt(time/60);
                int sec = Mathf.FloorToInt(time % 60);
                myText.text = string.Format("{0:00}:{1:00}", min, sec);
                break;
            case InfoType.Score:
                //currentExp->killCount
                int score = GameManager.instance.Player.killCount;
                myText.text = string.Format("Score: {0:F0}", score);
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.Player.level);
                break;

        }
    }
}
