using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OSSC;

public class ChestSystemInventory : SystemInventory
{
    public void Start()
    {
        inventory = GetComponent<InventoryChest>().inventory;
    }
}
