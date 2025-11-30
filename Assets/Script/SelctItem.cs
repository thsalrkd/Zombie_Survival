using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;
using UnityEngine.UI;

public class SelectItem : MonoBehaviour
{
    public ItemList data;
    public int level;
    public WeaponBat weaponbat;
    public WeaponSniper weaponspiner;
    public WeaponSunlight weaponsunlight;

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

        switch (data.itemType)
        {
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
                break;
            case ItemList.ItemType.Melee:
                break;
            case ItemList.ItemType.Rifle:
                break;
            case ItemList.ItemType.Range:
                break;
            case ItemList.ItemType.Glove:
                break;
            case ItemList.ItemType.Shoe:
                break;
        }

        level++;

        if(level == data.damages.Length + 1)
        {
            GetComponent<Button>().interactable = false; //Å¬¸¯ X
        }
    }
}
