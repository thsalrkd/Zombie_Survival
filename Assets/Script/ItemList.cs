using UnityEngine;

//아이템 데이터를 저장하기 위한 ScriptableObject
[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Object/ItemData")]
public class ItemList : ScriptableObject
{
    //아이템 타입 구분
    //열거형(enum) 사용
    public enum ItemType { Basic, Melee, Rifle, Range, Shoe, Glove }

    [Header("Main Info")]
    public ItemType itemType;  //아이템 종류
    public int itemId;         //아이템 ID
    public string itemName;    //아이템 이름
    [TextArea]
    public string itemDesc;    //아이템 설명
    public Sprite itemIcon;    //아이콘 이미지

    [Header("Level Data")]
    public float baseDamage;   //기본 데미지
    public float baseCooltime; //기본 쿨타임
    public float[] damages;    //레벨업 시 데미지 상승률 배열
    public float[] cooltime;   //레벨업 시 쿨타임 감소율, 범위 증가율 배열 

    [Header("Weapon")]
    public GameObject projectile; //무기 투사체 프리팹

}
