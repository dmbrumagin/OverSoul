using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Food,
    Weapon,
    Hat,
    Tunic,
    Soul,
    Default
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

    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }

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

    public Item()
    {
        Name = "";
        Id = -1;
        type = default;
        price = 0;
        description = null;
        buffs = new ItemBuff[0] ;
    }
    public Item(ItemObject item)
    {
    
        Name = item.name;
        Id = item.Id;
        type = item.type;        
        price = item.price;
        description = item.description;
        buffs = new ItemBuff[item.buffs.Length];
        for (int i = 0; i < buffs.Length; i++)
        {           
            buffs[i] = new ItemBuff(item.buffs[i].min, item.buffs[i].max, item.buffs[i].value);
            buffs[i].attribute = item.buffs[i].attribute;
            
        }
    }

}

[System.Serializable]
public class ItemBuff
{
    public Player.Attributes attribute;
    public int value;
    public int min;
    public int max;
    public ItemBuff(int _min, int _max,int _value)
    {
        min = _min;
        max = _max;
        if (max == 0)
        {
            value = _value;
        }
        else
        {
            generateValue();
        }
    }
    public ItemBuff()
    {
            value =0 ;
        
    }
    public void generateValue()
    {
        value = Random.Range(min, max);
    }
}