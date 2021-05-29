using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{

    public class StaticInterface : UserInterface
    {
       
        public GameObject[] slots;
        private void Awake()
        {


            AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, delegate { UnEquipItem(lastObject); });
            AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Select, delegate { OnClick(); });
            if (inventory)
            {
                for (int i = 0; i < inventory.Container.slots.Length; i++)
                {
                    inventory.Container.slots[i].parent = this;
                    inventory.getSlot[i].OnAfterUpdate += OnSlotUpdate;
                }
            }
        }

        private void OnEnable()
        {
            UpdateSlots();
        }

        public override void CreateSlots()
        {
           // gameobjectToInventorySlot = new Dictionary<GameObject, InventorySlot>();

            int i= 0;
            foreach (Transform item in EquipInventory.transform)
            {
                var obj = item.gameObject;

                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnDrag(obj); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnClick(); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnExit(obj); });
                AddEvent(obj, EventTriggerType.Deselect, delegate { OnExit2(); });
                inventory.getSlot[i].slotDisplay = obj;
                gameobjectToInventorySlot.Add(obj, inventory.Container.slots[i]);
                i++;
               
            }
        }

        public override void OnDragStart(GameObject obj)
        {
            if (CursorData.item == null)
            {
                lastObject = obj;
                if (MenuInventory.activeInHierarchy == false)
                {
                    MenuInventory.SetActive(!MenuInventory.activeSelf);
                }
                               
                onUse = MenuInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

                if (gameobjectToInventorySlot[lastObject].item.type == ItemType.Hat || gameobjectToInventorySlot[lastObject].item.type == ItemType.Weapon || gameobjectToInventorySlot[lastObject].item.type == ItemType.Tunic || gameobjectToInventorySlot[lastObject].item.type == ItemType.Soul)
                {
                    onUse.text = "UnEquip";

                }

                else
                {
                    onUse.text = "...";
                }
           
             //   AddEvent(MenuInventory.transform.GetChild(1).gameObject, EventTriggerType.Submit, delegate { DestroyItem(obj); });
               // AddEvent(MenuInventory.transform.GetChild(1).gameObject, EventTriggerType.Select, delegate { OnClick(); });
                EventSystem.current.SetSelectedGameObject(MenuInventory.transform.GetChild(0).gameObject);
                return;
            }
        }
    }

}

