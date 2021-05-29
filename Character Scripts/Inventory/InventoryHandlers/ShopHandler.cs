
using UnityEngine;
using PixelCrushers.DialogueSystem;
using Player;
using UnityEngine.EventSystems;

public class ShopHandler : MonoBehaviour
{
    public GameObject Canvas;
    public GameObject MenuInventory;
    public bool shop;
    GameObject PlayerObject;
    PlayerHandler handler;

    private void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player").gameObject;
        handler = PlayerObject.GetComponent<PlayerHandler>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Cancel") && MenuInventory.activeInHierarchy == false)
        {
            handler.anyMenu = false;
            DialogueLua.SetVariable("Shop", false);
            shop = false;
        }
        if (Input.GetButtonDown("Cancel") && MenuInventory.activeInHierarchy == true)
        {
            MenuInventory.SetActive(false);
            EventSystem.current.SetSelectedGameObject(GetComponentInChildren<ShopInterface>().lastObject);
        }
        if (DialogueLua.GetVariable("Shop").AsBool == true)
        {
            if (Canvas.activeInHierarchy == false)
            {
                handler.anyMenu = true;
                Canvas.SetActive(true);           
            }

        }
        else if (DialogueLua.GetVariable("Shop").AsBool == false)
        {
            if (Canvas.activeInHierarchy == true)
            {
                handler.anyMenu = false;
                Canvas.SetActive(false);               
            }
        }
        else
        {          
        }

    }
}
