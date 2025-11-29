using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

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
        //아이템 비활성화
        foreach(SelectItem item in items)
        {
            item.gameObject.SetActive(false);
        }
        //3개만 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);

            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for(int i=0;i<ran.Length;i++)
        {
            SelectItem ranItem = items[ran[i]];

            //만렙 아이템 비활성화
            if(ranItem.level == ranItem.data.damages.Length)
            {
                items[Random.Range(4, 5)].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }


    }
}
