using Player;
using UnityEngine;
using InventoryRelated;

public class ChestInventoryHandler : MonoBehaviour
{
    public InventoryObject inventory;
    private int slotNumber=7;    
    private ItemObject[] possibleItems;
    private int numberItems;
    private int randomItem;
    public ChestDisplay chestInterface;   
    public InventoryObject chestDefaultPrefab;


    private void Start()
    {
        inventory = Instantiate<InventoryObject>(chestDefaultPrefab);
        AssetBundle asset = AssetLoad.LoadAssetBundle("/database");
        inventory.database = asset.LoadAsset<ItemObjectDatabase>("itemDatabase");
        asset.Unload(false);
        inventory.Container = new Inventory();
        inventory.Container.slots = new InventorySlot[slotNumber];

        for (int i = 0; i < inventory.Container.slots.Length; i++)
        {
            inventory.Container.slots[i] = new InventorySlot();
        }
       //TODO copy array instead of array ref
        possibleItems = inventory.database.ItemObjects;
        numberItems = Random.Range(0, slotNumber);

        for (int i = 0; i < numberItems; i++)
        {
            randomItem = Random.Range(0, possibleItems.Length - 1);
            inventory.AddItem(new Item(possibleItems[randomItem]), 1);
        }
    }   
}
