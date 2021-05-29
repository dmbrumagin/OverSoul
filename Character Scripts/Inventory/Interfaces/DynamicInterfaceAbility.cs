using Sounds;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player
{
    public class DynamicInterfaceAbility : UserInterface
    {
        public GameObject abilityInventoryPrefab;
        public int xSpaceBetween;
        public int ySpaceBetween;
        public int numberColumns;
        public int xStart;
        public int yStart;
        public AbilityInventoryObject abilityInventory;
        public TextMeshProUGUI abilityEXP;
        public AbilityObject ability;

        private void Awake()
        {
            playerStats.onXPChange += ReactToChange;
           // UpdateAbilities();
        }

        private void Start()
        {
            soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
            CreateAbilities();
            if (abilityInventory)
            {
                for (int i = 0; i < abilityInventory.Container.abilities.Length; i++)
                {
                    abilityInventory.Container.abilities[i].parent = this;
                   
                        abilityInventory.getAbilitySlot[i].OnAfterUpdate += OnAbilitySlotUpdate;
                    
                }
            }
        }

        private void OnAbilitySlotUpdate(AbilityInventorySlot _slot)
        {
            if (_slot.ability.Id >= 0)
            {
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = abilityInventory.database.GetAbility[_slot.ID].UIDisplay;
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            }

            else
            {
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            }
        }

        private new void OnDisable()
        {

           // UpdateAbilities();
            playerStats.onXPChange += ReactToChange;
        }
        private new void OnEnable()
        {

            UpdateAbilities();
            playerStats.onXPChange += ReactToChange;
        }

        private void Update()
        {
        }

        private Vector3 GetPosition(int i)
        {
            return new Vector3(xStart + xSpaceBetween * (i % numberColumns), yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
        }

        void ReactToChange()
        {
            UpdateAbilities();
        }

        public void CreateAbilities()
        {

            abilitiesOnInterface = new Dictionary<GameObject, AbilityInventorySlot>();
            
            for (int i = 0; i < abilityInventory.Container.abilities.Length; i++)
            {
                var obj = Instantiate(abilityInventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                abilitiesOnInterface.Add(obj, abilityInventory.Container.abilities[i]);
                abilityInventory.getAbilitySlot[i].slotDisplay = obj;
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter2(obj); });
                AddEvent(obj, EventTriggerType.Submit, delegate { OnSubmit(obj); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnClick(); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnExit3(obj); });
                AddEvent(obj, EventTriggerType.Deselect, delegate { OnExit2(); });
            }
            UpdateAbilities();
        }

        public void UpdateAbilities()
        {
            foreach (KeyValuePair<GameObject, AbilityInventorySlot> _slot in abilitiesOnInterface)
            {
                if (_slot.Value.ID == -1)
                {
                    _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = null;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                    _slot.Key.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
                }

                else if (_slot.Value.ability.activeSkill == false && _slot.Value.ability.XPNeeded <= playerStats.PlayerXP)
                {
                    _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.ability.XPNeeded.ToString("n0");
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = abilityInventory.database.GetAbility[_slot.Value.ID].UIDisplay;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, .5f);
                    _slot.Key.GetComponent<Image>().color = new Color(1, 1, 1, .5f);
                }

                else if (_slot.Value.ability.activeSkill == false && _slot.Value.ability.XPNeeded > playerStats.PlayerXP)
                {
                    _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.ability.XPNeeded.ToString("n0");
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = abilityInventory.database.GetAbility[_slot.Value.ID].UIDisplay;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 0, 0, .5f);
                    _slot.Key.GetComponent<Image>().color = new Color(1, 0, 0, .5f);
                }

                else
                {
                    _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.ability.XPNeeded.ToString("n0");
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = abilityInventory.database.GetAbility[_slot.Value.ID].UIDisplay;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                    _slot.Key.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                }
            }


        }
        public bool GetAbility(PlayerStats Player, Ability ability)
        {
            if (ability.activeSkill == false && ability.XPNeeded <= Player.PlayerXP)
            {
                for (int i = 0; i < Player.attributesList.Count; i++)
                {
                    for (int j = 0; j < ability.buffs.Length; j++)
                    {
                        if (Player.attributesList[i].attributes.name == ability.buffs[j].attribute.name)
                        {
                            Player.attributesList[i].amount += ability.buffs[j].value;
                        }
                    }
                }
                soundPlayer.Play("EquipItem");
                Player.PlayerXP -= ability.XPNeeded;
                ability.activeSkill = true;
                return true;
            }

            if (ability.activeSkill == true)
            {
                for (int i = 0; i < Player.attributesList.Count; i++)
                {
                    for (int j = 0; j < ability.buffs.Length; j++)
                    {
                        if (Player.attributesList[i].attributes.name == ability.buffs[j].attribute.name)
                        {
                            Player.attributesList[i].amount -= ability.buffs[j].value;
                        }
                    }
                }
                soundPlayer.Play("UnequipItem");
                Player.PlayerXP += ability.XPNeeded;
                ability.activeSkill = false;
                return true;
            }
            return false;
        }
        public override void CreateSlots()
        {
        }

        public override void OnDragStart(GameObject obj)
        {
        }

        public void OnSubmit(GameObject obj)
        {
            if (GetAbility(playerStats, CursorData.hoverItem2.ability))
            {
                UpdateAbilities();
            }
        }
    }
}
