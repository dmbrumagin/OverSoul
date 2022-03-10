using System.Collections.Generic;
using UnityEngine;
using InventoryRelated;
using System.Linq;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Main Player Stats")]
        [SerializeField]
        public string PlayerName;
        [SerializeField]
        public int PlayerHP = 4;
        [SerializeField]
        public int experienceGained = 0;

        [Header("Player Attributes")]
        public static Dictionary<StatType,Stat> StatTypeToPlayerStat;
        public Stat[] playerStats;

        public delegate void OnStatChange();
        public event OnStatChange statsChanged;
        public void Awake()
        {
            StatTypeToPlayerStat = new Dictionary<StatType, Stat>();

            var i = 0;
            foreach(StatType t in StatType.GetValues(typeof(StatType)))
            {
                StatTypeToPlayerStat.Add(t,playerStats[i++]);
            }
        }
         public static void RecalculateAttributeValue(Buff[] buffs, bool isAdditive = true)
        {
            foreach (Buff buff in buffs)
            {
                foreach (Stat stat in PlayerStats.StatTypeToPlayerStat.Values)
                {
                    if (stat.statType == buff.stat.statType)
                    {
                        var abilityAmount = isAdditive ? buff.value : -1 * buff.value;
                        Debug.Log( new {abilityAmount});
                        stat.setAmount((stat.getAmount() + abilityAmount));
                        PlayerStatsDisplay.UpdateStats();
                    }
                }
            }
        }
        
        public delegate void OnXPChange();
        public event OnXPChange experiencePointsChange;

        [SerializeField]
        private int _PlayerLV = 1;

        public int PlayerLV
        {
            get { return _PlayerLV; }
            set
            {
                _PlayerLV = value;
                if (onLVChange != null)
                    onLVChange();
            }
        }

        public delegate void OnLVChange();
        public event OnLVChange onLVChange;

    }
}