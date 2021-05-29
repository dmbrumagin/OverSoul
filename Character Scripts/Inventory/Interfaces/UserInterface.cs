
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sounds;
using System;
using System.Collections;

namespace Player
{

    public abstract class UserInterface : MonoBehaviour
    {
        public SoundPlayer soundPlayer;
        public GameObject objectDefault;
        public GameObject lastObject;
        [SerializeField]
        public PlayerStats playerStats;
        //needs SETUP    
        public Image weaponImage;
        public TextMeshProUGUI onUse;
        //public Image abilityIcon;
        // public TextMeshProUGUI abilityDescription;          
        // public TextMeshProUGUI abilityAttribute;
        // public TextMeshProUGUI abilityAttrAmount;
        public InventoryObject inventory;
        public InventoryObject eqinventory;
        public GameObject EquipInventory;
        public GameObject MenuInventory;
        public Tooltip tooltip;
        public Dictionary<GameObject, InventorySlot> gameobjectToInventorySlot = new Dictionary<GameObject, InventorySlot>();
        public Dictionary<GameObject, AbilityInventorySlot> abilitiesOnInterface = new Dictionary<GameObject, AbilityInventorySlot>();
        Vector3 offset = new Vector3(-120, -50, 0);
        Vector3 offset2 = new Vector3(120, -50, 0);

        private void Awake()
        {

            playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
            if (inventory)
            {
                for (int i = 0; i < inventory.Container.slots.Length; i++)
                {
                    inventory.Container.slots[i].parent = this;
                    inventory.getSlot[i].OnAfterUpdate += OnSlotUpdate;
                }
            }

            //playerStats.onLVChange += ReactToChange;

        }

        void Start()
        {

            soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();



            CreateSlots();

        }

        public void OnSlotUpdate(InventorySlot _slot)
        {
            if (_slot.item.Id >= 0)
            {
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.ID].UIDisplay;
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                _slot.slotDisplay.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.amount == -1 ? "" : _slot.amount.ToString("n0");
            }

            else
            {
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                _slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                _slot.slotDisplay.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }

        void Update()
        {

            //UpdateSlots();
            if (MenuInventory.activeInHierarchy == true && Input.GetButtonDown("Cancel"))
            {
                MenuInventory.SetActive(false);
                EventSystem.current.SetSelectedGameObject(lastObject);
            }
        }

        public void OnEnable()
        {
            UpdateSlots();

        }

        public void OnDisable()
        {
            if (MenuInventory.activeInHierarchy == true)
            {
                MenuInventory.SetActive(!MenuInventory.activeSelf);
            }
            Destroy(CursorData.tempItemDrag);
            CursorData.item = null;
            // UpdateSlots();
        }

        public abstract void CreateSlots();

        public void UpdateSlots()
        {

            foreach (KeyValuePair<GameObject, InventorySlot> _slot in gameobjectToInventorySlot)
            {
                if (_slot.Value.ID >= 0)
                {
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = inventory.database.GetItem[_slot.Value.ID].UIDisplay;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
                    _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = _slot.Value.amount == -1 ? "" : _slot.Value.amount.ToString("n0");
                }

                else
                {
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
                    _slot.Key.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
                    _slot.Key.transform.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = "";
                }
            }

        }

        public void BuyItem(GameObject obj)
        {
            if (MenuInventory.activeInHierarchy == true)
            {
                MenuInventory.SetActive(!MenuInventory.activeSelf);
            }

            // Debug.Log(gameobjectToInventorySlot[obj]+"try"+ eqinventory.AnyEmptySlot(gameobjectToInventorySlot[obj].item));
            if (gameobjectToInventorySlot[obj] != null && eqinventory.AnyEmptySlot(gameobjectToInventorySlot[obj].item) != null)
            {
                Debug.Log("buy try");
                if (playerStats.attributesList[6].amount >= gameobjectToInventorySlot[obj].item.price)
                {
                    playerStats.attributesList[6].amount = playerStats.attributesList[6].amount - gameobjectToInventorySlot[obj].item.price;
                    Debug.Log("buy");
                    eqinventory.AddItem(gameobjectToInventorySlot[obj].item, gameobjectToInventorySlot[obj].amount);
                    soundPlayer.Play("UnequipItem");
                    EventSystem.current.SetSelectedGameObject(lastObject);
                    return;
                }
                return;
            }
            return;
        }

