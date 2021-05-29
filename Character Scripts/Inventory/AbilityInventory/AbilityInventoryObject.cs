using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Player
{
    [CreateAssetMenu(fileName = "New Ability Inventory", menuName = "Inventory System/AbilityInventory")]

    public class AbilityInventoryObject : ScriptableObject
    {
        public AbilityObjectDatabase database;
        // database of abilities scriptable object
        public AbilityInventory Container;
        //Container for (player) Inventory Object slots set later
        public AbilityInventorySlot[] getAbilitySlot { get { return Container.abilities; } }

        public void AddAbility(Ability _ability)
        {
            //adds ability to container


            for (int i = 0; i <  Container.abilities.Length; i++)
            {
               // Debug.Log(Container.abilities[i].ID+"_"+ _ability.Id);
                if (Container.abilities[i].ID == _ability.Id)
                {
                    //Debug.Log("ability not set");
                    // if abilityid is in abilityinventory > return
                    return;
                }
                if (Container.abilities[i].ID != _ability.Id)
                {
                   // Debug.Log("ability set");
                    // if ability.id is not in abilityinventory > addability
                    SetEmptySlot( _ability);
                    return;
                }
                
            }

        }

        public AbilityInventorySlot SetEmptySlot(Ability _ability)
        {
            for (int i = 0; i < Container.abilities.Length; i++)
            {
               // Debug.Log(Container.abilities[i].ID);
                if (Container.abilities[i].ID <= -1)
                {
                   // Debug.Log(Container.abilities[i].ID);
                    //cycles through container for first ability not set in list and returns container ability i
                    Container.abilities[i].UpdateSlot(_ability.Id, _ability);
                    return Container.abilities[i];
                }
            }

            return null;
        }

    }

    [System.Serializable]
    public class AbilityInventory
    {
        //sets up number of abilities
        public AbilityInventorySlot[] abilities = new AbilityInventorySlot[4];
    }

    public delegate void AbilitySlotUpdate(AbilityInventorySlot _slot);

    [System.Serializable]
    public class AbilityInventorySlot
    {
        public UserInterface parent;
        //paramaters for ability slot
        public int ID=-1;
        public Ability ability;
        [System.NonSerialized]
        public GameObject slotDisplay;
        [System.NonSerialized]
        public AbilitySlotUpdate OnBeforeUpdate;
        [System.NonSerialized]
        public AbilitySlotUpdate OnAfterUpdate;


        public AbilityInventorySlot()
        {
            //default constructor null for objects not in inventory
            ID = -1;
            ability = null;
            //Debug.Log("null slot");

        }
        public AbilityInventorySlot(int _id, Ability _ability)
        {
            ID = _id;
            ability = _ability;

        }
        public void UpdateSlot(int _id, Ability _ability)
        {
            //neccessary 
            ID = _id;
            ability = _ability;

            if (OnBeforeUpdate != null)
            {
                OnBeforeUpdate.Invoke(this);
            }
            _id = ID;
            _ability = ability;
            if (OnAfterUpdate != null)
            {
                OnAfterUpdate.Invoke(this);
            }

        }


    }
}