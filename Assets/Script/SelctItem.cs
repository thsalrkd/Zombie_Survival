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

    // SelectItem.cs 안의 OnClick 함수 부분

    public void OnClick()
    {
        // GameManager를 통해 플레이어 가져오기
        Playermove player = GameManager.instance.Player;

        switch (data.itemType)
        {
            case ItemList.ItemType.Melee: // 야구방망이
                // ★ 중요: (true)를 넣어야 비활성화된(꺼진) 오브젝트도 찾을 수 있습니다!
                WeaponBat bat = player.GetComponentInChildren<WeaponBat>(true);

                if (bat != null)
                {
                    // 1. 꺼져있다면? -> 켜준다 (해금)
                    if (!bat.gameObject.activeSelf)
                    {
                        bat.gameObject.SetActive(true);
                        // 처음 켰을 때 기본 데미지 설정 (데이터의 0번 인덱스 사용)
                        bat.LevelUp(data.damages[0], data.cooltime[0]);
                    }
                    else // 2. 켜져있다면? -> 강화한다 (LevelUp)
                    {
                        // 현재 레벨에 맞는 수치만큼 강화
                        bat.LevelUp(data.damages[level], data.cooltime[level]);
                    }
                }
                break;

            case ItemList.ItemType.Rifle: // 저격총
                WeaponSniper sniper = player.GetComponentInChildren<WeaponSniper>(true);
                if (sniper != null)
                {
                    if (!sniper.gameObject.activeSelf)
                    {
                        sniper.gameObject.SetActive(true); // 해금
                        sniper.LevelUp(data.damages[0], data.cooltime[0]);
                    }
                    else
                    {
                        sniper.LevelUp(data.damages[level], data.cooltime[level]); // 강화
                    }
                }
                break;

            case ItemList.ItemType.Range: // 태양빛
                WeaponSunlight sun = player.GetComponentInChildren<WeaponSunlight>(true);
                if (sun != null)
                {
                    if (!sun.gameObject.activeSelf)
                    {
                        sun.gameObject.SetActive(true); // 해금
                        sun.LevelUp(data.damages[0], data.counts[0]);
                    }
                    else
                    {
                        sun.LevelUp(data.damages[level], data.counts[level]); // 강화
                    }
                }
                break;
        }

        level++; // 아이템 카드 레벨 증가

        // 만렙 도달 시 버튼 비활성화 (선택 사항)
        if (level >= data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }

        // 창 닫기
        GetComponentInParent<LevelUp>().Hide();
    }
}