        public void TakeItem(GameObject obj)
        {
            if (MenuInventory.activeInHierarchy == true)
            {
                MenuInventory.SetActive(!MenuInventory.activeSelf);
            }

            if ((gameobjectToInventorySlot[obj] != null && eqinventory.AnyEmptySlot(gameobjectToInventorySlot[obj].item) != null))
            {
                Debug.Log(obj.GetInstanceID() + "_" + lastObject.GetInstanceID());
                if (obj.GetInstanceID() == lastObject.GetInstanceID())
                {
                    eqinventory.AddItem(gameobjectToInventorySlot[obj].item, gameobjectToInventorySlot[obj].amount);
                    inventory.RemoveItem(gameobjectToInventorySlot[obj]);
                    soundPlayer.Play("UnequipItem");
                    EventSystem.current.SetSelectedGameObject(lastObject);
                    return;
                }
            }
            return;
        }

        public void UseItem(GameObject obj)
        {
            if (gameobjectToInventorySlot[lastObject].item.type == ItemType.Hat || gameobjectToInventorySlot[lastObject].item.type == ItemType.Weapon || gameobjectToInventorySlot[lastObject].item.type == ItemType.Tunic || gameobjectToInventorySlot[lastObject].item.type == ItemType.Soul)
            {
                //FIXXXXX


                if (MenuInventory.activeInHierarchy == true)
                {
                    MenuInventory.SetActive(!MenuInventory.activeSelf);
                }

                if (gameobjectToInventorySlot[EquipInventory.transform.GetChild(0).gameObject].CanPlace(inventory.database.GetItem[gameobjectToInventorySlot[lastObject].ID]))
                {
                    for (int i = 0; i < playerStats.attributesList.Count; i++)
                    {
                        for (int j = 0; j < gameobjectToInventorySlot[lastObject].item.buffs.Length; j++)
                        {
                            if (playerStats.attributesList[i].attributes.name == gameobjectToInventorySlot[lastObject].item.buffs[j].attribute.name)
                            {
                                playerStats.attributesList[i].amount += gameobjectToInventorySlot[lastObject].item.buffs[j].value;
                            }
                        }
                    }

                    weaponImage.sprite = inventory.database.GetItem[gameobjectToInventorySlot[lastObject].ID].UIDisplay;
                    weaponImage.color = new Color(1, 1, 1, 1);
                    inventory.MoveItem(gameobjectToInventorySlot[lastObject], gameobjectToInventorySlot[EquipInventory.transform.GetChild(0).gameObject]);
                    // gameobjectToInventorySlot.Remove(obj);
                    // gameobjectToInventorySlot.Add(obj, gameobjectToInventorySlot[EquipInventory.transform.GetChild(0).gameObject]);
                    EventSystem.current.SetSelectedGameObject(lastObject);
                    soundPlayer.Play("EquipItem");
                    return;
                }
                if (gameobjectToInventorySlot[EquipInventory.transform.GetChild(1).gameObject].CanPlace(inventory.database.GetItem[gameobjectToInventorySlot[lastObject].ID]))
                {
                    for (int i = 0; i < playerStats.attributesList.Count; i++)
                    {
                        for (int j = 0; j < gameobjectToInventorySlot[lastObject].item.buffs.Length; j++)
                        {
                            if (playerStats.attributesList[i].attributes.name == gameobjectToInventorySlot[lastObject].item.buffs[j].attribute.name)
                            {
                                playerStats.attributesList[i].amount += gameobjectToInventorySlot[lastObject].item.buffs[j].value;
                            }
                        }
                    }
                    inventory.MoveItem(gameobjectToInventorySlot[lastObject], gameobjectToInventorySlot[EquipInventory.transform.GetChild(1).gameObject]);
                    //  gameobjectToInventorySlot.Remove(obj);
                    //  gameobjectToInventorySlot.Add(obj, gameobjectToInventorySlot[EquipInventory.transform.GetChild(1).gameObject]);
                    EventSystem.current.SetSelectedGameObject(lastObject);
                    soundPlayer.Play("EquipItem");
                    return;
                }
                if (gameobjectToInventorySlot[EquipInventory.transform.GetChild(2).gameObject].CanPlace(inventory.database.GetItem[gameobjectToInventorySlot[lastObject].ID]))
                {
                    for (int i = 0; i < playerStats.attributesList.Count; i++)
                    {
                        for (int j = 0; j < gameobjectToInventorySlot[lastObject].item.buffs.Length; j++)
                        {
                            if (playerStats.attributesList[i].attributes.name == gameobjectToInventorySlot[lastObject].item.buffs[j].attribute.name)
                            {
                                playerStats.attributesList[i].amount += gameobjectToInventorySlot[lastObject].item.buffs[j].value;
                            }
                        }
                    }
                    inventory.MoveItem(gameobjectToInventorySlot[lastObject], gameobjectToInventorySlot[EquipInventory.transform.GetChild(2).gameObject]);
                    // gameobjectToInventorySlot.Remove(obj);
                    // gameobjectToInventorySlot.Add(obj, gameobjectToInventorySlot[EquipInventory.transform.GetChild(2).gameObject]);
                    EventSystem.current.SetSelectedGameObject(lastObject);
                    soundPlayer.Play("EquipItem");
                    return;
                }
                if (gameobjectToInventorySlot[EquipInventory.transform.GetChild(3).gameObject].CanPlace(inventory.database.GetItem[gameobjectToInventorySlot[lastObject].ID]))
                {
                    for (int i = 0; i < playerStats.attributesList.Count; i++)
                    {
                        for (int j = 0; j < gameobjectToInventorySlot[lastObject].item.buffs.Length; j++)
                        {
                            if (playerStats.attributesList[i].attributes.name == gameobjectToInventorySlot[lastObject].item.buffs[j].attribute.name)
                            {
                                playerStats.attributesList[i].amount += gameobjectToInventorySlot[lastObject].item.buffs[j].value;
                            }
                        }
                    }
                    inventory.MoveItem(gameobjectToInventorySlot[lastObject], gameobjectToInventorySlot[EquipInventory.transform.GetChild(3).gameObject]);

                    //  gameobjectToInventorySlot.Remove(obj);
                    //  gameobjectToInventorySlot.Add(obj, gameobjectToInventorySlot[EquipInventory.transform.GetChild(3).gameObject]);
                    EventSystem.current.SetSelectedGameObject(lastObject);
                    soundPlayer.Play("EquipItem");
                    return;
                }
                return;
            }

            else
            {
                Debug.Log("null");
            }
        }



