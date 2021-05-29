using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Restore Ability", menuName = "Inventory System/Ability/Restore")]
public class RestoreAbility : Player.AbilityObject
{
    public int RestoreHealthValue;

    public void Awake()
    {
        type = Player.AbilityType.Restore;
    }
}
