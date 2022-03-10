

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using Sounds;
using Player;

namespace UI
{
    public abstract class MenuDisplay : MonoBehaviour
    {

    // PUBLIC SETTINGS ********************************************************
        
    public GameObject previouslySelectedObject;
    public GameObject MenuInventory;
    public Tooltip tooltip;
        
        
    // PRIVATE STATE **********************************************************      

    protected GameObject objectDefault;        
    protected SoundPlayer soundPlayer;
    protected bool tooltipShown=false;


    // UNITY METHODS **********************************************************

    public void OnDisable()
    {
        TurnOffSubMenu();
        ResetCursorItem();
    } 

    // EVENT HANDLERS *********************************************************

    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    // PUBLIC METHODS *********************************************************    

    public void SetToDefaultSlot()
    {
        EventSystem.current.SetSelectedGameObject(objectDefault);
    }

    public void SetToPreviousSlot()
    {
        EventSystem.current.SetSelectedGameObject(previouslySelectedObject);
    }

    public void TurnOffSubMenu()
    {
        if (MenuInventory.activeInHierarchy)
            MenuInventory.SetActive(!MenuInventory.activeSelf);
    }

    public void TurnOnSubMenu()
    {
        if (!MenuInventory.activeInHierarchy) MenuInventory.SetActive(!MenuInventory.activeSelf);
    }

    /*public bool IsSameInstance(InventorySlot obj)
    {
        return (obj.GetInstanceID() == previouslySelectedObject.GetInstanceID());
    }*/

    // PRIVATE METHODS ********************************************************

    protected virtual void InitializeReferences() {
        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
    }  
        
    protected void CreateDefaultSlotReference(GameObject obj)
    {
        EventSystem.current.SetSelectedGameObject(obj);
        objectDefault = obj;
    }
        
    protected void SetToFirstSubMenuChild()
    {
        EventSystem.current.SetSelectedGameObject(MenuInventory.transform.GetChild(0).gameObject);
    }
       
    protected void DisableTooltip()
    {
        if(tooltipShown){
            tooltip.tooltipAnimator.SetTrigger("Hide");
            tooltipShown=false;
        }
    }        

    protected void ResetCursorItem()
    {           
        Destroy(CursorData.tempItemDrag);
        CursorData.cursorInventorySlot = null;
    }         
     
    protected void PlayMenuClickSound()
    {           
        soundPlayer.Play("EquipItem");
    }
         
    protected void OnExit(GameObject obj)
    {    
        DisableTooltip();
    }   
                 
    }
}