        public void UnEquipItem(GameObject obj)
        {
            if (gameobjectToInventorySlot[lastObject].item.type == ItemType.Hat || gameobjectToInventorySlot[lastObject].item.type == ItemType.Weapon || gameobjectToInventorySlot[lastObject].item.type == ItemType.Tunic || gameobjectToInventorySlot[lastObject].item.type == ItemType.Soul)
            {
                //FIXXXXX


                if (MenuInventory.activeInHierarchy == true)
                {
                    MenuInventory.SetActive(!MenuInventory.activeSelf);
                }

                if (gameobjectToInventorySlot[lastObject].item.type == ItemType.Weapon)
                {
                    weaponImage.sprite = null;
                    weaponImage.color = new Color(1, 1, 1, 0);
                }

                if (gameobjectToInventorySlot[obj] != null && eqinventory.FirstEmptySlot(gameobjectToInventorySlot[obj].item) != null)
                {
                    for (int i = 0; i < playerStats.attributesList.Count; i++)
                    {
                        for (int j = 0; j < gameobjectToInventorySlot[obj].item.buffs.Length; j++)
                        {
                            if (playerStats.attributesList[i].attributes.name == gameobjectToInventorySlot[obj].item.buffs[j].attribute.name)
                            {
                                Debug.Log(gameobjectToInventorySlot[obj].item.buffs[j].attribute.name + "_" + gameobjectToInventorySlot[obj].item.buffs[j].value);
                                playerStats.attributesList[i].amount -= gameobjectToInventorySlot[obj].item.buffs[j].value;
                            }
                        }
                    }
                    inventory.MoveItem(gameobjectToInventorySlot[obj], eqinventory.FirstEmptySlot(gameobjectToInventorySlot[obj].item));

                    // gameobjectToInventorySlot.Remove(obj);
                    // gameobjectToInventorySlot.Add(obj, eqinventory.FirstEmptySlot(gameobjectToInventorySlot[obj].item));
                    soundPlayer.Play("UnequipItem");
                    EventSystem.current.SetSelectedGameObject(lastObject);
                }

                return;
            }
            else
            {
                Debug.Log("null");
            }



        }

