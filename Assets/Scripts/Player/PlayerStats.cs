using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Base Stats")]
    public int basePhysicalDamage = 5;
    public int baseMagicDamage = 2;

    [Header("Runtime Stats")]
    public int currentPhysicalDamage;
    public int currentMagicDamage;

    private ItemData equippedWeapon;

   
    
    
    // Start is called before the first frame update
    private void Start()
    {
        RecalculateStats();
    }


    public void EquipWeapon(ItemData weapon)
    {
        if(weapon == null || weapon.itemType != ItemType.Weapon)
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

        if(equippedWeapon != null)
        {
            currentPhysicalDamage += equippedWeapon.physicalDamageBonus;
            currentMagicDamage += equippedWeapon.magicDamageBonus;
        }
    }
}
