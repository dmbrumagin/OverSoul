using UnityEngine;
using InventoryRelated;

[CreateAssetMenu(fileName = "New Buff Ability", menuName = "Inventory System/Ability/Buff")]
public class BuffAbility : AbilityObject
{
    public void Awake()
    {
        type =  AbilityType.Buff;
    }
}
