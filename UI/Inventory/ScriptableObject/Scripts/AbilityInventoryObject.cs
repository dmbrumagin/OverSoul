using UnityEngine;
using UI;

namespace InventoryRelated
{
    [CreateAssetMenu(fileName = "New Ability Inventory", menuName = "Inventory System/AbilityInventory")]

    public class AbilityInventoryObject : ScriptableObject
    {
        public AbilityObjectDatabase database;
        public AbilityInventory Container;

        public AbilityInventorySlot[] getAbilitySlot { get { return Container.abilities; } }

        public void AddAbility(Ability _ability)
        {

            for (int i = 0; i <  Container.abilities.Length; i++)
            {
                if (Container.abilities[i].ID == _ability.Id)
                    return;

                if (Container.abilities[i].ID != _ability.Id)
                {
                    SetEmptySlot( _ability);
                    return;
                }                
            }
        }

        public void RemoveAbility(AbilityObject ability){
            for (int i = 0; i <  Container.abilities.Length; i++)
            {
                if (Container.abilities[i].ID == ability.Id){
                    
                    if(Container.abilities[i].ability.isPermanentSkill){   
                    Debug.Log("ability found permanent");
                    return;}
                    else{
                        Debug.Log("ability found update");
                        Container.abilities[i].UpdateSlotPermanentXP(-1);
                    }
                }             
            }

        }

        public AbilityInventorySlot SetEmptySlot(Ability _ability)
        {
            for (int i = 0; i < Container.abilities.Length; i++)
            {
                if (Container.abilities[i].ID <= -1)
                {
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
        public AbilityInventorySlot[] abilities;
    }

    public delegate void AbilitySlotUpdate(AbilityInventorySlot _slot);

    [System.Serializable]
    public class AbilityInventorySlot
    {
        public MenuDisplay parent;
        public int ID=-1;
        public Ability ability;
        [System.NonSerialized]
        public GameObject onInterface;
        [System.NonSerialized]
        public AbilitySlotUpdate OnBeforeUpdate;
        [System.NonSerialized]
        public AbilitySlotUpdate OnAfterUpdate;


        public AbilityInventorySlot()
        {
            ID = -1;
            UpdateSlot(-1, new Ability());
        }

        public AbilityInventorySlot(int _id, Ability _ability)
        {
            ID = _id;
            ability = _ability;
        }

        public void UpdateSlot(int _id, Ability _ability)
        {
           

            if (OnBeforeUpdate != null)
            {
                OnBeforeUpdate.Invoke(this);
            }

             ID = _id;
            ability = _ability;

            if (OnAfterUpdate != null)
            {
                OnAfterUpdate.Invoke(this);
            }
        }

        public void UpdateSlotPermanentXP(int _id)
        {
           

            if (OnBeforeUpdate != null)
            {
                OnBeforeUpdate.Invoke(this);
            }

             ID = _id;
             /*var newAbility = new Ability();
             newAbility.description =  _ability.description;
             newAbility.experienceToLearn =  _ability.experienceToLearn;
             newAbility.Id =  _ability.Id;
             newAbility.monsterTypeToLearn =  _ability.monsterTypeToLearn;
             newAbility.LVNeeded =  _ability.LVNeeded;
             newAbility.Name =  _ability.Name;
             newAbility.requiredAbilityPoints =  _ability.requiredAbilityPoints;
             newAbility.buffs =  _ability.buffs;
             Debug.Log(new{newAbility.experienceToLearn});

            ability = newAbility;*/

            if (OnAfterUpdate != null)
            {
                OnAfterUpdate.Invoke(this);
            }
        }
    }
}