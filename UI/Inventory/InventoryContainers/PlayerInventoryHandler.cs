

using Player;
using UnityEngine;
using Sounds;
using UI;
using System.Linq;

namespace InventoryRelated
{
    public class PlayerInventoryHandler : MonoBehaviour
    {

    // PUBLIC SETTINGS ********************************************************

    public static InventoryObject inventory;  
    public static AbilityInventoryObject abilityInventory;
    public static InventoryObject equipInventory;    
    public static AbilityInventoryObject tempAbilityInventory;
    public ScriptableObject[] inventoryContainer;


    // PRIVATE STATE **********************************************************

    protected static ItemType[] equipableTypes = { ItemType.Hat, ItemType.Weapon, ItemType.Tunic, ItemType.Soul };  

    // UNITY METHODS **********************************************************

    private void Awake()
    {        
        inventory = (InventoryObject)inventoryContainer[0];
        equipInventory = (InventoryObject)inventoryContainer[1];
        abilityInventory = (AbilityInventoryObject)inventoryContainer[2]; 
        tempAbilityInventory = (AbilityInventoryObject)inventoryContainer[3];        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {

        if(collider.GetComponent<GroundItem>())
        {        
            if (IsEmptyAbilitySlot())
                PickupItem(collider);
        }       

        else if(collider.GetComponent<GroundAbility>())
        {        
            if (IsEmptyAbilitySlot())
                PickupAbility(collider);
        }        

        else if (collider.GetComponent<Money>())
            PickupMoney(collider);   

        else if (collider.tag=="Arrow")
            PickupArrow(collider);       
    }

    public void OnApplicationQuit()
    {
        //reset inventories
        inventory.Container.slots =new InventorySlot[25];
        equipInventory.Container.slots = new InventorySlot[4];
        abilityInventory.Container.abilities = new AbilityInventorySlot[42];
        tempAbilityInventory.Container.abilities = new AbilityInventorySlot[24];
    }


    // PUBLIC METHODS *********************************************************

    public static bool IsEquipable(InventorySlot obj)
    {
        return equipableTypes.Contains(obj.item.type);       
    }

    public static void AddToOtherInventory(InventorySlot obj,ItemDisplay fromThisInterface)
    {
        inventory.AddItem(obj.item, obj.amount);
        SoundPlayer.soundPlayer.Play("UnequipItem");
    }

    public static void switchObjectToMainInventory(InventorySlot obj)
    {
        equipInventory.MoveItem(obj,inventory.FirstEmptySlot(obj.item));   
        SoundPlayer.soundPlayer.Play("UnequipItem");
    }

    public static void BuyItem(InventorySlot buyObject ,ItemDisplay otherInterface)
    {
        otherInterface.TurnOffSubMenu();

        if (otherInterface.IsEmptySlot(buyObject))
        {
            var playerCash = PlayerStats.StatTypeToPlayerStat[StatType.Money].getAmount();
            var priceToBuy = buyObject.item.price;

            if (playerCash >= priceToBuy)
            {
                PlayerStats.StatTypeToPlayerStat[StatType.Money].setAmount(playerCash - priceToBuy);
                AddToOtherInventory(buyObject,otherInterface);
                otherInterface.SetToPreviousSlot();
            }
        }
    }

    public static void TakeItem(InventorySlot obj, ItemDisplay otherInterface, InventoryObject inventory)
    {
        otherInterface.TurnOffSubMenu();

        if (otherInterface.IsEmptySlot(obj))
        {
           // if (otherInterface.IsSameInstance(obj))
           // {
                AddToOtherInventory(obj,otherInterface);
                inventory.RemoveItem(obj);
                
           // }
        }
        otherInterface.SetToDefaultSlot();
    }

    public static void UseItem(InventorySlot obj)
    {
        if (IsEquipable(obj))
        {
            MenuHandler.inventoryScreen.TurnOffSubMenu();
            equipItem();
        }

        else
            buffItem();        
    }

    public static void equipItem()
    {
        var equipScreenObjToSlot = MenuHandler.equipScreen.ObjectCurrentSlotMapping;
        var inventoryScreenObjToSlot =MenuHandler.inventoryScreen.ObjectCurrentSlotMapping;

        foreach (Transform t in MenuHandler.equipScreen.transform)
        {
            var equipSlot = equipScreenObjToSlot[t.gameObject];
            if (equipSlot.CanPlace(inventory.database.GetItem[inventoryScreenObjToSlot[MenuHandler.inventoryScreen.previouslySelectedObject].ID]))
            {
                var buffs = inventoryScreenObjToSlot[MenuHandler.inventoryScreen.previouslySelectedObject].item.buffs;
                PlayerStats.RecalculateAttributeValue(buffs);

                var isSet = setIfAlreadyLearned();
                if(!isSet){
                abilityInventory.AddAbility(new Ability(abilityInventory.database.GetAbility[inventoryScreenObjToSlot[MenuHandler.inventoryScreen.previouslySelectedObject].item.ability.Id]));
                Debug.Log("is not set");
                }

                if(IsEquipable(equipSlot))
                {
                    var buffsToRemove = equipSlot.item.buffs;
                    PlayerStats.RecalculateAttributeValue(buffsToRemove, isAdditive: false);
                }

                MenuHandler.inventoryScreen.updateCharWeaponSprite(true);
                inventory.MoveItem(inventoryScreenObjToSlot[MenuHandler.inventoryScreen.previouslySelectedObject], equipScreenObjToSlot[t.gameObject]);                        
                SoundPlayer.soundPlayer.Play("EquipItem");
                MenuHandler.inventoryScreen.SetToPreviousSlot();
                return;
            }
        }
    }

    public static bool setIfAlreadyLearned(){
        for(int i=0; i < abilityInventory.Container.abilities.Length;i++){
                    if(abilityInventory.database.GetAbility[MenuHandler.inventoryScreen.ObjectCurrentSlotMapping[MenuHandler.inventoryScreen.previouslySelectedObject].item.ability.Id].Id==abilityInventory.Container.abilities[i].ability.Id){
                        Debug.Log("is set");
                        abilityInventory.Container.abilities[i].ID=MenuHandler.inventoryScreen.ObjectCurrentSlotMapping[MenuHandler.inventoryScreen.previouslySelectedObject].item.ability.Id;
                        abilityInventory.Container.abilities[i].UpdateSlot(abilityInventory.Container.abilities[i].ID,abilityInventory.Container.abilities[i].ability);
                        return true;
                    }
                }
                return false;

    }

    public static void buffItem()
    {
        Debug.Log("can't equip");
        //TODO handle non equipment items
        MenuHandler.inventoryScreen.TurnOffSubMenu();
        MenuHandler.inventoryScreen.SetToPreviousSlot();
    }

    public static void UnEquipItem(InventorySlot obj)
    {
        if (IsEquipable(obj))
        {
            MenuHandler.equipScreen.TurnOffSubMenu();
            MenuHandler.inventoryScreen.updateCharWeaponSprite(false);

            abilityInventory.RemoveAbility(obj.item.ability);
            
            if (MenuHandler.equipScreen.IsSlotAvailable(obj))
            {
                var buffs = obj.item.buffs;
                PlayerStats.RecalculateAttributeValue(buffs, isAdditive: false);

                switchObjectToMainInventory(obj);
                MenuHandler.equipScreen.SetToPreviousSlot();
            }                
        }

        else
            fullInventory();        
    }

    public static void fullInventory( )
    {
        Debug.Log("can't unequip");
        //TODO Handle inventory is full
        MenuHandler.equipScreen.TurnOffSubMenu();
        MenuHandler.equipScreen.SetToPreviousSlot();
    }


    // PRIVATE METHODS ********************************************************   
    
    private bool IsEmptyItemSlot()
    {
        foreach (InventorySlot slot in inventory.Container.slots)
        {
            if(slot.ID == -1)
                return true;                         
        }

        return false;
    }

    private bool IsEmptyAbilitySlot()
    {
        foreach (AbilityInventorySlot slot in abilityInventory.Container.abilities)
        {
            if(slot.ID == -1)
                return true;                           
        }
        
        return false;
    }

    private void PickupItem(Collider2D pickupObject)
    {
        var itemToPickup = pickupObject.GetComponent<GroundItem>();

        SoundPlayer.soundPlayer.Play("Pickup");
        inventory.AddItem(new Item(itemToPickup.item), 1);
        Destroy(pickupObject.gameObject);
    }

    private void PickupAbility(Collider2D pickupObject)
    {
        var abilityToPickup = pickupObject.GetComponent<GroundAbility>();

        SoundPlayer.soundPlayer.Play("Pickup");
        abilityInventory.AddAbility(new Ability(abilityToPickup.ability));
        Destroy(pickupObject.gameObject);
    }

    private void PickupMoney(Collider2D pickupMoney)
    {
        SoundPlayer.soundPlayer.Play("Pickup");
        var money = PlayerStats.StatTypeToPlayerStat[StatType.Money].getAmount();
        PlayerStats.StatTypeToPlayerStat[StatType.Money]
            .setAmount( money + pickupMoney.GetComponent<Money>().amount);
        Destroy(pickupMoney.gameObject);
    }

    private void PickupArrow(Collider2D pickupArrow)
    {
        SoundPlayer.soundPlayer.Play("Pickup");
        var arrows = PlayerStats.StatTypeToPlayerStat[StatType.Arrow].getAmount();
        if(arrows<=9) // TODO maxArrows should be configurable
        PlayerStats.StatTypeToPlayerStat[StatType.Arrow]
            .setAmount( arrows + 1);
        Destroy(pickupArrow.gameObject);
    }
    
    }
}