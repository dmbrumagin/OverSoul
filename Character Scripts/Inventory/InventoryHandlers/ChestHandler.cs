using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ChestHandler : MonoBehaviour
{

    public GameObject MenuInventory;
    public GameObject Canvas;    
    public Animator anim3d;
    public bool chest=false;
    GameObject PlayerObject;
    public PlayerHandler handler;

    private void Awake()
    {
        PlayerObject = GameObject.FindGameObjectWithTag("Player").gameObject;
        handler = PlayerObject.GetComponent<PlayerHandler>();
        if (SceneManager.GetActiveScene().name=="platformer")
        {
            anim3d = GetComponent<Animator>();
            GetComponent<SpriteRenderer>().enabled = true;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            // GetComponent<sortingorder>().enabled = false;
        }

        else
        {
            anim3d = gameObject.transform.GetChild(0).GetComponent<Animator>();
            GetComponent<SpriteRenderer>().enabled = false;
            gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    void Update()
    {
        if ((Input.GetButtonDown("Cancel") && MenuInventory.activeInHierarchy == false&& Canvas.activeInHierarchy == true))
        {

            Canvas.SetActive(false);
            handler.anyMenu = false;
                chest = false;
            anim3d.SetBool("Chest", false);
        }

        if (Input.GetButtonDown("Cancel") && MenuInventory.activeInHierarchy == true)
        {
            MenuInventory.SetActive(false);
            EventSystem.current.SetSelectedGameObject(GetComponentInChildren<ChestInterface>().lastObject);
        }

        if (chest == true)
        {
            if (Canvas.activeInHierarchy == false)
            {
                //   PlayOpen();
                Canvas.SetActive(true);
                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.tag);
        if (other.tag == "Player" && Input.GetButtonDown("Submit")&& GameObject.FindGameObjectWithTag("PlayerMenu") == null)
        {
            handler.anyMenu = true;
            Debug.Log(other.tag + "_");
            chest = true;
            anim3d.SetBool("Chest", true);
        }
    }
   
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag=="Player" && Input.GetButtonDown("Submit") && GameObject.FindGameObjectWithTag("PlayerMenu") == null&&handler.anyMenu==false)
        {
            handler.anyMenu = true;
            Debug.Log(other.tag+"_");
            chest = true;
            anim3d.SetBool("Chest", true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player" )
        {
            handler.anyMenu = false;
            chest = false;
            anim3d.SetBool("Chest", false);
        }
    }  
}
