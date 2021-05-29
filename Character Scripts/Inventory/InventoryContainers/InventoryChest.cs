using Player;
using UnityEngine;

public class InventoryChest : MonoBehaviour
{
    public InventoryObject inventory;
    public int slotNumber=7;    
    public ItemObject[] items;
    private int random;
    private int itemRandom;
    public ChestInterface interChest;   
    public InventoryObject chestDefaultPrefab;


    private void Awake()
    {
        inventory = Instantiate<InventoryObject>(chestDefaultPrefab);
        inventory.database = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryPlayer>().inventory.database;
        inventory.Container = new Inventory();
        inventory.Container.slots = new InventorySlot[slotNumber];        

        for (int i = 0; i < inventory.Container.slots.Length; i++)
        {
            inventory.Container.slots[i] = new InventorySlot();
        }
       
        items = inventory.database.ItemObjects;
        random = Random.Range(0, slotNumber);

        for (int i = 0; i < random; i++)
        {
            itemRandom = Random.Range(0, items.Length - 1);
            inventory.AddItem(new Item(items[itemRandom]), 1);
        }

        interChest.inventory = inventory;
    }   
}
