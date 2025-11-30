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
            case ItemList.ItemType.Melee:
            case ItemList.ItemType.Rifle:
            case ItemList.ItemType.Range:
                if (level < data.damages.Length)
                    textDesc.text = string.Format(data.itemDesc, data.damages[level], data.counts[level]);
                break;
            case ItemList.ItemType.Shoe:
            case ItemList.ItemType.Glove:
                textDesc.text = data.itemDesc;
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
                // 기존 무기 로직
                if (data.itemType == ItemList.ItemType.Melee)
                {
                    WeaponBat w = player.GetComponentInChildren<WeaponBat>(true);
                    if (!w.gameObject.activeSelf) w.gameObject.SetActive(true);
                    else w.damage += (int)data.damages[level];
                }
                else if (data.itemType == ItemList.ItemType.Rifle)
                {
                    WeaponSniper w = player.GetComponentInChildren<WeaponSniper>(true);
                    if (!w.gameObject.activeSelf) w.gameObject.SetActive(true);
                    else w.damage += (int)data.damages[level];
                }
                else if (data.itemType == ItemList.ItemType.Range)
                {
                    WeaponSunlight w = player.GetComponentInChildren<WeaponSunlight>(true);
                    if (!w.gameObject.activeSelf) w.gameObject.SetActive(true);
                    else w.damage += (int)data.damages[level];
                }
                level++;
                break;

            // ★ 추가됨: 스탯 아이템 로직
            case ItemList.ItemType.Shoe:
                // 이동속도 0.5 증가 (무제한 강화)
                player.moveSpeed += 0.5f;
                Debug.Log("이동속도 증가! 현재: " + player.moveSpeed);
                break;

            case ItemList.ItemType.Glove:
                // 모든 무기 쿨타임 0.3초 감소 (무제한 강화)
                player.ReduceCooldownAllWeapons(0.3f);
                Debug.Log("공격속도 증가!");
                break;
        }

        // 무기가 아니면 레벨 개념이 딱히 없거나 무한이므로 level++ 생략 가능하지만,
        // UI 갱신을 위해 올릴 수도 있음. 여기서는 스탯은 무제한이므로 level 변수 활용 안 함.

        GetComponentInParent<LevelUp>().Hide();
    }
}