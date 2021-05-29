using Sounds;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class ShopInterface : UserInterface
    {
        GameObject PlayerObject;
        public GameObject inventoryPrefab;
        public changescene changescene;
        public int xSpaceBetween;
        public int ySpaceBetween;
        public int numberColumns;
        public int xStart;
        public int yStart;

        private void Awake()
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
            soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
            playerStats =PlayerObject.GetComponent<PlayerStats>();
            eqinventory =PlayerObject.GetComponent<InventoryPlayer>().inventory;
            changescene = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<changescene>();
            AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, delegate { BuyItem(lastObject); });
            AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Select, delegate { OnClick(); });
            gameobjectToInventorySlot = new System.Collections.Generic.Dictionary<GameObject, InventorySlot>();

        }
        private IEnumerator Wait()
        {
            {
                yield return null;
            }
        }
        public new void OnEnable()
        {
            this.GetComponentInParent<PixelCrushers.DialogueSystem.DialogueSystemTrigger>().enabled = false;
            EventSystem.current.SetSelectedGameObject(objectDefault);
            UpdateSlots();
        }
        private new void OnDisable()
        {
            this.GetComponentInParent<PixelCrushers.DialogueSystem.DialogueSystemTrigger>().enabled = true;
        }
        private Vector3 GetPosition(int i)
        {
            return new Vector3(xStart + xSpaceBetween * (i % numberColumns), yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
        }
        public override void CreateSlots()
        {
           // gameobjectToInventorySlot = new Dictionary<GameObject, InventorySlot>();
            for (int i = 0; i < inventory.Container.slots.Length; i++)
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                gameobjectToInventorySlot.Add(obj, inventory.Container.slots[i]);
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnDrag(obj); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnClick(); });
                if (i == 0)
                {
                    EventSystem.current.SetSelectedGameObject(obj);
                    objectDefault = obj;
                }
                UpdateSlots();
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
               
                if (gameobjectToInventorySlot[lastObject].item.type == ItemType.Weapon)
                {                  
                    onUse.text = "Buy";
                }

                else
                {                   
                    onUse.text = "fix";
                }
               
                EventSystem.current.SetSelectedGameObject(MenuInventory.transform.GetChild(0).gameObject);
              //  return;
            }
            return;
           /* if (CursorData.slotHoveredOn)
            {
                if (CursorData.item.ID == -1)
                {
                    inventory.MoveItem(CursorData.item, CursorData.hoverItem.parent.gameobjectToInventorySlot[CursorData.slotHoveredOn]);
                    Destroy(CursorData.tempItemDrag);
                    CursorData.item = null;
                }

                else if (CursorData.hoverItem.CanPlace(inventory.database.GetItem[CursorData.item.ID]) && (CursorData.hoverItem.ID <= -1 || (CursorData.hoverItem.ID <= 0 && CursorData.hoverItem.CanPlace(inventory.database.GetItem[CursorData.hoverItem.item.Id]))))
                {
                    inventory.MoveItem(CursorData.item, CursorData.hoverItem.parent.gameobjectToInventorySlot[CursorData.slotHoveredOn]);
                    Destroy(CursorData.tempItemDrag);
                    CursorData.item = null;
                }

                return;
            }*/
        }
        private void OnApplicationQuit()
        {
            inventory.Container.slots= new InventorySlot[4];
        }
    }   
}
