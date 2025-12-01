using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //HUD가 표시할 데이터 종류
    //열거형(enum) 사용
    public enum InfoType { HP, Exp, Stage, Time, Score, Level }

    public InfoType type;

    Text myText; //텍스트 UI 컴포넌트 (Stage, Time, Score, Level)
    Slider mySlider; //슬라이더 UI 컴포넌트 (HP, Exp)

    void Awake()
    {
      //컴포넌트 가져오기
      myText = GetComponent<Text>();
      mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.HP: //체력
                float curHP = GameManager.instance.Player.currentHP;
                float maxHP = GameManager.instance.Player.maxHP;
                mySlider.value = curHP / maxHP;
                break;

            case InfoType.Exp: //경험치
                float curExp = GameManager.instance.Player.currentExp;
                int index = GameManager.instance.Player.level - 1;

                //인덱스 범위
                //GameManager.instanve.Player.expTable.Length의 범위를 벗어나지 않도록 보정
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

            case InfoType.Stage: //스테이지
                int curStage=GameManager.instance.currentStage;
                myText.text = string.Format("Stage {0:F0}", curStage);
                break;

            case InfoType.Time: //시간
                float time = GameManager.instance.stageTimer;
                int min = Mathf.FloorToInt(time/60);
                int sec = Mathf.FloorToInt(time % 60);
                myText.text = string.Format("{0:00}:{1:00}", min, sec);
                break;

            case InfoType.Score: //점수 = 적 처치 수
                int score = GameManager.instance.Player.killCount;
                myText.text = string.Format("Score: {0:F0}", score);
                break;

            case InfoType.Level: //레벨
                myText.text = string.Format("Lv.{0:F0}", GameManager.instance.Player.level);
                break;

        }
    }
}
