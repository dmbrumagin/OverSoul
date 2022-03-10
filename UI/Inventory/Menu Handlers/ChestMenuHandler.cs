using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class ChestMenuHandler : MonoBehaviour
{
    public GameObject chestSubMenu;
    public GameObject chestDisplay;    
    private GameObject player;
    private PlayerActionController playerActionController;
    private PlayerInput playerInput;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerActionController = player.GetComponent<PlayerActionController>();
        playerInput = player.GetComponent<PlayerInput>();
    }

    public bool IsActive() 
    {
        return chestDisplay.activeInHierarchy;
    }

    public void Open(InputAction.CallbackContext context) 
    {
        if (context.performed) 
        {
            playerInput.SwitchCurrentActionMap("UI");
            chestDisplay.SetActive(true);   
        }
    }

    public void Close(InputAction.CallbackContext context) 
    {
        if (context.performed) Close();
    }
    
    public void Close(bool exitingArea = false)
    {
        var isSubMenuActive = chestSubMenu.activeInHierarchy;

        if(exitingArea)
        {
            chestDisplay.SetActive(false);
            playerInput.SwitchCurrentActionMap("Player");
        }

        else
        {
            if (isSubMenuActive&&exitingArea) {
                chestSubMenu.SetActive(false);
                chestDisplay.GetComponent<ChestDisplay>().SetToDefaultSlot();
            } else if (!isSubMenuActive && chestDisplay.activeInHierarchy) {
                chestDisplay.SetActive(false);
                playerInput.SwitchCurrentActionMap("Player");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // TODO?
        if (other.tag == "Player")
            playerActionController.ReadyChest(this);
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" && chestDisplay.activeInHierarchy)
            Close(exitingArea: true);

        playerActionController.ReadyChest(null); 
    }  
}
