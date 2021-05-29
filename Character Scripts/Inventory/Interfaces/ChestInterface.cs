using Sounds;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class ChestInterface : UserInterface
    {
        private GameObject PlayerObject;
        public GameObject inventoryPrefab;
        public changescene changescene;
        public int xSpaceBetween;
        public int ySpaceBetween;
        public int numberColumns;
        public int xStart;
        public int yStart;
        public bool walk1;
        public bool walk2;
        public bool anim;


        private void Awake()
        {
            PlayerObject = GameObject.FindGameObjectWithTag("Player");
            anim = PlayerObject.transform.GetChild(0).GetComponentInChildren<Animator>().enabled;
            soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
            playerStats =PlayerObject.GetComponent<PlayerStats>();
            eqinventory =PlayerObject.GetComponent<InventoryPlayer>().inventory;
            changescene = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<changescene>();

            AddEvent(MenuInventory.transform.GetChild(0).gameObject, EventTriggerType.Submit, delegate { TakeItem(lastObject); });
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

        public new void OnEnable()
        {        
            EventSystem.current.SetSelectedGameObject(objectDefault);
            foreach (var item in gameobjectToInventorySlot)
            {
                Debug.Log(item.Key.GetInstanceID() + "chest ITEMS SLOT" + item.Value.item + "_" + item.Value.ID);
            }
        }
        
        private Vector3 GetPosition(int i)
        {
            return new Vector3(xStart + xSpaceBetween * (i % numberColumns), yStart + (-ySpaceBetween * (i / numberColumns)), 0f);
        }

        public override void CreateSlots()
        {
            Debug.Log("check this");
                       
            for (int i = 0; i < inventory.Container.slots.Length; i++)
            {
                var obj = Instantiate(inventoryPrefab, Vector3.zero, Quaternion.identity, transform);
                obj.GetComponent<RectTransform>().localPosition = GetPosition(i);
                gameobjectToInventorySlot.Add(obj, inventory.Container.slots[i]);
                // Debug.Log(inventory.Container.items[i]);
                inventory.getSlot[i].slotDisplay = obj;
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnEnter(obj); });
                AddEvent(obj, EventTriggerType.Submit, delegate { OnDragStart(obj); });
                AddEvent(obj, EventTriggerType.UpdateSelected, delegate { OnDrag(obj); });
                AddEvent(obj, EventTriggerType.Select, delegate { OnClick(); });

                if (i == 0)
                {
                    EventSystem.current.SetSelectedGameObject(obj);
                    objectDefault = obj;
                }
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

                EventSystem.current.SetSelectedGameObject(MenuInventory.transform.GetChild(0).gameObject);
                onUse = MenuInventory.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();

                if (gameobjectToInventorySlot[lastObject].item.type == ItemType.Weapon)
                {
                    onUse.text = "Take";
                }

                else
                {
                    onUse.text = "..";
                }


                return;
            }
            /*
            if (CursorData.slotHoveredOn)
            {
                if (CursorData.item.ID == -1)
                {
                    inventory.MoveItem(CursorData.item, CursorData.hoverItem.parent.gameobjectToInventorySlot[CursorData.slotHoveredOn]);
                   // gameobjectToInventorySlot.Remove(CursorData.slotHoveredOn);
                   // gameobjectToInventorySlot.Add(CursorData.slotHoveredOn, gameobjectToInventorySlot[EquipInventory.transform.GetChild(3).gameObject]);
                    Destroy(CursorData.tempItemDrag);
                    CursorData.item = null;
                }
                else if (CursorData.hoverItem.CanPlace(inventory.database.GetItem[CursorData.item.ID]) && (CursorData.hoverItem.ID <= -1 || (CursorData.hoverItem.ID <= 0 && CursorData.hoverItem.CanPlace(inventory.database.GetItem[CursorData.hoverItem.item.Id]))))
                {
                    inventory.MoveItem(CursorData.item, CursorData.hoverItem.parent.gameobjectToInventorySlot[CursorData.slotHoveredOn]);
                    
                    Destroy(CursorData.tempItemDrag);
                    CursorData.item = null;
                }*/
            return;
        }
            
    }
}