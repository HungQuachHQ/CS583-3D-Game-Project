using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "DungeonCrawler/Item")]
public class ItemData : ScriptableObject
{
    [Header("General")]
    public string itemId;
    public string displayName;
    
    [TextArea] public string description;
    public Sprite icon;
    public ItemType itemType;

    [Header("Weapon")]
    public WeaponType weaponType;
    public int physicalDamageBonus;
    public int magicDamageBonus;

    [Header("Stacking")]
    public bool isStackable = false;
    public int maxStackSize = 1;
}
