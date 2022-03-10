

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UI;
using InventoryRelated;

namespace Player
{
    public class ShopDisplay : ItemDisplay
    {

    // PUBLIC SETTINGS ********************************************************

    public GameObject slotPrefab;
    public int xSpaceBetween;
    public int ySpaceBetween;
    public int numberColumns;
    public int xStart;
    public int yStart;
    
    
    // UNITY METHODS **********************************************************

    private void Awake()
    {
        ObjectCurrentSlotMapping = new Dictionary<GameObject, InventorySlot>();
    }

    private void Start()
    {
        InitializeReferences();
        createSubMenuEvents();
        CreateSlots();
    }

    private void OnEnable()
    {
        SetToDefaultSlot();
        UpdateAllSlotsOnInterface();
    }


    // EVENT HANDLERS *********************************************************

    private void createSlotEvents(GameObject obj)
    {
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
        AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
        AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnMove(obj); });
        AddEvent(obj, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
    }

    private void createSubMenuEvents()
    {
        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, delegate { PlayerInventoryHandler.BuyItem(ObjectCurrentSlotMapping[previouslySelectedObject],this); });
        AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Select, delegate { PlayMenuClickSound(); });
    }


    // PRIVATE METHODS ********************************************************
        
    protected override void InitializeReferences()
    {
        base.InitializeReferences();
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + xSpaceBetween * (i % numberColumns), yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
    }

    private void CreateSlots()
    {
        var inventory = this.gameObject.GetComponentInParent<ShopInventoryHandler>().inventory;
        for (int i = 0; i < inventory.Container.slots.Length; i++)
        {
            var obj = Instantiate(slotPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
            ObjectCurrentSlotMapping.Add(obj, inventory.Container.slots[i]);
            inventory.getSlot[i].slotOnInterface = obj;
            inventory.getSlot[i].OnAfterUpdate += OnSlotUpdate;
            createSlotEvents(obj);

            if (i == 0)
                CreateDefaultSlotReference(obj);
            
            UpdateAllSlotsOnInterface();
        }
    }

    protected override void OnDragStart(GameObject obj)
    {
        if (CursorData.cursorInventorySlot == null)
        {
            previouslySelectedObject = obj;
            TurnOnSubMenu();
            var buttonText = MenuInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
            buttonText.text = "Buy";
            SetToFirstSubMenuChild();
        }
    }

    }   
}
