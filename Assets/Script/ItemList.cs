using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemList : ScriptableObject
{
    public enum ItemType { Basic, Melee, Rifle, Range, Shoe, Glove }

    [Header("Main Info")]
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("Level Data")]
    public float baseDamage;
    public int baseCount;
    public float[] damages;
    public float[] cooltime;
    public int[] counts;

    [Header("Weapon")]
    public GameObject projectile;

}
