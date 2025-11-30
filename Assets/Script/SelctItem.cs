using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public ItemList data;
    public int level;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        Image[] images = GetComponentsInChildren<Image>();
        icon = images[1];
        icon.sprite = data.itemIcon;

        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];
        textName.text = data.itemName;
    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (level + 1);

        // 설명 텍스트 포맷팅
        switch (data.itemType)
        {
            case ItemList.ItemType.Basic:
                if(level < data.damages.Length)
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.cooltime[level], data.counts[level]);
                break;
            case ItemList.ItemType.Melee:
            case ItemList.ItemType.Rifle:
                if (level < data.damages.Length)
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.cooltime[level]);
                break;
            case ItemList.ItemType.Range:
                if (level < data.damages.Length)
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.cooltime[level]);
                break;
            case ItemList.ItemType.Shoe:
            case ItemList.ItemType.Glove:
                if (level < data.damages.Length)
                    textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100 );
                break;
        }
    }

    public void OnClick()
    {
        Playermove player = GameManager.instance.Player;

        switch (data.itemType)
        {
            case ItemList.ItemType.Melee:
            case ItemList.ItemType.Rifle:
            case ItemList.ItemType.Range:

                if (data.itemType == ItemList.ItemType.Melee)
                {
                    WeaponBat w = player.GetComponentInChildren<WeaponBat>(true);
                    if (!w.gameObject.activeSelf)
                    {
                        w.gameObject.SetActive(true);
                    }
                    else
                    {
                        // 단순히 damage만 더하는 게 아니라, 쿨타임도 줄여주는 LevelUp 함수 호출
                        w.LevelUp(data.damages[level], data.cooltime[level]);
                    }
                }
                else if (data.itemType == ItemList.ItemType.Rifle)
                {
                    WeaponSniper w = player.GetComponentInChildren<WeaponSniper>(true);
                    if (!w.gameObject.activeSelf)
                    {
                        w.gameObject.SetActive(true);
                        w.LevelUp(data.damages[0], data.cooltime[0]);
                    }
                    else
                    {
                        // 데미지 + 쿨타임 감소 함수 호출
                        w.LevelUp(data.damages[level], data.cooltime[level]);
                    }
                }
                else if (data.itemType == ItemList.ItemType.Range)
                {
                    WeaponSunlight w = player.GetComponentInChildren<WeaponSunlight>(true);
                    if (!w.gameObject.activeSelf)
                    {
                        w.gameObject.SetActive(true);
                        w.LevelUp(data.damages[0], data.cooltime[0]);
                    }
                    else
                    {
                        // 데미지 + 범위(counts) 증가 함수 호출
                        w.LevelUp(data.damages[level], data.cooltime[level]);
                    }
                }
                level++;
                break;

            case ItemList.ItemType.Shoe:
                // 이동속도 0.5 증가
                player.moveSpeed += 0.5f;
                Debug.Log("이동속도 증가! 현재: " + player.moveSpeed);
                level++;
                break;

            case ItemList.ItemType.Glove:
                // 모든 무기 쿨타임 0.3초 감소
                player.ReduceCooldownAllWeapons(0.3f);
                Debug.Log("공격속도 증가!");
                level++;
                break;
        }

        // 만렙 체크 (데이터 길이보다 레벨이 커지면 버튼 비활성화용)
        if (data.itemType != ItemList.ItemType.Shoe && data.itemType != ItemList.ItemType.Glove)
        {
            if (level >= data.damages.Length)
            {
                GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
        }

        GetComponentInParent<LevelUp>().Hide();
    }
}