        public void MoveItem(GameObject obj)
        {
            if (MenuInventory.activeInHierarchy == true)
            {
                MenuInventory.SetActive(!MenuInventory.activeSelf);
            }

            EventSystem.current.SetSelectedGameObject(lastObject);
            var mouseObject = new GameObject();
            var rt = mouseObject.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(50, 50);
            mouseObject.transform.SetParent(transform.parent);

            if (gameobjectToInventorySlot[obj].ID >= 0)
            {
                var img = mouseObject.AddComponent<Image>();
                img.sprite = inventory.database.GetItem[gameobjectToInventorySlot[obj].ID].UIDisplay;
                img.raycastTarget = false;
            }

            Destroy(CursorData.tempItemDrag);
            CursorData.item = null;
            CursorData.tempItemDrag = mouseObject;
            CursorData.item = gameobjectToInventorySlot[obj];
            CursorData.tempItemDrag.GetComponent<RectTransform>().position = EventSystem.current.currentSelectedGameObject.transform.position;
        }

       

        public void DestroyItem(GameObject obj)
        {
            if (lastObject.GetInstanceID() == obj.GetInstanceID())
            {
                if (MenuInventory.activeInHierarchy == true)
                {
                    MenuInventory.SetActive(!MenuInventory.activeSelf);
                }
                gameobjectToInventorySlot[obj].UpdateSlot(-1, null, 0);
                soundPlayer.Play("Drop");
                EventSystem.current.SetSelectedGameObject(objectDefault);
                CursorData.item = null;
                return;
            }
        }

        public void OnClick()
        {            
            soundPlayer.Play("EquipItem");
          //  tooltip.ShowTooltip();
        }
        public void OnEnter()
        {
          // tooltip.HideTooltip();
        }



        protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
        {
            EventTrigger trigger = obj.GetComponent<EventTrigger>();
            var eventTrigger = new EventTrigger.Entry();
            eventTrigger.eventID = type;
            eventTrigger.callback.AddListener(action);
            trigger.triggers.Add(eventTrigger);
        }

