using UnityEngine;


namespace Player
{
    [CreateAssetMenu(menuName ="RPG Generator/Player/Create Attribute")]
    public class Stat : ScriptableObject
    {
        public StatType statType;
        private int amount;    

        public void setAmount(int amountToSet)
        {
            amount = amountToSet;
        }
        public int getAmount()
        {
            return amount;
        }

    }

    

    public enum StatType{
        Attack,
        Defence,
        Intelligence,
        Speed,
        Strength,
        Vitality,
        Money,
        Arrow
    }
}
