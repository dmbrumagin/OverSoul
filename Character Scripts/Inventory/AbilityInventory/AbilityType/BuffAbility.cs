using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Buff Ability", menuName = "Inventory System/Ability/Buff")]
public class BuffAbility : Player. AbilityObject
{
   

    public void Awake()
    {
        type =  Player.AbilityType.Buff;
    }
}
