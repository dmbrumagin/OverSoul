using UnityEngine;
using InventoryRelated;

[CreateAssetMenu(fileName = "New Damage Ability", menuName = "Inventory System/Ability/Damage")]
public class DamageAbility : AbilityObject
{
    public int DamageHealthValue;

    public void Awake()
    {
        type = AbilityType.Damage;
    }
}
