using UnityEngine;
using Player;

namespace InventoryRelated
{
    public enum ItemType
    {
        Default,
        Weapon,
        Hat,
        Tunic,
        Soul,
        Food    
    }

    public abstract class ItemObject : ScriptableObject
    {
        public Sprite UIDisplay;
        public ItemType type;
        [TextArea(15, 20)]
        public string description;

        public int Id;
        public int price;
        public ItemBuff[] buffs;
        public string Name;  
        public AbilityObject ability;  
    }

    [System.Serializable]
    public class Item
    {
        public string Name;
        public int Id;
        public ItemType type;    
        public int price;
        public string description;
        public ItemBuff[] buffs;
        public AbilityObject ability;

        public Item()
        {
            Name = null;
            Id = -1;
            price = 0;
            description = null;
            buffs = null;
        }
        public Item(ItemObject item)
        {
    
            Name = item.Name;
            Id = item.Id;
            type = item.type;        
            price = item.price;
            description = item.description;
            buffs = new ItemBuff[item.buffs.Length];
            if(item.ability!=null)
                ability = item.ability;

            for (int i = 0; i < buffs.Length; i++)
            {           
                buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max, item.buffs[i].value);
                buffs[i].stat = item.buffs[i].stat;
            }
        }    
    }
}