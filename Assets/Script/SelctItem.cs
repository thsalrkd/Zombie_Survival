using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public ItemList data;
    public int level;
    public WeaponBat weapon;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    void Awake()
    {
        icon = GetComponentsInChildren<Image>()[1];
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

        switch(data.itemType){
            case ItemList.ItemType.Basic:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.cooltime[level] * 100, data.counts[level]); ;
                break;
            case ItemList.ItemType.Melee:
            case ItemList.ItemType.Rifle:
            case ItemList.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.cooltime[level] * 100);
                break;
            case ItemList.ItemType.Glove:
            case ItemList.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100); ;
                break;
        }
    }

    public void OnClick()
    {
        switch (data.itemType)
        {
            case ItemList.ItemType.Basic:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<WeaponBat>();
                    //weapon 초기화 (weapon data 연결 필요)
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    float nextCooltime = data.baseCooltime;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCooltime -= data.baseCooltime * data.cooltime[level];
                    nextCount += data.counts[level];

                    //weapon.LevelUp(nextDamage, nextCount);
                }
                break;

            case ItemList.ItemType.Melee:
            case ItemList.ItemType.Rifle:
            case ItemList.ItemType.Range:
                if (level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<WeaponBat>();
                    //weapon 초기화 (weapon data 연결 필요)
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;

                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];

                    //weapon.LevelUp(nextDamage, nextCount);
                }
                    break;
            
            case ItemList.ItemType.Glove:
                break;
            case ItemList.ItemType.Shoe:
                break;
        }

        level++;

        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
