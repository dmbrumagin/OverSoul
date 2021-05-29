using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Player
{
    public enum AbilityType
    {
        //enumeration of ability types
        Buff,
        Restore,
        Damage,
        Default
    }
   // public enum Attributes
   // {
        //possible affected attrivutes 
   //     Speed,
   //     Intelligence,
   //     Stamina,
   //     Strength,
    //    HP
   // }
    public abstract class AbilityObject : ScriptableObject
    {
        public Ability aby;
        //sprite on field or in display
        public Sprite UIDisplay;
        //enumerated type above
        public AbilityType type;
        //description -unused currently set in inspector
        [TextArea(15, 20)]
        public string description;
        //id set in ability object database
        public int Id;
       // public int LVNeeded;
        public int XPNeeded;
        public AbilityBuff[] buffs2;


        //public Ability CreateAbility()
        //{
        //    Ability newAbility = new Ability(this);
        //    return newAbility;
        //}
    }

    [System.Serializable]
    public class Ability
    {
        public Ability aby; 
        public string Name;
        public int Id;
        public AbilityBuff[] buffs;
       // public int LVNeeded;
        public int XPNeeded;
        public bool activeSkill;
        public Sprite UIDisplay;
        public string description;

        public Ability(AbilityObject ability)
        {
            aby = ability.aby;
            Name = ability.name;
            Id = ability.Id;
           // LVNeeded = ability.LVNeeded;
            XPNeeded =ability.XPNeeded;
            activeSkill =false;
            UIDisplay = ability.UIDisplay;
            description = ability.description;
        //set from ability object database

        buffs = new AbilityBuff[ability.buffs2.Length];
            //sets value to inspector values of ability
            for (int i = 0; i < buffs.Length; i++)
            {
               //Debug.Log(ability.buffs2[i].attribute +"_"+ ability.buffs2[i].value+"_"+ ability.buffs2.Length);
              //  Debug.Log(buffs.Length);
                buffs[i] = ability.buffs2[i];
                //Debug.Log(buffs[i].value);
               // buffs[i].attribute = ability.buffs2[i].attribute;
               // Debug.Log(buffs[i].attribute);
            }
        }

    }

    [System.Serializable]
    public class AbilityBuff
    {
        //set in inspector
        public Attributes attribute;
        public int value;

    }
}
