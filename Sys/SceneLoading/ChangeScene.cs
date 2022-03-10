using Sounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using LevelManager;
using PixelCrushers.DialogueSystem.Demo.Wrappers;
using InventoryRelated;

public class ChangeScene : MonoBehaviour {
    private SoundPlayer soundPlayer;
    public static int endOfLevel;
    public static string PLATFORMFOLDER;
    public static string ITEMFOLDER;
    public static string MONSTERFOLDER;
  //  private Vector2 lastPos;
    private GameObject Player;
    private string LastScene;
    private Camera cam;
    private GameObject hud;
   // public float countDownRange;
    private MenuHandler anyMenu;
    public bool newRange;
    private GameObject staminaBar;
    public static ItemObject[] items;
    private GameObject health;
    private bool countdownToShrine;
    public LevelSettingsDatabase levelSettingsDatabase;
    private LevelSetting levelSetting;
    private FadeController fadeController;

    public void Awake() {
        
    }

    public void Start()
    {
        staminaBar = GameObject.FindGameObjectWithTag("Stamina");
        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
        fadeController = GameObject.FindGameObjectWithTag("MusicController").GetComponent<FadeController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        // TODO REMOVE?
        if (Player)
        {
            anyMenu = Player.GetComponent<MenuHandler>();
            var intelligenceStat = PlayerStats.StatTypeToPlayerStat[StatType.Intelligence].getAmount();
           // countDownRange = intelligenceStat + Random.Range(5, 10);
            cam.GetComponent<SmoothCameraWithBumper>().target = Player.transform;
        }

        
        hud = GameObject.FindGameObjectWithTag("HUD");
        health = GameObject.FindGameObjectWithTag("Health");
        countdownToShrine = true;

        // TODO move to event system - input control items
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        ChangeProperty();
       
    }
    public void Update()
    {
        /*if (Player)
        {
            if (playerController.IsMoving() && countdownToShrine == true) {
                countDownRange = countDownRange - Time.deltaTime;
            }

            if (countDownRange < 0)
            {
                countDownRange = 0;
                LoadScene("platformer");
            }
        }*/
    }

    

    private IEnumerator WaitFrame()    
    {
        yield return null;        
    }
    
    public void ChangeProperty()
    {
        if (SceneManager.GetActiveScene().name != name)
        {
            StartCoroutine(WaitFrame());

            if (SceneManager.GetActiveScene().name  == "platformer")            
                ShrineValues();            
            
            else            
                RegValues();
        }
    }

    public void LoadScene(string name)
    {
        LoadScene(name, null);
    }

    public void LoadScene(string name, string lastScene)
    {
        if (name.Contains("@"))
        {
            var strings = name.Split('@');
            var nameNew = strings[0];

            levelSetting = levelSettingsDatabase.GetLevel[nameNew];
        }
        else
        {
            levelSetting = levelSettingsDatabase.GetLevel[name];
        }

        // TODO https://trello.com/c/HTrbqU5c/9-create-system-for-non-linear-soundtrack
        if (levelSetting.name == "platformer")
        {
            // musicController.SetTrack(Track.Level1)
            // musicController.SetIntensity(Intensity.Battle)
            fadeController.StartFadeOut(1, 0.02f);
            fadeController.StartFadeIn(2, 0.02f, 0.75f);
        }
        else if (levelSetting.name == "1")
        {
            // musicController.SetTrack(Track.Level1)
            // musicController.SetIntensity(Intensity.Overworld);
            fadeController.StartFadeOut(2, 0.02f);
            fadeController.StartFadeIn(1, 0.02f);
        }
        else if (levelSetting.name == "shop")
        {
            // musicController.SetTrack(Track.Shop)
            // musicController.SetIntensity(Intensity.Overworld)
        }
        else
        {
            Debug.LogWarning("Failed to find music for level setting: " + levelSetting.name);
        }

        if (name != "platformer")
        {
            PLATFORMFOLDER = levelSetting.platformLocation;
            MONSTERFOLDER = levelSetting.monsterLocation;
            ITEMFOLDER = levelSetting.itemLocation;
            endOfLevel = levelSetting.endOfLevel;
        }
        
        if (Player && name == "platformer") {
            PixelCrushers.SaveSystem.lastPos = Player.transform.position;
        }

        if (newRange == true) NewRange();
        PixelCrushers.SaveSystem.lastScene = SceneManager.GetActiveScene().name;

        if (SceneManager.GetActiveScene().name != name)
        {

            StartCoroutine(WaitFrame());

            if (name == "platformer") ShrineValues();
            else RegValues();
           
            PixelCrushers.SaveSystem.LoadScene(name);
        }
    }

    public void NewRange()
    {
      /*  newRange = false;
        countDownRange = Random.Range(5 +PlayerStats.StatTypeToPlayerStat[StatType.Intelligence].getAmount(), 
        10 + PlayerStats.StatTypeToPlayerStat[StatType.Intelligence].getAmount());
        staminaBar.GetComponent<StaminaBar>().time = countDownRange;*/
    }

    public void ShrineValues()
    {
        countdownToShrine = false;
        Player.transform.position = new Vector2(10, 7);
        Player.GetComponent<PlayerDamageHandler>().enabled = true;
        Player.GetComponent<SpriteRenderer>().enabled = true;
       
    }

    public void RegValues()
    {
        // TODO https://trello.com/c/wW5M1qgt/215-create-shrine-entrance-and-remove-countdown
        LastScene = SceneManager.GetActiveScene().name;
        countdownToShrine = true; // toggles platformer

        if (hud)
            hud.SetActive(true);

        if (Player)
        {
            Player.GetComponent<SpriteRenderer>().enabled = true;
            Player.transform.position= PixelCrushers.SaveSystem.lastPos;
        }
    }   
}
