using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment Object", menuName = "Inventory System/Items/Equipment")]
public class EquipmentObject : ItemObject
{
    //ItemObject itemThis;
    //  public int AttackValue;
    //  public int DefenceValue;
    //  public int SpeedValue;
    public ItemType typing;
    public void Awake()
    {
        //item = this;
        //itemThis = this;
        type = typing;
    }
}
