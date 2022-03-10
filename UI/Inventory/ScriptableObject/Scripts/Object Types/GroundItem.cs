using UnityEngine;
using InventoryRelated;

public class GroundItem : MonoBehaviour,ISerializationCallbackReceiver
{
    public ItemObject item;

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = item.UIDisplay;
      
    }
}
