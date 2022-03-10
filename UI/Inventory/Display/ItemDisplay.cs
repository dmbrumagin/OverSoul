using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using InventoryRelated;

namespace UI{
    public abstract class ItemDisplay : MenuDisplay
    {
    
    // PUBLIC SETTINGS ********************************************************

    public Dictionary<GameObject, InventorySlot> ObjectCurrentSlotMapping;

    // PRIVATE STATE **********************************************************

    private Vector3 GameObjectOffset = new Vector3(-120, -50, 0);

    // EVENT HANDLERS *********************************************************

    public void SetEvents(GameObject obj)
    {
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnMove(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
        AddEvent(obj, EventTriggerType.Select, delegate { TooltipEvent(obj); });
    }

    // PUBLIC METHODS *********************************************************

      public bool IsEmptySlot(InventorySlot obj)
    {
        return (obj != null 
        && PlayerInventoryHandler.inventory.AnyEmptySlot(obj.item) != null);
    }
    public bool IsSlotAvailable(InventorySlot obj)
    {
        return (obj != null 
        && PlayerInventoryHandler.inventory.FirstEmptySlot(obj.item) != null);
    }

    // PRIVATE METHODS ********************************************************
    
    protected void OnSlotUpdate(InventorySlot _slot)
    {
        UpdateSlotOnInterface( _slot);
    }

    protected void UpdateSlotOnInterface(InventorySlot slot)
    {
        if (slot.item.Id >= 0) UpdateSlot(slot);
        else DisableSlot(slot);
    }

    protected void UpdateSlot(InventorySlot slot)
    {
        if (slot.slotOnInterface) 
        {
            var slotImage = slot.slotOnInterface.transform.GetChild(0).GetComponentInChildren<Image>();
            var itemDatabaseReference = PlayerInventoryHandler.inventory.database.GetItem[slot.ID];
            slotImage.sprite = itemDatabaseReference.UIDisplay;
            slotImage.color = new Color(1, 1, 1, 1);
            var itemQuantity = slot.amount == -1 ? "" : slot.amount.ToString("n0");
            slot.slotOnInterface.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = itemQuantity;
        }
    }

    protected void DisableSlot(InventorySlot slot)
    {
        var slotImage = slot.slotOnInterface.transform.GetChild(0).GetComponentInChildren<Image>();
        slotImage.sprite = null;
        slotImage.color = new Color(1, 1, 1, 0);            
        slot.slotOnInterface.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    protected void UpdateAllSlotsOnInterface()
    {
        foreach(KeyValuePair<GameObject,InventorySlot> slot in ObjectCurrentSlotMapping){
            UpdateSlotOnInterface(slot.Value);
        }
    }

    protected GameObject createCursorItem(GameObject obj)
    {
        var cursorObject = new GameObject();
        var rt = cursorObject.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(50, 50);
        cursorObject.transform.SetParent(transform.parent);
            
        if (ObjectCurrentSlotMapping[obj].ID >= 0)
        {
            var img = cursorObject.AddComponent<Image>();
            img.sprite = PlayerInventoryHandler.inventory.database.GetItem[ObjectCurrentSlotMapping[obj].ID].UIDisplay;
            img.raycastTarget = false;
        }

        return cursorObject;
    }

    protected void SetCursorItem(GameObject cursorItem, GameObject slotToMove)
    {
        CursorData.tempItemDrag = cursorItem;
        CursorData.cursorInventorySlot = ObjectCurrentSlotMapping[slotToMove];
        CursorData.tempItemDrag.GetComponent<RectTransform>().position = EventSystem.current.currentSelectedGameObject.transform.position;
    }

    protected void DestroyItem(GameObject obj)
    {
        TurnOffSubMenu();

       // if (IsSameInstance(obj))
       // {
            ObjectCurrentSlotMapping[obj].resetSlot();
            soundPlayer.Play("Drop");
            ResetCursorItem();
            SetToDefaultSlot();
       // }
    }

    protected void grabHoveredItem(GameObject obj)
    {
        TurnOffSubMenu();
        SetToPreviousSlot();
        ResetCursorItem();
        var cursorObject = createCursorItem(obj);            
        SetCursorItem(cursorObject,obj);
    }

    protected void OnEnter(GameObject obj)
    {
        CursorData.slotHoveredOver = obj;

        if (ObjectCurrentSlotMapping.ContainsKey(obj))
            CursorData.inventorySlotHoveredOver = ObjectCurrentSlotMapping[obj];

        if (ObjectCurrentSlotMapping[obj].ID >= 0)            
            CursorData.position = obj.transform.position + GameObjectOffset;
    }

    protected abstract void OnDragStart(GameObject gameObject);

    protected void OnMove(GameObject obj)
    {
        if (CursorData.tempItemDrag != null)
            CursorData.tempItemDrag.GetComponent<RectTransform>().position = EventSystem.current.currentSelectedGameObject.transform.position;
    }

    protected void TooltipEvent(GameObject obj)
    {     
        StartCoroutine(ShowTooltipForObject(obj));    
    }

    // TODO fix this/tooltipAnimator
    protected  IEnumerator ShowTooltipForObject(GameObject obj)
    {
        yield return new WaitForSeconds(2f);
   
        if (CursorData.inventorySlotHoveredOver== ObjectCurrentSlotMapping[obj])
        {                
            CursorData.toolTipSlotToShow = CursorData.inventorySlotHoveredOver;
            tooltipShown=true;

            if (tooltip != null) {
                //TODO Offset position
                tooltip.transform.position = CursorData.position;
                    
                tooltip.tooltipAnimator.SetTrigger("Show");
                tooltip.ShowTooltip();
            }
        }
    }

    }
}
