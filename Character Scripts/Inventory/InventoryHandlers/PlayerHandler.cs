using UnityEngine;
using UnityEngine.SceneManagement;
using Sounds;
using PixelCrushers.DialogueSystem;

namespace Player
{
    public class PlayerHandler : MonoBehaviour
    {
        public PlayerStats Player;
        public SpriteRenderer notif;
        public GameObject Canvas;
        public GameObject sub;
        public GameObject sub1;
        public GameObject sub2;
        public GameObject hud;
        public SoundPlayer soundPlayer;
        public bool anyMenu;

        public void MenuSetFalse()
        {
            anyMenu = false;
        }

        private void Awake()
        {
            soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
            notif = GameObject.FindGameObjectWithTag("notification").GetComponent<SpriteRenderer>();
            notif.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = false;
            hud.SetActive(false);                      
        }

        void Update()
        {
            if (SceneManager.GetActiveScene().name != "platformer" && SceneManager.GetActiveScene().name != "Title")
            {
                if (Input.GetButtonDown("Cancel") && Canvas.activeInHierarchy == true && sub.activeInHierarchy == false && sub1.activeInHierarchy == false && sub2.activeInHierarchy == false)
                {
                    anyMenu = false;
                    soundPlayer.Play("CloseMenu");
                    Canvas.SetActive(false);
                    return;
                }

                if (Input.GetButtonDown("Options") && Canvas.activeInHierarchy == false&&anyMenu== false)
                {

                    anyMenu = true;
                    soundPlayer.Play("OpenMenu");
                    Canvas.SetActive(true);
                    return;

                }
                if (DialogueManager.IsConversationActive)
                {
                    anyMenu = false;
                    soundPlayer.Play("CloseMenu");
                    Canvas.SetActive(false);
                    return;
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag=="Chest"|| collision.tag == "interact")
            {
                notif.enabled = true;
            }
            
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.tag == "Chest" || collision.tag == "interact")
            {
               notif.enabled = true;
            }

        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            notif.enabled = false;
        }
    }
    
}