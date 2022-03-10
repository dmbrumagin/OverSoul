using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using InventoryRelated;

namespace UI
{
    public class EquipmentDisplay : ItemDisplay
    {

    // UNITY METHODS **********************************************************

    private void Awake(){
        ObjectCurrentSlotMapping = new Dictionary<GameObject, InventorySlot>();
    }

    private void Start()
    {
        InitializeReferences();
        CreateSlots();            
        UpdateAllSlotsOnInterface();
    } 


    // EVENT HANDLERS *********************************************************

    private new void SetEvents(GameObject obj){
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnMove(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
        AddEvent(obj, EventTriggerType.Deselect, delegate { OnExit(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { TooltipEvent(obj); });
    }

    private void setSubMenuEvents(EquipmentDisplay inventoryScreen){
        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, delegate { PlayerInventoryHandler.UnEquipItem(ObjectCurrentSlotMapping[previouslySelectedObject]); });
        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
    }

    // PRIVATE METHODS ********************************************************

               
    private void CreateSlots()
    {

        int i= 0;    
        var val  =ItemType.GetValues(typeof(ItemType)); 
           
        foreach (Transform t in this.gameObject.transform)
        {
            PlayerInventoryHandler.equipInventory.getSlot[i].slotOnInterface = t.gameObject;
            ObjectCurrentSlotMapping.Add(t.gameObject, PlayerInventoryHandler.equipInventory.Container.slots[i]);          
            SetEvents(t.gameObject);
            PlayerInventoryHandler.equipInventory.getSlot[i].OnAfterUpdate += OnSlotUpdate;
            PlayerInventoryHandler.equipInventory.Container.slots[i].AllowedItem=(ItemType)val.GetValue(++i);
        }
            
    }
        
    protected override void OnDragStart(GameObject obj)
    {
        if (CursorData.cursorInventorySlot == null)
        {
            previouslySelectedObject = obj;
            TurnOnSubMenu();                
            setSubMenuEvents(this);
            var buttonText = MenuInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

            if (PlayerInventoryHandler.IsEquipable(ObjectCurrentSlotMapping[previouslySelectedObject]))
                buttonText.text = "UnEquip";
            else{buttonText.text = "...";}          
                
            SetToFirstSubMenuChild();
        }
    }

    }
}

