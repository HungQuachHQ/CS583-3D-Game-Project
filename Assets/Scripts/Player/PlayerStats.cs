using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int basePhysicalDamage = 5;
    public int baseMagicDamage = 2;
    public int baseDefense = 0;

    [Header("Runtime Stats")]
    public int currentPhysicalDamage;
    public int currentMagicDamage;
    public int currentDefense;

    private ItemData equippedWeapon;

    // potion-only buffs (do not serialize)
    int attackBuffFromPotions = 0;
    int defenseBuffFromPotions = 0;

    public int AttackBuffFromPotions => attackBuffFromPotions;
    public int DefenseBuffFromPotions => defenseBuffFromPotions;

    private int levelBonusPhysical = 0;
    private int levelBonusMagic = 0;
    private int levelBonusDefense = 0;

    public void ApplyLevelBonuses(int physical, int magic,  int defense)
    {
        levelBonusPhysical = physical;
        levelBonusMagic = magic;
        levelBonusDefense = defense;
        RecalculateStats();
    }



    private void Start()
    {
        RecalculateStats();
    }

    public void EquipWeapon(ItemData weapon)
    {
        if (weapon == null || weapon.itemType != ItemType.Weapon)
        {
            Debug.LogWarning("Tried to equip a non-weapon item.");
            return;
        }

        equippedWeapon = weapon;
        RecalculateStats();
        Debug.Log($"Equipped weapon: {equippedWeapon.displayName}");
    }

    public void UnequipWeapon()
    {
        equippedWeapon = null;
        RecalculateStats();
        Debug.Log("Unequipped weapon.");
    }

    public ItemData GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    private void RecalculateStats()
    {
        currentPhysicalDamage = basePhysicalDamage;
        currentMagicDamage = baseMagicDamage;
        currentDefense = baseDefense;

        //Level bonuses
        currentPhysicalDamage += levelBonusPhysical;
        currentMagicDamage += levelBonusMagic;
        currentDefense += levelBonusDefense;

        if (equippedWeapon != null)
        {
            currentPhysicalDamage += equippedWeapon.physicalDamageBonus;
            currentMagicDamage += equippedWeapon.magicDamageBonus;
        }

        currentPhysicalDamage += attackBuffFromPotions;
        currentDefense += defenseBuffFromPotions;
    }

    // Called by buff system
    public void AddAttackBuff(int amount)
    {
        attackBuffFromPotions += amount;
        RecalculateStats();
    }

    public void RemoveAttackBuff(int amount)
    {
        attackBuffFromPotions -= amount;
        if (attackBuffFromPotions < 0) attackBuffFromPotions = 0;
        RecalculateStats();
    }

    public void AddDefenseBuff(int amount)
    {
        defenseBuffFromPotions += amount;
        RecalculateStats();
    }

    public void RemoveDefenseBuff(int amount)
    {
        defenseBuffFromPotions -= amount;
        if (defenseBuffFromPotions < 0) defenseBuffFromPotions = 0;
        RecalculateStats();
    }
}
