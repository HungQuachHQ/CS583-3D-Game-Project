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

    [Header("Consumable")]
    public ConsumableType consumableType = ConsumableType.None;

    // Health potions
    [Range(0f, 1f)]
    public float healFraction = 0f;            // e.g. 0.25f = 25% of max HP

    // Stamina potions
    public float speedMultiplier = 1f;         // e.g. 1.5f = +50% speed
    public float buffDurationSeconds = 20f;    // used by all timed buffs

    // Attack / Defense potions
    public int attackBonusPerPotion = 0;
    public int defenseBonusPerPotion = 0;
    public int maxAttackBonusFromPotions = 10;
    public int maxDefenseBonusFromPotions = 10;
}