        public void OnEnter(GameObject obj)
        {
            CursorData.slotHoveredOn = obj;
            
            if (gameobjectToInventorySlot.ContainsKey(obj))
            {
                CursorData.hoverItem = gameobjectToInventorySlot[obj];
               
                    
                
            }

        }
        public void OnExit(GameObject obj)
        {
            // if (gameobjectToInventorySlot.ContainsKey(obj))
            // {
            if (gameobjectToInventorySlot[obj].ID>=0)
            {
                

                StartCoroutine(countTooltipShow(obj));
                    tooltip.enabled = true;
                    CursorData.position = obj.transform.position + offset;
                    CursorData.tooltip = gameobjectToInventorySlot[obj];
                    tooltip.transform.position = CursorData.position;
                    tooltip.ShowTooltip();
                    //  }
                    // else
                    // {
                    //   Debug.Log(CursorData.hoverItem.item.Name);
                    // Debug.Log(CursorData.item.item.Name);
                    Debug.Log(CursorData.tooltip.item.Name);
                    //Debug.Log(CursorData.slotHoveredOn.);
                    //}}
                
            }
            else
            {
                tooltip.enabled = false;
                tooltip.HideTooltip();
            }
        }

        public void OnExit2()
        {
            // if (gameobjectToInventorySlot.ContainsKey(obj))
            // {
            tooltip.enabled = false;
            tooltip.HideTooltip();
            //  tooltip.HideTooltip();

            //  }
            // else
            // {
            //   Debug.Log(CursorData.hoverItem.item.Name);
            // Debug.Log(CursorData.item.item.Name);
            //Debug.Log(CursorData.slotHoveredOn.);
            //}
        }
        public void OnExit3(GameObject obj)
        {
            if (abilitiesOnInterface[obj].ID >= 0)
            {

                StartCoroutine(countTooltipShow2(obj));
                    tooltip.enabled = true;
                    CursorData.position = obj.transform.position + offset2;
                    CursorData.tooltip2 = abilitiesOnInterface[obj];
                    tooltip.transform.position = CursorData.position;
                    tooltip.showToolTip2();
                    //  }
                    // else
                    // {
                    //   Debug.Log(CursorData.hoverItem.item.Name);
                    // Debug.Log(CursorData.item.item.Name);
                    Debug.Log(CursorData.tooltip.item.Name);
                
            }
            else
            {
                tooltip.enabled = false;
                tooltip.HideTooltip();
            }
        }

        public void OnEnter2(GameObject obj)
        {
            CursorData.slotHoveredOn = obj;

            if (abilitiesOnInterface.ContainsKey(obj))
            {
                CursorData.hoverItem2 = abilitiesOnInterface[obj];
                CursorData.hoverAbility = abilitiesOnInterface[obj].ability;
            }

        }

        public abstract void OnDragStart(GameObject obj);

        public void OnDrag(GameObject obj)
        {
            //tooltip.ShowTooltip();
            if (CursorData.tempItemDrag != null)
            {
                CursorData.tempItemDrag.GetComponent<RectTransform>().position = EventSystem.current.currentSelectedGameObject.transform.position;
            }
        }

        public  IEnumerator countTooltipShow(GameObject obj)
        {
            yield return new WaitForSeconds(1f);
            if (CursorData.hoverItem== gameobjectToInventorySlot[obj])
            {
                tooltip.tooltipAnimator.SetTrigger("Show");
                yield return null;
            }
            else
            {
                tooltip.enabled=false;
            }

        }
        public IEnumerator countTooltipShow2(GameObject obj)
        {
            yield return new WaitForSeconds(1f);
            if (CursorData.hoverItem2 == abilitiesOnInterface[obj])
            {
                tooltip.tooltipAnimator.SetTrigger("Show");
                yield return null;
            }
            else
            {
                tooltip.enabled = false;
            }
        }
        public void Break()
        {
            return;
        }

    }
    public static class CursorData
    {
        public static Vector3 position;
        public static UserInterface interfaceCursorOn;
        public static GameObject tempItemDrag;
        public static InventorySlot item;
        public static InventorySlot hoverItem;
        public static InventorySlot tooltip;
        public static AbilityInventorySlot tooltip2;
        public static AbilityInventorySlot hoverItem2;
        public static Ability hoverAbility;
        public static GameObject slotHoveredOn;

        
    }
   
}