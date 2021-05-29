using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GroundAbility : MonoBehaviour,ISerializationCallbackReceiver
{
    public Player.AbilityObject ability;

    public void OnAfterDeserialize()
    {
        
    }

    public void OnBeforeSerialize()
    {
        GetComponentInChildren<SpriteRenderer>().sprite = ability.UIDisplay;
      
    }
}
