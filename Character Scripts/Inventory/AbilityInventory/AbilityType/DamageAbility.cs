using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Damage Ability", menuName = "Inventory System/Ability/Damage")]
public class DamageAbility : Player.AbilityObject
{
    public int DamageHealthValue;

    public void Awake()
    {
        type = Player.AbilityType.Damage;
    }
}
