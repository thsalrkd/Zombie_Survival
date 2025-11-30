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
            // ★ 1. 권총 (Basic) 추가됨
            case ItemList.ItemType.Basic:
                WeaponGun gun = player.GetComponentInChildren<WeaponGun>(true);
                if (gun != null)
                {
                    // 권총은 처음부터 가지고 있으므로 active 체크 없이 바로 강화
                    gun.LevelUp(data.damages[level], data.cooltime[level]);
                }
                level++; // 레벨 증가
                break;

            // ★ 2. 기존 무기들 (방망이, 저격총, 태양빛)
            case ItemList.ItemType.Melee:
            case ItemList.ItemType.Rifle:
            case ItemList.ItemType.Range:

                if (data.itemType == ItemList.ItemType.Melee)
                {
                    WeaponBat w = player.GetComponentInChildren<WeaponBat>(true);
                    if (!w.gameObject.activeSelf)
                    {
                        w.gameObject.SetActive(true);
                        w.LevelUp(data.damages[0], data.cooltime[0]);
                    }
                    else
                    {
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
                        w.LevelUp(data.damages[level], data.cooltime[level]);
                    }
                }
                else if (data.itemType == ItemList.ItemType.Range)
                {
                    WeaponSunlight w = player.GetComponentInChildren<WeaponSunlight>(true);
                    if (!w.gameObject.activeSelf)
                    {
                        w.gameObject.SetActive(true);
                        w.LevelUp(data.damages[0], data.counts[0]);
                    }
                    else
                    {
                        w.LevelUp(data.damages[level], data.counts[level]);
                    }
                }
                level++; // 무기 공통 레벨 증가
                break;

            // ★ 3. 스탯 아이템 (신발, 장갑)
            case ItemList.ItemType.Shoe:
                player.moveSpeed += 0.5f;
                Debug.Log("이동속도 증가! 현재: " + player.moveSpeed);
                // 스탯은 무제한이라 level 변수를 안 쓸 수도 있지만, 데이터 관리를 위해 올림
                level++;
                break;

            case ItemList.ItemType.Glove:
                player.ReduceCooldownAllWeapons(0.3f);
                Debug.Log("공격속도 증가!");
                level++;
                break;
        }

        // ★ 4. 만렙 체크 (버튼 비활성화)
        // 스탯 아이템(Shoe, Glove)은 무제한 강화이므로 체크 제외
        if (data.itemType != ItemList.ItemType.Shoe && data.itemType != ItemList.ItemType.Glove)
        {
            if (level >= data.damages.Length)
            {
                GetComponent<UnityEngine.UI.Button>().interactable = false;
            }
        }

        // 창 닫기
        GetComponentInParent<LevelUp>().Hide();
    }
}