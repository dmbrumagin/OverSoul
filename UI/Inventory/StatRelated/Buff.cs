using Player;
using UnityEngine;

namespace InventoryRelated
{
    public abstract class Buff
    {
        public Stat stat;
        public int value;
    }

    [System.Serializable]
    public class AbilityBuff : Buff
    {

    }

    [System.Serializable]
    public class ItemBuff : Buff
    {
        public int min, max;
        public ItemBuff(int _min, int _max, int _value)
        {
            min = _min;
            max = _max;

            if (max == 0)
                value = _value;
            else
                generateValue();
        }
        public ItemBuff()
        {
            value = 0;
        }
        public void generateValue()
        {
            value = Random.Range(min, max);
        }
    }

}