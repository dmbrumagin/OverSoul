using UnityEngine;
using Player;

namespace InventoryRelated
{
    public enum AbilityType
    {
        Buff,
        Restore,
        Damage,
        Default
    }

    public abstract class AbilityObject : ScriptableObject
    {
        public Ability ability;
        public Sprite UIDisplay;
        public AbilityType type;
        public int Id;
    }

    public delegate void AbilityUpdate(Ability ability);

     

    [System.Serializable]
    public class Ability
    {
       
        public string Name;
        public int Id;
        public AbilityBuff[] buffs;
        public int requiredAbilityPoints;
        public int experienceToLearn;
        public int LVNeeded;
        public bool activeSkill;
        public bool isPermanentSkill;

        public bool isAlreadyAquired=false;

        public EnemyDamageHandler.typeOfMonster monsterTypeToLearn;


        [TextArea(15, 20)]
        public string description;

         [System.NonSerialized]
        public AbilityUpdate OnBeforeUpdate;
        [System.NonSerialized]
        public AbilityUpdate OnAfterUpdate;

        public Ability()
        {
            Id = -1;
            LVNeeded = 0;
            requiredAbilityPoints = 0;
            experienceToLearn = 0;
            activeSkill = false;
            description = null;
            buffs = null;
            isPermanentSkill= false;
        }

        public Ability(AbilityObject abilityObject)
        {
            if(!isAlreadyAquired){
            Name = abilityObject.name;
            
            Id = abilityObject.Id;
            LVNeeded = abilityObject.ability.LVNeeded;
            requiredAbilityPoints = abilityObject.ability.requiredAbilityPoints;
            if(experienceToLearn>abilityObject.ability.experienceToLearn||!isPermanentSkill)
            experienceToLearn = abilityObject.ability.experienceToLearn;
            activeSkill = false;
            isPermanentSkill= false;
            description = abilityObject.ability.description;
            buffs = new AbilityBuff[abilityObject.ability.buffs.Length];

            for (int i = 0; i < buffs.Length; i++)
            {
                buffs[i] = abilityObject.ability.buffs[i];
            }isAlreadyAquired= true;
            }
        
        }

         public void UpdateAbility(int experience)
        {          
            if (OnBeforeUpdate != null)
            {
                OnBeforeUpdate.Invoke(this);
            }
            if(Id!=-1)
                experienceToLearn -= experience;
            if(experienceToLearn<=0)
                isPermanentSkill=true;

            if (OnAfterUpdate != null)
            {
                OnAfterUpdate.Invoke(this);
            }
        }

    }
}
