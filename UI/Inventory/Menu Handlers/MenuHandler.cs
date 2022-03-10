using UnityEngine;
using UnityEngine.SceneManagement;
using Sounds;
using PixelCrushers.DialogueSystem;
using UnityEngine.InputSystem;
using UI;

namespace Player
{
    public class MenuHandler : MonoBehaviour
    {
        private SpriteRenderer notif;
        public GameObject playerMenu;
        private GameObject hud;
        
        public static EquipmentDisplay equipScreen;
        public static InventoryDisplay inventoryScreen;
        public static AbilityDisplay abilityScreen;
        public static AbilityDisplay tempAbilityScreen;
        private SoundPlayer soundPlayer;
        public bool anyMenu;


        private void Start()
        {
            soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
            playerMenu = GameObject.FindGameObjectWithTag("PlayerMenu");

            if (playerMenu)
            {                
                inventoryScreen = GameObject.FindGameObjectWithTag("inventoryScreen").GetComponent<InventoryDisplay>();
                equipScreen = GameObject.FindGameObjectWithTag("equipmentScreen").GetComponent<EquipmentDisplay>();
                abilityScreen =GameObject.FindGameObjectWithTag("abilityScreen").GetComponent<AbilityDisplay>();
                tempAbilityScreen = GameObject.FindGameObjectWithTag("tempAbilityScreen").GetComponent<AbilityDisplay>();
                playerMenu.SetActive(false);
            }

            hud = GameObject.FindGameObjectWithTag("HUD");
            notif = GameObject.FindGameObjectWithTag("notification").GetComponent<SpriteRenderer>();
            notif.enabled = false;                    
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().name != "Title")
            {
                if (DialogueManager.IsConversationActive)
                {
                    playerMenu.SetActive(false);
                }
            }
        }

        public void Close()
        {
            if (SceneManager.GetActiveScene().name != "Title"
            && playerMenu.activeInHierarchy == true) {
                soundPlayer.Play("CloseMenu");
                playerMenu.SetActive(false);
            }
        }

        public void Open()
        {
            if (SceneManager.GetActiveScene().name != "Title"
            && playerMenu.activeInHierarchy == false)
            {
                soundPlayer.Play("OpenMenu");
                playerMenu.SetActive(true);
            }
            
        }

        // TODO disable if conversation is active
        // https://trello.com/c/qyC5NIKK/203-implement-disableonmenu-attribute
        // [DisableOn(InputFocus.Conversation)]
        public void OpenClose(InputAction.CallbackContext context)
        {
            if (context.performed) {
                if (!playerMenu.activeInHierarchy) Open();
                else Close();
            }
        }

        //shows notification sprite under different conditions

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag=="Chest"|| collision.tag == "interact")
                notif.enabled = true;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Chest" || collision.tag == "interact")
               notif.enabled = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            notif.enabled = false;
        }
    }
    
}