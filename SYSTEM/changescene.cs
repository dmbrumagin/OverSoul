using Sounds;
using PixelCrushers.DialogueSystem.Demo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using LevelManager;

public class changescene : MonoBehaviour {

    public SoundPlayer soundPlayer;
    public static int endOfLevel;
    public static string PLATFORMFOLDER;
    public static string ITEMFOLDER;
    public static string MONSTERFOLDER;
    public Vector3 playerPosition;
    public GameObject Player;
    public string LastScene;
    public Camera cam;
    public GameObject playerMenu;
    public GameObject hud;
    public float RN;
    public PlayerHandler anyMenu;
    public bool newRange = false;
    public GameObject staminaBar;
    public static ItemObject[] items;
    public bool bool2d;
    public RuntimeAnimatorController controller2d;
    public RuntimeAnimatorController controllerPlatform;
    public GameObject health;
    bool RandomGenerate;
    public LevelSettingsDatabase levelSettingsDatabase;
    LevelSetting levelSetting;

    private void Awake()
    {
        
        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
        soundPlayer.PlayMusicNow("Title");        
        anyMenu = Player.GetComponent<PlayerHandler>();
        RandomGenerate = false;
        PLATFORMFOLDER = "test";
        RN = Random.Range(5 + Player.GetComponent<PlayerStats>().attributesList[0].amount, 10+Player.GetComponent<PlayerStats>().attributesList[0].amount);
        items=AssetLoad.LoadAllItemsAt("/platform/" + PLATFORMFOLDER + "/items");
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;     
        DontDestroyOnLoad(this);        
        endOfLevel =1;
        
    }
    private void Start()
    {
        hud.SetActive(false);
    }

    public void TryGameOver()
    {
        StartCoroutine(Try());
    }

