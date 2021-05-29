using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class DynamicInterface : UserInterface
    {
        public GameObject inventoryPrefab;
        public int xSpaceBetween;
        public int ySpaceBetween;
        public int numberColumns;
        public int xStart;
        public int yStart;

        private void Awake()
        {
            if (inventory)
            {
                for (int i = 0; i < inventory.Container.slots.Length; i++)
                {
                    inventory.Container.slots[i].parent = this;
                    inventory.getSlot[i].OnAfterUpdate += OnSlotUpdate;
                }
            }
        }

        public new void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(objectDefault);
            UpdateSlots();
        }

        private Vector3 GetPosition(int i)
        {
            return new Vector3(xStart + xSpaceBetween * (i % numberColumns), yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
        }

        public override void CreateSlots()
        {
            gameobjectToInventorySlot = new Dictionary<GameObject, InventorySlot>();

            for (int i = 0; i < inventory.Container.slots.Length; i++)
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                gameobjectToInventorySlot.Add(obj, inventory.Container.slots[i]);
                inventory.getSlot[i].slotDisplay = obj;
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnDrag(obj); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnClick(); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnExit(obj); });
                AddEvent(obj, EventTriggerType.Deselect, delegate { OnExit2(); });


                if (i == 0)
                {
                    EventSystem.current.SetSelectedGameObject(obj);
                    objectDefault = obj;
                }
            }

            int j = 0;
            foreach (Transform item in EquipInventory.transform)
            {
                var obj = item.gameObject;
                gameobjectToInventorySlot.Add(obj, eqinventory.Container.slots[j]);
                j++;
            }
            UpdateSlots();
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
                AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, delegate { UseItem(lastObject); });
                AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Select, delegate { OnClick(); });

                AddEvent(MenuInventory.transform.GetChild(1).gameObject, EventTriggerType.Submit, delegate { MoveItem(lastObject); });
                AddEvent(MenuInventory.transform.GetChild(1).gameObject, EventTriggerType.Select, delegate { OnClick(); });

                if (MenuInventory.transform.GetChild(2).gameObject != null)
                {
                    AddEvent(MenuInventory.transform.GetChild(2).gameObject, EventTriggerType.Submit, delegate { DestroyItem(lastObject); });
                    AddEvent(MenuInventory.transform.GetChild(2).gameObject, EventTriggerType.Select, delegate { OnClick(); });
                }
                soundPlayer.Play("EquipItem");
                onUse = MenuInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

                if (gameobjectToInventorySlot[lastObject].item.type == ItemType.Weapon)
                {
                    onUse.text = "Equip";
                }

                else
                {
                    onUse.text = "Use";
                }



                Debug.Log("not sure click sound");
                EventSystem.current.SetSelectedGameObject(MenuInventory.transform.GetChild(0).gameObject);
                return;
            }

            if (CursorData.slotHoveredOn)
            {
                if (CursorData.item.ID == -1)
                {
                    inventory.MoveItem(CursorData.item, CursorData.hoverItem.parent.gameobjectToInventorySlot[CursorData.slotHoveredOn]);
                    Destroy(CursorData.tempItemDrag);
                    CursorData.item = null;
                    soundPlayer.Play("Drop");
                }
                else if (CursorData.hoverItem.CanPlace(inventory.database.GetItem[CursorData.item.ID]) && (CursorData.hoverItem.ID >= -1 || (CursorData.hoverItem.ID <= 0 && CursorData.hoverItem.CanPlace(inventory.database.GetItem[CursorData.hoverItem.item.Id]))))
                {
                    inventory.MoveItem(CursorData.item, CursorData.hoverItem.parent.gameobjectToInventorySlot[CursorData.slotHoveredOn]);
                    Destroy(CursorData.tempItemDrag);
                    CursorData.item = null;
                    soundPlayer.Play("Drop");
                }

                else
                {
                    Debug.Log("this");
                }
                return;
            }
        }
    }
}