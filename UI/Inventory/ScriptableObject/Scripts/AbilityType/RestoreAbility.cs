using UnityEngine;
using InventoryRelated;

[CreateAssetMenu(fileName = "New Restore Ability", menuName = "Inventory System/Ability/Restore")]
public class RestoreAbility : AbilityObject
{
    public int RestoreHealthValue;

    public void Awake()
    {
        type = AbilityType.Restore;
    }
}