    public void Update()
    {
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)&&anyMenu.anyMenu==false&& RandomGenerate==true)
        {
            RN= RN - Time.deltaTime;

            if (RN<=0&&SceneManager.GetActiveScene().name=="GameOver")
            {
                
            }
            else if (RN <= 0)
            {
                RN = 0;
                LoadScene("platformer");
            }
        }

        //if (Player.transform.position.y <= -5 && SceneManager.GetActiveScene().name == "platformer")
        //{
        //     PlayGameOver();
        //     LoadScene("GameOver");
        //}

        if (SceneManager.GetActiveScene().name == "GameOver" && Input.GetButtonDown("Submit"))
        {
            Application.Quit();
        }
    }

    public IEnumerator Try()
    {
        if (Player.GetComponent<Player2copy>().animate.GetBool("death") == true)
        {

            soundPlayer.Play("GameOver");
            yield return new WaitForSeconds(.8f);
            LoadScene("GameOver");
            yield break;
        }

        yield return null;

        if (SceneManager.GetActiveScene().name == "platformer")
        {
            StartCoroutine(Try());
        }
    }

    private IEnumerator Wait()    
    {
        yield return null;        
    }

    void DoManualAnimationCleanup()
    {
        foreach (Animation a in FindObjectsOfType(typeof(Animation)))
        {
            a.Stop();

            List<string> clipNames = new List<string>();
            foreach (AnimationState v in a)
                clipNames.Add(v.clip.name);

            foreach (string s in clipNames)
            {
                try
                {
                    a.RemoveClip(s);
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Failed to remove animation : " + e.ToString());
                }
            }
        }
    }
    
    public void ChangeProperty()
    {
        if (SceneManager.GetActiveScene().name != name)
        {
            StartCoroutine(Wait());

            if (SceneManager.GetActiveScene().name  == "platformer")
            {
                PlatformerValues();
            }

            else if (SceneManager.GetActiveScene().name  == "Title")
            {
                TitleValues();
            }

            else
            {
                Is3dValues();
            }
        }
    }

    public void LoadScene(string name)
    {
        LoadScene(name, false);
    }

        public void LoadScene(string name, bool is2d)
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
        soundPlayer.PlayMusicNow(levelSetting.songName);
        Debug.Log(levelSetting.songName);
        
        playerPosition = Player.transform.position;

        if (name=="platformer")
        {
            
        }
        else
        {
            PLATFORMFOLDER = levelSetting.platformLocation;
            MONSTERFOLDER = levelSetting.monsterLocation;
            ITEMFOLDER = levelSetting.itemLocation;
            endOfLevel = levelSetting.endOfLevel;
        }      

        if (newRange == true)
        {
            NewRange();
        }        

        if (SceneManager.GetActiveScene().name != name)
        {
            StartCoroutine(Wait());
            if (name == "platformer")
            {
                PlatformerValues();
            }

            else if (is2d == true)
            {
                Is2dValues();
            }

            else if (name == "Title"|| name == "GameOver")
            {
                TitleValues();
            }

            else
            {
                Is3dValues();
            }

            PixelCrushers.SaveSystem.LoadScene(name);
        }

        else
        {
            Debug.Log("current name to load is last scene");
        }
    }

    public void NewRange()
    {
        newRange = false;
        RN = Random.Range(5 + Player.GetComponent<PlayerStats>().attributesList[0].amount, 10 + Player.GetComponent<PlayerStats>().attributesList[0].amount);
        staminaBar.GetComponent<StaminaBar>().time = RN;
    }

    public void TitleValues()
    {
        RandomGenerate = false;
        hud.SetActive(false);
        Player.GetComponent<SpriteRenderer>().enabled = false;
        Player.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = false;
        Player.GetComponent<DAMAGE>().enabled = false;
    }

    public void PlatformerValues()
    {
        LastScene = SceneManager.GetActiveScene().name;
        RandomGenerate = false;
        hud.SetActive(true);
        Player.transform.position = new Vector2(10, 8);
        cam.transform.position = new Vector2(10, 10);
        cam.GetComponent<CameraControler>().enabled = false;
        cam.GetComponent<SmoothCameraWithBumper>().enabled = true;
        Player.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = false;
        Player.transform.GetChild(0).GetComponentInChildren<Player3>().enabled = false;
        Player.transform.GetChild(0).GetComponentInChildren<Animator>().enabled = false;
        Player.GetComponent<Animator>().runtimeAnimatorController = controllerPlatform;
        Player.GetComponent<Rigidbody2D>().gravityScale = 3;
        Player.GetComponent<SpriteRenderer>().enabled = true;
        Player.GetComponent<PlatformLoad>().enabled = true;
        Player.GetComponent<DAMAGE>().enabled = true;
        Player.GetComponent<Player2copy>().enabled = true;
        Player.GetComponent<Player2>().enabled = false;
        Player.GetComponent<CircleCollider2D>().enabled = true;
        Player.GetComponent<BoxCollider2D>().size = new Vector2(0.3698139f, 0.6669418f);
        Player.GetComponent<BoxCollider2D>().offset = new Vector2(0.01981831f, 0.3345527f);
        health.GetComponentInChildren<HealthBar>().healthMax = Player.GetComponent<PlayerStats>().PlayerHP;
        health.GetComponentInChildren<HealthBar>().healthNew = Player.GetComponent<DAMAGE>().currentHealth;
    }

    public void Is2dValues()
    {
        RandomGenerate = false;
        cam.GetComponent<CameraControler>().enabled = false;
        cam.GetComponent<SmoothCameraWithBumper>().enabled = true;
        Player.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = false;
        Player.transform.GetChild(0).GetComponentInChildren<Animator>().enabled = false;
        Player.transform.GetChild(0).GetComponentInChildren<Player3>().enabled = false;
        Player.GetComponent<Animator>().runtimeAnimatorController = controller2d;
        Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Player.GetComponent<SpriteRenderer>().enabled = true;
        Player.GetComponent<Player2copy>().enabled = false;
        Player.GetComponent<Player2>().enabled = true;
        bool2d = false;
        health.GetComponentInChildren<HealthBar>().healthMax = Player.GetComponent<PlayerStats>().PlayerHP;
        health.GetComponentInChildren<HealthBar>().healthNew = Player.GetComponent<DAMAGE>().currentHealth;
    }

    public void Is3dValues()
    {
        RandomGenerate = true;
        cam.transform.position = new Vector3(0, 0, -4);
        hud.SetActive(true);
        cam.GetComponent<CameraControler>().enabled = true;
        cam.GetComponent<SmoothCameraWithBumper>().enabled = false;
        Player.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>().enabled = true;
        Player.transform.GetChild(0).GetComponentInChildren<Animator>().enabled = true;
        Player.transform.GetChild(0).GetComponentInChildren<Player3>().enabled = true;
        Player.GetComponent<Rigidbody2D>().gravityScale = 0;
        Player.GetComponent<SpriteRenderer>().enabled = false;
        Player.GetComponent<PlatformLoad>().enabled = false;
        Player.GetComponent<DAMAGE>().enabled = false;
        Player.GetComponent<Player2>().enabled = true;
        Player.GetComponent<Player2copy>().enabled = false;
        Player.GetComponent<Player2copy>().GetInput();
        Player.GetComponent<CircleCollider2D>().enabled = false;
        Player.GetComponent<BoxCollider2D>().size = new Vector2(0.3698139f, 0.1706687f);
        Player.GetComponent<BoxCollider2D>().offset = new Vector2(0.01981831f, 0.08641618f);
        health.GetComponentInChildren<HealthBar>().healthMax = Player.GetComponent<PlayerStats>().PlayerHP;
        health.GetComponentInChildren<HealthBar>().healthNew = Player.GetComponent<DAMAGE>().currentHealth;
    }

    public void Quit()
    {
        Application.Quit();
    }   
}
