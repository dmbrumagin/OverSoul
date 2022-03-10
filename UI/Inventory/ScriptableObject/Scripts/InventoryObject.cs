using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace InventoryRelated
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

    public class InventoryObject : ScriptableObject {
        public ItemObjectDatabase database;
        public Inventory Container;
        public InventorySlot[] getSlot { get { return Container.slots; } }


        public void AddItem(Item _item, int _amount)
        {
            if(_item!=null){
            if (_item.buffs != null && _item.buffs.Length > 0)
            {
                //new slot if custom buffs currently all stats are handled with buffs
                SetEmptySlot(_item, _amount);
                return;
            }

            else
            {
                for (int i = 0; i < Container.slots.Length; i++)
                {
                    if (Container.slots[i].ID == _item.Id)
                    {
                        Container.slots[i].AddAmount(_amount);
                        return;
                    }
                }
                SetEmptySlot(_item, _amount);
                return;
            }}
        }

        public void RemoveItem(InventorySlot _slot)
        {
            InventorySlot temp = new InventorySlot();
            temp.item = new Item();
            _slot.UpdateSlot(temp.ID, temp.item, temp.amount);
        }

        public InventorySlot FirstEmptySlot(Item _item)
        {
            for (int i = 0; i < Container.slots.Length; i++)
            {
                if (Container.slots[i].ID <= -1)
                    return Container.slots[i];
            }

            return null;
        }

        public InventorySlot AnyEmptySlot(Item _item)
        {
            if(_item.Id==-1){ return null;}

            if (_item.buffs.Length <= 0)
            {

                for (int i = 0; i < Container.slots.Length; i++)
                {
                    if (Container.slots[i].ID == _item.Id)
                        return Container.slots[i];
                }

                return FirstEmptySlot(_item);
            }

            else
                return FirstEmptySlot(_item);
        }
        
        public InventorySlot SetEmptySlot(Item _item, int _amount)
        {
            for (int i = 0; i < Container.slots.Length; i++)
            {
                if (Container.slots[i].ID <= -1)
                {
                    Container.slots[i].UpdateSlot(_item.Id, _item, _amount);
                    return Container.slots[i];
                }
            }

            return null;
        }

        public void MoveItem(InventorySlot item1, InventorySlot item2)
        {

            InventorySlot temp = new InventorySlot(item2.ID, item2.item, item2.amount);
            item2.UpdateSlot(item1.ID, item1.item, item1.amount);
            item1.UpdateSlot(temp.ID, temp.item, temp.amount);

        }

        public void TakeItem(InventorySlot item1, InventorySlot item2)
        {

            InventorySlot temp = new InventorySlot();
            temp.item = new Item();

            if (item2.item.buffs.Length<=0)
                item2.UpdateSlot(item1.ID, item1.item, item1.amount + item2.amount);

            item1.UpdateSlot(temp.ID, temp.item, temp.amount);
            
        }
    }

    [System.Serializable]
    public class Inventory
    {
        public InventorySlot[] slots;         
    }
    
    public delegate void SlotUpdate(InventorySlot _slot);

    [System.Serializable]
    public class InventorySlot
    {
        public ItemType AllowedItem;
        public int ID;
        [System.NonSerialized]
        public GameObject slotOnInterface;
        [System.NonSerialized]
        public SlotUpdate OnBeforeUpdate;
        [System.NonSerialized]
        public SlotUpdate OnAfterUpdate;
        public Item item;
        public int amount;

        public InventorySlot()
        {
            slotOnInterface = null;
            UpdateSlot(-1, new Item(), 0);
        }
         public InventorySlot(int _id, Item _item, int _amount)
        {         
            UpdateSlot( _id,  _item,  _amount);
        }
         public void UpdateSlot(int _id, Item _item, int _amount)
        {
            if (OnBeforeUpdate != null)
                OnBeforeUpdate.Invoke(this);

            ID = _id;
            item = _item;
            amount = _amount;

            if (OnAfterUpdate!=null)
                OnAfterUpdate.Invoke(this);
            
        }
        public void UpdateSlot(ItemType _type)
        {
            if (OnBeforeUpdate != null)
                OnBeforeUpdate.Invoke(this);

           

            if (OnAfterUpdate!=null)
                OnAfterUpdate.Invoke(this);
            
        }
        public void resetSlot(){
            UpdateSlot(-1, new Item(), 0);
        }
       
        public void AddAmount(int value)
        {
            UpdateSlot(item.Id, item, amount += value);
        }
        public void SubAmount(int value)
        {
            UpdateSlot(item.Id, item, amount -= value);
        }

        public bool CanPlace(ItemObject _item)
        {
            if (AllowedItem==ItemType.Default)
                return true;

            if (_item.type == AllowedItem)            
                return true;
            
            return false;
        }
    }
}