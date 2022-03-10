

using UnityEngine;
using PixelCrushers.DialogueSystem;
using Player;
using UnityEngine.InputSystem;

public class ShopMenuHandler : MonoBehaviour
{
    public GameObject shopDisplay;
    public GameObject subMenu;
    private PlayerInput playerInput;

    private void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player").gameObject;
        playerInput = player.GetComponent<PlayerInput>();
        var playerActionController = player.GetComponent<PlayerActionController>();
        playerActionController.ReadyShop(this);
    }
    
    public void OnEnable()
    {
        Lua.RegisterFunction("StartShop", this, SymbolExtensions.GetMethodInfo(() => StartShop()));
    }

    public void OnDisable()
    {
        Lua.UnregisterFunction("StartShop");
    }

    public void StartShop()
    {
        shopDisplay.SetActive(true);
    }

    public void Open(InputAction.CallbackContext context)
    {
        if (context.performed) playerInput.SwitchCurrentActionMap("UI");
    }

    public void Close(InputAction.CallbackContext context)
    {
        if (context.performed) Close();
    }

    public void Close()
    {
        shopDisplay.SetActive(false);

        if (subMenu.activeInHierarchy)
        {
            subMenu.SetActive(false);
            shopDisplay.GetComponent<ShopDisplay>().SetToDefaultSlot();
        }
        
        playerInput.SwitchCurrentActionMap("Player");
    }

    public bool IsActive()
    {
        return shopDisplay.activeInHierarchy;
    }
}
