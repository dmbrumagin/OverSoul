using Player;
using UnityEngine;
using InventoryRelated;

public class ShopInventoryHandler : MonoBehaviour
{
    public InventoryObject shopDefaultPrefab;
    public InventoryObject inventory;   
    public ItemObject[] itemObjects;


    private void Awake()
    {
        inventory = Instantiate<InventoryObject>(shopDefaultPrefab);
        inventory.database = PlayerInventoryHandler.inventory.database;
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
    }
}
