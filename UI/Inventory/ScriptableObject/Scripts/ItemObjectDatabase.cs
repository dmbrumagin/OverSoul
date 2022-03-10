using System.Collections.Generic;
using UnityEngine;

namespace InventoryRelated
{
    [CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
    public class ItemObjectDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public ItemObject[] ItemObjects;
        public Dictionary<int, ItemObject> GetItem = new Dictionary<int, ItemObject>();

        public void OnBeforeSerialize()
        {
            GetItem = new Dictionary<int, ItemObject>();
        }

        public void OnAfterDeserialize()
        {
            for (int i = 0; i < ItemObjects.Length; i++)
            {
                ItemObjects[i].Id = i;
                GetItem.Add(i, ItemObjects[i]);
            }
        }
    }
}