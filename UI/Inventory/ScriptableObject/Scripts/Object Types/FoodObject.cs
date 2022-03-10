using UnityEngine;
using InventoryRelated;

[CreateAssetMenu(fileName = "New Food Object", menuName = "Inventory System/Items/Food")]
public class FoodObject : ItemObject
{
    public int RestoreHealthValue;

    public void Awake()
    {
        type = ItemType.Food;
    }
}
