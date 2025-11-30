using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    RectTransform rect;
    SelectItem[] items;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<SelectItem>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        GameManager.instance.Stop();
    }
    public void Hide()
    {
        rect.localScale = Vector3.zero;
        GameManager.instance.Resume();
    }

    public void Select(int i)
    {
        items[i].OnClick();
    }

    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (SelectItem item in items)
        {
            item.gameObject.SetActive(false);
        }

        // 2. 현재 활성화된 무기 개수 (Gun 포함)
        int activeWeaponCount = GameManager.instance.Player.GetActiveWeaponCount();

        // 3. 랜덤 후보 선정
        List<int> validIndices = new List<int>();

        for (int i = 0; i < items.Length; i++)
        {
            ItemList data = items[i].data;

            // ★ 중요: 무기 타입인지 확인 (Shoe, Glove는 false가 됨)
            // Basic(Gun)은 어차피 이미 가지고 있어서 'Upgrade'로 빠지므로 여기 포함되어도 상관없음
            bool isWeapon = (data.itemType == ItemList.ItemType.Melee ||
                             data.itemType == ItemList.ItemType.Rifle ||
                             data.itemType == ItemList.ItemType.Range);

            // 이미 가지고 있는 아이템인지 확인 (강화용)
            bool hasItem = CheckPlayerHasItem(data.itemType);

            // [조건 1] 만렙이면 제외
            if (items[i].level >= data.damages.Length)
                continue;

            // [조건 2] 무기 슬롯 제한 로직
            // "무기이고(Shoe/Glove 아님)", "아직 안 배웠는데(New)", "무기 칸(3개)이 꽉 찼다면" -> 제외
            if (isWeapon && !hasItem && activeWeaponCount >= 3)
                continue;

            validIndices.Add(i);
        }

        // 4. 랜덤 3개 활성화
        int[] ran = new int[3];

        if (validIndices.Count <= 3)
        {
            for (int i = 0; i < validIndices.Count; i++)
                items[validIndices[i]].gameObject.SetActive(true);
        }
        else
        {
            for (int i = 0; i < 3; i++)
            {
                int r = Random.Range(0, validIndices.Count);
                int selectedIndex = validIndices[r];
                items[selectedIndex].gameObject.SetActive(true);
                validIndices.RemoveAt(r);
            }
        }
    }

    // 플레이어가 아이템을 가지고 있는지 확인
    bool CheckPlayerHasItem(ItemList.ItemType type)
    {
        Playermove player = GameManager.instance.Player;

        switch (type)
        {
            case ItemList.ItemType.Basic: // Gun
                return player.GetComponentInChildren<WeaponGun>(true).gameObject.activeSelf;
            case ItemList.ItemType.Melee: // Bat
                return player.GetComponentInChildren<WeaponBat>(true).gameObject.activeSelf;
            case ItemList.ItemType.Rifle: // Sniper
                return player.GetComponentInChildren<WeaponSniper>(true).gameObject.activeSelf;
            case ItemList.ItemType.Range: // Sunlight
                return player.GetComponentInChildren<WeaponSunlight>(true).gameObject.activeSelf;
            default: // Shoe, Glove 등은 스탯이라 "가지고 있다"는 개념(활성화)이 아님 (무조건 false 리턴해서 레벨업마다 뜨게 하거나 별도 로직)
                // 현재 코드 구조상 스탯 아이템은 activeSelf 체크가 없으므로 false 리턴하여 계속 뜨게 함(만렙 전까지)
                return false;
        }
    }
}