using System.Collections.Generic;
using UnityEngine;

namespace InventoryRelated
{
    [CreateAssetMenu(fileName = "New Ability Database", menuName = "Inventory System/Ability/Database")]
    public class AbilityObjectDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public AbilityObject[] Abilities;
        public Dictionary<int, AbilityObject> GetAbility = new Dictionary<int, AbilityObject>();

        public void OnAfterDeserialize()
        {
            int i =0;
            foreach(AbilityObject abilityObject in Abilities) 
            {
                abilityObject.Id = i;
                GetAbility.Add(i++, abilityObject);
            }
        }

        public void OnBeforeSerialize()
        {
            GetAbility = new Dictionary<int, AbilityObject>();
        }

    }
}

