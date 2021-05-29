using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Player
{
    [CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]

    public class InventoryObject : ScriptableObject {
        public ItemObjectDatabase database;
        public Inventory Container;
        public InventorySlot[] getSlot { get { return Container.slots;} }

        public void AddItem(Item _item, int _amount)
        {
            if (_item.buffs.Length > 0)
            {
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
                //for (int i = 0; i < Container.items.Length; i++)
             //   {
              //      if (Container.items[i].ID != _item.Id)
              //      {
               //         SetEmptySlot(_item, _amount);
               //         return;
               //     }
              //  }
            }
        }
        public void RemoveItem(InventorySlot _slot)
        {
            InventorySlot temp = new InventorySlot();
            temp.item = new Item();
            _slot.UpdateSlot(temp.ID, temp.item, temp.amount);
            Debug.Log("null item REMOVE ITEM");
        }
        public InventorySlot FirstEmptySlot(Item _item)
        {
            for (int i = 0; i < Container.slots.Length; i++)
            {
                if (Container.slots[i].ID <= -1)
                {
                    return Container.slots[i];
                }
            }
            return null;
        }
        public InventorySlot AnyEmptySlot(Item _item)
        {
            Debug.Log("any empty");
            if (_item.buffs != null)
            {

                Debug.Log("any empty not null");

                if (_item.buffs.Length <= 0)
                {
                    for (int i = 0; i < Container.slots.Length; i++)
                    {
                        if (Container.slots[i].ID == _item.Id)
                        {
                            Debug.Log(" buff 0 same item already");
                            return Container.slots[i];
                        }
                    }
                    for (int i = 0; i < Container.slots.Length; i++)
                    {
                        if (Container.slots[i].ID <= -1)//Container.items[i].ID != _item.Id && Container.items[i].ID <= -1)
                        {

                            Debug.Log("buff 0 empty slot");
                            return Container.slots[i];

                        }

                    }
                }

                else
                {
                    for (int i = 0; i < Container.slots.Length; i++)
                    {
                        if (Container.slots[i].ID <= -1)//Container.items[i].ID != _item.Id && Container.items[i].ID <= -1)
                        {

                            Debug.Log("empty slot");
                            return Container.slots[i];

                        }

                    }
                }
            }
                
          //  }
          //  else if (_item.buffs.Length > 0)
           // {
            //    for (int i = 0; i < Container.items.Length; i++)
           //     {
           //         if (Container.items[i].ID <= -1)
            //        {

            //            Debug.Log("3");
           //             return Container.items[i];
           //         }
            //    }
           // }
           
            //  if (Container.items[i].ID != _item.Id)
            //  {
            //       Debug.Log("2");
            //  for (int j = 0; i < Container.items.Length; i++)
            //{
            //         if (Container.items[i].ID <= -1)
            //          {

            //              Debug.Log("3");
            //             return Container.items[i];
       // }
                   // }
             //   }
                
            return null;

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
            {

                item2.UpdateSlot(item1.ID, item1.item, item1.amount + item2.amount);

            }
            else
            {
               // item2.SetEmptySlot(item1.ID, item1.item, item1.amount + item2.amount)
            }
            item1.UpdateSlot(temp.ID, temp.item, temp.amount);
            
        }



        [ContextMenu("Clear")]
        public void Clear()
        {            
            Container = new Inventory();
        }        

        //   public void OnBeforeSerialize()
        //   {
        //       throw new System.NotImplementedException();
        //  }
        //  public void OnAfterDeserialize()
        //  {
        //          for (int i = 0; i < Container.items.Length; i++)
        //          {
        //         Container.items[i].ID = i;
        //               database.GetItem.Add(i, database.Items[i]);
        //           }
        // }
        //  [ContextMenu("Save")]
        //   public void Save()
        //  {
        // string saveData = JsonUtility.ToJson(this, true);
        //  BinaryFormatter bf = new BinaryFormatter();
        //  FileStream file = File.Create(string.Concat(Application.persistentDataPath, savePath));
        //  bf.Serialize(file, saveData);
        //  file.Close();
        //       IFormatter formatter = new BinaryFormatter();
        //       Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Create, FileAccess.Write);
        //       formatter.Serialize(stream, Container);
        //       stream.Close();
        //   }
        // [ContextMenu("Load")]
        // public void Load()
        //  {
        //    if (File.Exists(string.Concat(Application.persistentDataPath, savePath)))
        //     {
        //  BinaryFormatter bf = new BinaryFormatter();
        //  FileStream file = File.Open(string.Concat(Application.persistentDataPath, savePath), FileMode.Open);
        //  JsonUtility.FromJsonOverwrite(bf.Deserialize(file).ToString(),this);
        //  file.Close();
        //       IFormatter formatter = new BinaryFormatter();
        //        Stream stream = new FileStream(string.Concat(Application.persistentDataPath, savePath), FileMode.Open, FileAccess.Read);
        //        Inventory newContainer = (Inventory)formatter.Deserialize(stream);
        //         for (int i = 0; i < Container.items.Length; i++)
        //          {
        //              Container.items[i].UpdateSlot(newContainer.items[i].ID, newContainer.items[i].item, newContainer.items[i].amount);
        //          }
        //          stream.Close();
        //      }
        //   }
    }

    [System.Serializable]
    public class Inventory
    {
        public InventorySlot[] slots =new InventorySlot[7];

        public void Clear()
        {
            Debug.Log(slots.Length);
            for (int i = 0; i < slots.Length; i++)
            {
                slots[i].UpdateSlot(-1, new Item(), 0);
            }
        }                
    }
    
    public delegate void SlotUpdate(InventorySlot _slot);

    [System.Serializable]
    public class InventorySlot
    {
        public ItemType[] AllowedItems=new ItemType[0];
        public UserInterface parent;
        public int ID;
        [System.NonSerialized]
        public GameObject slotDisplay;
        [System.NonSerialized]
        public SlotUpdate OnBeforeUpdate;
        [System.NonSerialized]
        public SlotUpdate OnAfterUpdate;
        public Item item;
        public int amount;

        public InventorySlot()
        {
            slotDisplay = null;
            UpdateSlot(-1, new Item(), 0);
        }

        public InventorySlot(int _id, Item _item, int _amount)
        {         
            UpdateSlot( _id,  _item,  _amount);
        }

        public void UpdateSlot(int _id, Item _item, int _amount)
        {
            if (OnBeforeUpdate != null)
            {
                OnBeforeUpdate.Invoke(this);
            }
            ID = _id;
            item = _item;
            amount = _amount;
            if (OnAfterUpdate!=null)
            {
                OnAfterUpdate.Invoke(this);
            }
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
            if (AllowedItems.Length <= 0)
                return true;
            for (int i = 0; i < AllowedItems.Length; i++)
            {
                if (_item.type == AllowedItems[i])
                    return true;
            }
            return false;
        }
    }
}