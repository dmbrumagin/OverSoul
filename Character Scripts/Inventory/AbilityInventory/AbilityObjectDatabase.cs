using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Player
{
    [CreateAssetMenu(fileName = "New Ability Database", menuName = "Inventory System/Ability/Database")]
    public class AbilityObjectDatabase : ScriptableObject, ISerializationCallbackReceiver
    {
        public AbilityObject[] Abilities;
        //database of all abilities

        public Dictionary<int, AbilityObject> GetAbility = new Dictionary<int, AbilityObject>();
        //dictionary of abilities against an id

        public void OnAfterDeserialize()
        {

            for (int i = 0; i < Abilities.Length; i++)
            {
                Abilities[i].Id = i;
                //for every id in the database
                GetAbility.Add(i, Abilities[i]);
               
                //feed in the id to get the ability needed to display in menu
            }
        }
        private void OnEnable()
        {
           // Debug.Log(GetAbility[0]);
           // Debug.Log(GetAbility[1]);
           // Debug.Log(GetAbility[2]);
           // Debug.Log(GetAbility[3]);
        }
        public void OnBeforeSerialize()
        {
            GetAbility = new Dictionary<int, AbilityObject>();
            //needed to display in menu
        }

    }
}

