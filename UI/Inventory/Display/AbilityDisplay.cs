

using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Player;
using InventoryRelated;

namespace UI
{
    public class AbilityDisplay : MenuDisplay
    {
    // PUBLIC SETTINGS ********************************************************

    public GameObject abilityInventoryPrefab;
    public Dictionary<GameObject, AbilityInventorySlot> ObjectCurrentAbilitySlotMapping;        
    public AbilityInventoryObject abilityInventory;
    public int xSpaceBetween;
    public int ySpaceBetween;
    public int numberColumns;
    public int xStart;
    public int yStart;

    // PRIVATE STATE **********************************************************

    private PlayerStats playerStats;    
    

    // UNITY METHODS **********************************************************

    private void Awake()
    {            
        ObjectCurrentAbilitySlotMapping = new Dictionary<GameObject, AbilityInventorySlot>();
    }

    private void Start()
    {
        InitializeReferences();
        playerStats=GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        CreateAbilities();
        UpdateAbilities();
    }

    private void OnEnable()
    {
        UpdateAbilities();
    }

    private new void OnDisable()
    {
        UpdateAbilities();
    }

    // EVENT HANDLERS *********************************************************

    public void SetEvents(GameObject obj)
    {
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnAbilityEnter(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnSubmit(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
        AddEvent(obj, EventTriggerType.Deselect, delegate { OnExit(obj); });
    }

    // PRIVATE METHODS ********************************************************       

    private bool isEmpty(AbilityInventorySlot abilitySlot){
        return abilitySlot.ID == -1;
    }    

    private bool isNotActive(AbilityInventorySlot abilitySlot){
        return !abilitySlot.ability.activeSkill;
    }

    private bool playerHasEnoughPoints(AbilityInventorySlot abilitySlot){
        return abilitySlot.ability.requiredAbilityPoints <= playerStats.experienceGained;
    }

    private void OnAbilitySlotUpdate(AbilityInventorySlot slot)
    {
        changeAbilityVisuals(slot);
    }
        
    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + xSpaceBetween * (i % numberColumns), yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
    }

    private void CreateAbilities()
    {                 
        var i = 0;

        foreach(AbilityInventorySlot abilitySlot in abilityInventory.Container.abilities)
        {
           
            var obj=createAbility(abilitySlot);
             if(i==0)
             objectDefault= obj;
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i++);
            SetEvents(obj);
        }            
    
    }
    private GameObject createAbility(AbilityInventorySlot abilitySlot)
    {
        var obj = Instantiate(abilityInventoryPrefab, Vector3.zero, Quaternion.Euler(0,90,0), transform);
            
        ObjectCurrentAbilitySlotMapping.Add(obj, abilitySlot);
        abilitySlot.onInterface = obj;
        abilitySlot.OnAfterUpdate += OnAbilitySlotUpdate;
                
        return obj;
    }  

    private void changeAbilityVisuals(AbilityInventorySlot abilitySlot)
    {
        if (isEmpty(abilitySlot))
        {
            updateAbilityValues(abilitySlot, new Color(1, 0, 0, 0));
        }
        else if (isNotActive(abilitySlot) && playerHasEnoughPoints(abilitySlot))
        {
            updateAbilityValues(abilitySlot, new Color(1, 1, 1, .5f));
        }
        else if (isNotActive(abilitySlot) && !playerHasEnoughPoints(abilitySlot))
        {
            updateAbilityValues(abilitySlot, new Color(1, 0, 0, .5f));
        }
        else
        {
            updateAbilityValues(abilitySlot, new Color(1, 1, 1, 1));
        }
    }

    private void updateAbilityValues(AbilityInventorySlot abilitySlot, Color color)
    {
        if(isEmpty(abilitySlot))
        {
            abilitySlot.onInterface.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text =null;
            abilitySlot.onInterface.transform.GetChild(0).GetComponentInChildren<Image>().sprite=null;                            
        }
        else
        {
            abilitySlot.onInterface.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = abilitySlot.ability.requiredAbilityPoints.ToString("n0");
            abilitySlot.onInterface.transform.GetChild(0).GetComponentInChildren<Image>().sprite = abilityInventory.database.GetAbility[abilitySlot.ID].UIDisplay;          
            abilitySlot.onInterface.GetComponent<Image>().color = color; 
        }

        abilitySlot.onInterface.transform.GetChild(0).GetComponentInChildren<Image>().color = color;   
    }

    private void UpdateAbilities()
    {
        foreach (KeyValuePair<GameObject, AbilityInventorySlot> _slot in ObjectCurrentAbilitySlotMapping)
        {
            changeAbilityVisuals(_slot.Value);               
        }            
    }
    private bool ToggleAbility(AbilityInventorySlot abilitySlot)
    {
        var isChanged = false;
        Debug.Log("Toggle");
        var abilityPoints = playerStats.experienceGained;
        Debug.Log(abilityPoints);
        if (abilitySlot.ability.activeSkill == false && abilitySlot.ability.requiredAbilityPoints <= abilityPoints &&abilitySlot.ID!=-1)
        {
            PlayerStats.RecalculateAttributeValue(abilitySlot.ability.buffs);
            soundPlayer.Play("EquipItem");
            playerStats.experienceGained -= abilitySlot.ability.requiredAbilityPoints;
            abilitySlot.ability.activeSkill = true;
            isChanged = true;
        }            
        else if (abilitySlot.ability.activeSkill == true)
        {
            PlayerStats.RecalculateAttributeValue(abilitySlot.ability.buffs, isAdditive: false);
            soundPlayer.Play("UnequipItem");
            playerStats.experienceGained += abilitySlot.ability.requiredAbilityPoints;
            abilitySlot.ability.activeSkill = false;
            isChanged = true;
        }
        return isChanged;
    }

    private void OnSubmit(GameObject obj)
    {
        if (ToggleAbility(CursorData.abilitySlotHoveredOver)) {
            UpdateAbilities();
        }
    }
    protected void OnAbilityEnter(GameObject obj)
    {
        CursorData.slotHoveredOver = obj;

        if (ObjectCurrentAbilitySlotMapping.ContainsKey(obj)){
            CursorData.abilitySlotHoveredOver = ObjectCurrentAbilitySlotMapping[obj];
            CursorData.abilityHoveredOver = ObjectCurrentAbilitySlotMapping[obj].ability;
        }
    }

    }
}
