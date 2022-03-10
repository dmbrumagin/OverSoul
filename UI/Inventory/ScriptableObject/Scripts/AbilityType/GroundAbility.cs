using UnityEngine;
using InventoryRelated;

public class GroundAbility : MonoBehaviour,ISerializationCallbackReceiver
{
    public AbilityObject ability;

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = ability.UIDisplay;
    }
}
