using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Threading.Tasks;

public class PlayerActionController : MonoBehaviour
{

    // PRIVATE STATE **********************************************************
    private JumpController jumpController;
    private ChestMenuHandler chestHandler;
    private ShopMenuHandler shopHandler;


    // UNITY METHODS **********************************************************
    public void Awake()
    {
        jumpController = GetComponent<JumpController>();
    }

    // EVENT HANDLERS & HELPERS ***********************************************
    public void Action(InputAction.CallbackContext context)
    {
        // TODO https://trello.com/c/kgxx2BYp/209-cleanup-player-world-interactions
        // find last actionable object in range
        // either open or consume the object
        if (context.performed)
        {
            if (chestHandler != null && !chestHandler.IsActive()) chestHandler.Open(context);
            if (shopHandler != null && !shopHandler.IsActive()) shopHandler.Open(context);
            if (shopHandler != null) Debug.Log(new { shop = shopHandler.IsActive() });
        }
        else
        {

        }
    }
    public void Cancel(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("CANCEL ACTION IN PLAYER CONTROLLER");
            if (chestHandler != null && chestHandler.IsActive())
            {
                if (jumpController.enabled) StartCoroutine(CloseChestAfterJumpGracePeriod());
                else chestHandler.Close();
            }
            if (shopHandler != null && shopHandler.IsActive())
            {
                Debug.Log("CLOSING SHOP");
                if (jumpController.enabled) StartCoroutine(CloseShopAfterJumpGracePeriod());
                else shopHandler.Close();
            }
        }
        else
        {

        }
    }


    // PUBLIC METHODS *********************************************************

    // TODO create ReadyActionableObject(Actionable actionableObject)
    public void ReadyChest(ChestMenuHandler chestHandler)
    {
        this.chestHandler = chestHandler;
    }

    public void ReadyShop(ShopMenuHandler shopHandler)
    {
        this.shopHandler = shopHandler;
    }

    // Openable
    // public void ReadyOpenableObject(Closable openableObject) {

    // }


    // PRIVATE METHODS ********************************************************

    private IEnumerator WaitForJumpGracePeriod()
    {
        // NOTE: prevents jumping when using gamepad to close chest
        // (default mapping does jump & close, so the graceperiod allows a jump to occur immediately after closing)
        yield return new WaitForSeconds(jumpController.GetJumpGracePeriod());
        yield return new WaitForEndOfFrame();
        yield return new WaitForFixedUpdate();
    }
    private IEnumerator CloseChestAfterJumpGracePeriod()
    {
        yield return WaitForJumpGracePeriod();
        chestHandler.Close();
    }

    private IEnumerator CloseShopAfterJumpGracePeriod()
    {
        yield return WaitForJumpGracePeriod();
        shopHandler.Close();
    }
}
