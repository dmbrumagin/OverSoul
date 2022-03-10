

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;
using InventoryRelated;

namespace UI
{
    public class InventoryDisplay : ItemDisplay
    {

    // PUBLIC SETTINGS ********************************************************

    public GameObject inventoryPrefab;
    public Image weaponImage;
    public int xSpaceBetween;
    public int ySpaceBetween;
    public int numberColumns;
    public int xStart;
    public int yStart;

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

    private void OnEnable()
    {         
        SetToDefaultSlot();
    }

    // EVENT HANDLERS *********************************************************
    
    private new void SetEvents(GameObject obj)
    {
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnMove(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
        AddEvent(obj, EventTriggerType.Deselect, delegate { OnExit(obj); }); 
        AddEvent(obj, EventTriggerType.Select, delegate { TooltipEvent(obj); });
    }

    // PUBLIC METHODS *********************************************************

    public void updateCharWeaponSprite(bool setNewSprite)
    {
        if(setNewSprite)
        {
            if (ObjectCurrentSlotMapping[previouslySelectedObject].item.type == ItemType.Weapon)
            {
                weaponImage.sprite = PlayerInventoryHandler.inventory.database.GetItem[ObjectCurrentSlotMapping[previouslySelectedObject].ID].UIDisplay;
                 weaponImage.color = new Color(1, 1, 1, 1);
            }
        }

        else
        {
            weaponImage.sprite = null;
            weaponImage.color = new Color(1, 1, 1, 0);                
        }
    }

    // PRIVATE METHODS ********************************************************
             
    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + xSpaceBetween * (i % numberColumns), 
        yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
    }

    private void CreateSlots()
    {
        int i=0;
        foreach (InventorySlot slot in PlayerInventoryHandler.inventory.Container.slots)
        {
            var obj = createSlotObject(slot);
                
            if (i == 0)
            {
                CreateDefaultSlotReference(obj);
            }   

            obj.GetComponent<RectTransform>().localPosition = GetPosition(i++);
            SetEvents(obj);                 
        }
                              
    }

    private GameObject createSlotObject(InventorySlot slot)
    {
        var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            
        ObjectCurrentSlotMapping.Add(obj, slot);              
        slot.slotOnInterface = obj;
        slot.OnAfterUpdate += OnSlotUpdate;

        return obj;
    }    

    private void setSubMenuEvents()
    {
        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, delegate { PlayerInventoryHandler.UseItem(ObjectCurrentSlotMapping[previouslySelectedObject]); });
        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
        AddEvent(MenuInventory.transform.GetChild(1).gameObject, EventTriggerType.Submit, delegate { grabHoveredItem(previouslySelectedObject); });
        AddEvent(MenuInventory.transform.GetChild(1).gameObject, EventTriggerType.Select, delegate { PlayMenuClickSound(); });

        if (MenuInventory.transform.GetChild(2))
        {
            AddEvent(MenuInventory.transform.GetChild(2).gameObject, EventTriggerType.Submit, delegate { DestroyItem(previouslySelectedObject); });
            AddEvent(MenuInventory.transform.GetChild(2).gameObject, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
        }
    }

    private bool isHoldingAndValidSlot()
    {
        return CursorData.slotHoveredOver && CursorData.cursorInventorySlot!=null;
    }
    protected override void OnDragStart(GameObject obj)
    {
        if (CursorData.cursorInventorySlot == null)
        {
            previouslySelectedObject = obj;
            TurnOnSubMenu();
            setSubMenuEvents();
            soundPlayer.Play("EquipItem");
            var buttonText = MenuInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

            if (PlayerInventoryHandler.IsEquipable(ObjectCurrentSlotMapping[previouslySelectedObject]))
                buttonText.text = "Equip";
                
            else
            {
                buttonText.text = "Use";
            }
                
            SetToFirstSubMenuChild();
        }

        if (isHoldingAndValidSlot())
        {
            var slotToMoveTo = ObjectCurrentSlotMapping[CursorData.slotHoveredOver];
            PlayerInventoryHandler.inventory.MoveItem(CursorData.cursorInventorySlot, slotToMoveTo);
            ResetCursorItem();
            soundPlayer.Play("Drop");                
        }
    }
    
    }
}