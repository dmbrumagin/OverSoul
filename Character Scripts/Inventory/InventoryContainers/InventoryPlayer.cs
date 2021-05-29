using Player;
using UnityEngine;
using Sounds;

public class InventoryPlayer : SystemInventory
{
    public AbilityInventoryObject abilityInventory;
    public InventoryObject eqinventory;
    SoundPlayer soundPlayer;

    private void Awake()
    {       
        soundPlayer= GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>(); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        bool doNotAdd = false;
        var item = other.GetComponent<GroundItem>();
        for (int i = 0; i < inventory.Container.slots.Length; i++)
        {
           if(inventory.Container.slots[i].ID == -1)
            {
                doNotAdd = true;               
            }
        }

        if (item&&doNotAdd==true)
        {
            soundPlayer.Play("Pickup");
            inventory.AddItem(new Item(item.item), 1);
            Destroy(other.gameObject);
        }
      
        var ability = other.GetComponent<GroundAbility>();

        for (int i = 0; i < abilityInventory.Container.abilities.Length; i++)
        {
            if (abilityInventory.Container.abilities[i].ID == -1)
            {
                doNotAdd = true;
            }
        }

        if (ability && doNotAdd == true)
        {
            soundPlayer.Play("Pickup");
            abilityInventory.AddAbility(new Ability(ability.ability));
            Destroy(other.gameObject);
        }

        var money = other.GetComponent<Money>();

        if (money)
        {
            soundPlayer.Play("Pickup");
            var mon = GetComponent<PlayerStats>().attributesList[6].amount;
            GetComponent<PlayerStats>().attributesList[6].amount = mon + other.GetComponent<Money>().amount;
            Destroy(other.gameObject);
        }
    }

   

    private void OnApplicationQuit()
    {
        inventory.Container.slots=new InventorySlot[15];
        eqinventory.Container.slots = new InventorySlot[4];
        abilityInventory.Container.abilities = new AbilityInventorySlot[4];
    }
}
