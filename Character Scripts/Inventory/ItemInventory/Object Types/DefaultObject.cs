using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Default Object",menuName ="Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    // Start is called before the first frame update
    public void Awake()
    {
        type = ItemType.Default;
    }
}
