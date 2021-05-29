using Player;
using UnityEngine;
using OSSC;

public class InventoryShop : MonoBehaviour
{
    public InventoryObject shopDefaultPrefab;
    public InventoryObject inventory;   
    public ItemObject[] itemObjects;


    private void Awake()
    {
        inventory = Instantiate<InventoryObject>(shopDefaultPrefab);
        GetComponent<ShopInterface>().inventory = inventory;
        inventory.database = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryPlayer>().inventory.database;
        inventory.Container = new Inventory();
        inventory.Container.slots = new InventorySlot[itemObjects.Length];
        for (int i = 0; i < inventory.Container.slots.Length; i++)
        {
            inventory.Container.slots[i] = new InventorySlot();
        }

        for (int i = 0; i < itemObjects.Length; i++)
        {
            inventory.AddItem(new Item(itemObjects[i]), 1);
        }
        //inventory.Container.slots[i].item
        // inventory.AddItem( , 1);  Container.slots[i].item


    }
}
