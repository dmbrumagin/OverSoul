

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UI;
using InventoryRelated;
using System.Collections.Generic;

namespace Player
{
    public class ChestDisplay : ItemDisplay
    {

    // PUBLIC SETTINGS ********************************************************

    public GameObject inventoryPrefab;
    public int xSpaceBetween;
    public int ySpaceBetween;
    public int numberColumns;
    public int xStart;
    public int yStart;


    // PRIVATE STATE **********************************************************
    
    private bool onDragReset;


    // UNITY METHODS **********************************************************

    private void Awake()
    {
        ObjectCurrentSlotMapping = new Dictionary<GameObject, InventorySlot>();
        onDragReset=true;
    }

    private void Start()
    {
        InitializeReferences();            
        CreateSlots();  
        SetSubMenuEvents();         
    }

    private new void OnDisable()
    {        
        TurnOffSubMenu();
        ResetCursorItem();
        onDragReset=false;        
    }


    // EVENT HANDLERS *********************************************************

    private void SetSubMenuEvents()
    {
        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, 
        delegate { PlayerInventoryHandler.TakeItem(ObjectCurrentSlotMapping[previouslySelectedObject],this,GetComponentInParent<ChestInventoryHandler>().inventory); });

        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
    }


    // PRIVATE METHODS ********************************************************      
        
    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + xSpaceBetween * (i % numberColumns), yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
    }

    private void CreateSlots()
    {
        var inventory = GetComponentInParent<ChestInventoryHandler>().inventory;
        for (int i = 0; i < inventory.Container.slots.Length; i++)
        {
            var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);                
            ObjectCurrentSlotMapping.Add(obj, inventory.Container.slots[i]);
            inventory.getSlot[i].slotOnInterface = obj;
            inventory.getSlot[i].OnAfterUpdate += OnSlotUpdate;

            SetEvents(obj);

            if (i == 0)                 
                CreateDefaultSlotReference(obj);
        }
            
        UpdateAllSlotsOnInterface();
           
    }
        
    protected override void OnDragStart(GameObject obj)
    {            
        if (CursorData.cursorInventorySlot == null && onDragReset)
        {               
            previouslySelectedObject = obj;
            TurnOnSubMenu();
            SetToFirstSubMenuChild();
            var buttonText = MenuInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            buttonText.text = "Take";
        }

        else if(!onDragReset){
            onDragReset=true;
        }
    }   

    }
}