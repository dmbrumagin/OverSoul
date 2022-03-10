using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using InventoryRelated;
using PixelCrushers.DialogueSystem.Demo;
using UnityEngine.Rendering.PostProcessing;
using System.Collections;
public class PlatformLoad : MonoBehaviour
{
    public Transform player;
    public static float renderDistance=4;    
    public static List<Vector3> tileLocations;
    static Dictionary<Vector2, Chunk> chunkMap;
    private GameObject[] platformAssets;
    private GameObject[] commonAssets;
    private AssetBundle assetBundle;
    private AssetBundle commonAssetBundle;
    public List<GameObject> bottom;
    public List<GameObject> middle ;
    public List<GameObject> top ;
    public static GameObject chest { get; private set; }
    public static GameObject coin { get; private set; }
    public static List<GameObject> enemylist { get; private set; }
    public GameObject[] Prefab;   
    int prefabIndex;
    private BoxCollider2D mainCollider;
    private EdgeCollider2D[] sideColliders;
    int bottomColliderPosition = -25;
    float time = 1f;
    Vector3 vectorStart = new Vector3(0,0,0);
    bool fluctuate= false;
    bool running = false;

   SmoothCameraWithBumper cam;
    private void Awake()
    {
        LoadAssetBundles();
        LoadAssets();
        FindReferences();
        ResetContainers();        
        RandomizeLevelContents();        

        AssetBundle.UnloadAllAssetBundles(false);
    }

    private void Start(){
        cam = Camera.main.GetComponent<SmoothCameraWithBumper>();
        sideColliders = GameObject.FindGameObjectWithTag("sideBorder").GetComponents<EdgeCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        var list1 = new List<Vector2>();
        list1.Add(new Vector2(0,-25));
        list1.Add(new Vector2(0,(Chunk.screenHeight*(ChangeScene.endOfLevel+1))));
        sideColliders[0].SetPoints(list1);
        //sideColliders[0].offset = new Vector2(Chunk.screenWidth/2,(Chunk.screenHeight*(ChangeScene.endOfLevel+1)));
        var list2 = new List<Vector2>();
        list2.Add(new Vector2(Chunk.screenWidth,-25));
        list2.Add(new Vector2(Chunk.screenWidth,Chunk.screenHeight*(ChangeScene.endOfLevel+1)));
        sideColliders[1].SetPoints(list2);

        var list3 = new List<Vector2>();
        list3.Add(new Vector2((sideColliders[1].offset.x+Chunk.screenWidth),bottomColliderPosition));
        list3.Add(new Vector2((sideColliders[0].offset.x),bottomColliderPosition));
        sideColliders[2].SetPoints(list3);
        //sideColliders[1].offset = new Vector2(Chunk.screenWidth/2,(Chunk.screenHeight*(ChangeScene.endOfLevel+1)));


    }

    private void LoadAssetBundles()
    {
        ChangeScene.PLATFORMFOLDER = "test";
        assetBundle = AssetLoad.LoadAssetBundle("/platform/" + ChangeScene.PLATFORMFOLDER);
        commonAssetBundle= AssetLoad.LoadAssetBundle("/common");
    }

    private void LoadAssets()
    {
        platformAssets = AssetLoad.LoadAllPrefabsAt(assetBundle);
        commonAssets = AssetLoad.LoadAllPrefabsAt(commonAssetBundle);
    }

    private void FindReferences()
    {
        chest = FindObject("chest");
        coin = FindObject("coin");
        bottom = FindObjects("bottom"); 
        middle =FindObjects("middle");
        top = FindObjects("top");
        enemylist = FindObjects("enemy");   
    }

    private void ResetContainers()
    {
        Prefab = new GameObject[ChangeScene.endOfLevel + 1];
        chunkMap = new Dictionary<Vector2, Chunk>();
        tileLocations = new System.Collections.Generic.List<Vector3>();
    }

    private int RandomIndex(List<GameObject> list){
        return UnityEngine.Random.Range(0, list.Count-1);
    }

    private void RandomizeLevelContents()
    {
        Prefab[0] = bottom[RandomIndex(bottom)];
        
        for (int i = 1; i < ChangeScene.endOfLevel; i++)
        {
            var valid = false;

            while (!valid) {
                
                Prefab[i] = middle[RandomIndex(middle)];

                
                
                if (conditionCheck(Prefab[i-1].GetComponent<Chunk>().topConditions,(Prefab[i].GetComponent<Chunk>().bottomConditions))) 
                    valid = true;                
            }
        }

        Prefab[ChangeScene.endOfLevel] = top[RandomIndex(bottom)];
    }

    public bool conditionCheck(Chunk.Condition[] conditions, Chunk.Condition[] compareConditions){
       var valid = false;
            foreach(Chunk.Condition conditionToCompare in compareConditions){
                 if(conditions.Contains(conditionToCompare)){
                        valid=true;
                 }                  
                 
                 else{valid= false;}
                   
            }
            
        return valid;

    }

    public List<GameObject> FindObjects(string search)
    {
        List<GameObject> results = new List<GameObject>();
        foreach ( GameObject g in platformAssets)
        {
            if (g.name.Contains(search))
                results.Add(g);

        }
        return results;
    }
    public GameObject FindObject(string search)
    {
        foreach (GameObject g in commonAssets)
        {
            var temp = g;
           if (g.name.Contains(search))
                return temp;

        }
        return null;
    }

    void Update()
    {
        FindChunksToLoad();
        DeleteChunkAt();
        
    }
    void FixedUpdate()
    {
        CheckDisableCameraMovement();
    }
       private void CheckDisableCameraMovement()
       {
           
           if(player==null)
           player=GameObject.FindGameObjectWithTag("Player").transform;

            if(player.transform.position.y<=(bottomColliderPosition+6))
            {
                cam.freezeY=true;              
                if(player.transform.position.y<=(bottomColliderPosition+2)){
                    Camera.main.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled=true;
                
                    if(!running)
                    {                    
                    StartCoroutine(Fluctuate());
                    }
                }
                else if(player.transform.position.y>(bottomColliderPosition+2))
                {
                    Camera.main.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled=false;
                
                    if(running)
                    {
                        StopCoroutine(Fluctuate());
                        running = false;           
                    }
                    var bloom = Camera.main.transform.GetChild(0).gameObject.GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>();
                  bloom.intensity.Interp(0f,0f,Time.deltaTime);
                }
            }
            else
            {
                cam.freezeY=false;
                time =1f;
                    
                if(running)
                {
                    StopCoroutine(Fluctuate());
                    running = false;
                }
                 
               Camera.main.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled=false;
           }
           
       
       }

public IEnumerator Fluctuate()
{
    running=true;
    var bloom = Camera.main.transform.GetChild(0).gameObject.GetComponent<PostProcessVolume>().profile.GetSetting<Bloom>();
    if(fluctuate)
    {
                
        yield return new WaitForFixedUpdate();
        bloom.intensity.Interp(bloom.intensity,.1f,Time.deltaTime);
        if(bloom.intensity>=.008f)
            fluctuate=false;
        StartCoroutine(Fluctuate());
    }
    else
    {
        yield return new WaitForFixedUpdate();
        bloom.intensity.Interp(bloom.intensity,0f,Time.deltaTime);
        if(bloom.intensity<=.001f)
            fluctuate=true;
        StartCoroutine(Fluctuate());
    }
}

    void FindChunksToLoad()
    {
        int yPos = (int)player.position.y;        

        //horizontal
        //for (int i = 0; i < 1; i++)
        //{
        //vertical
            for (int j = yPos; j < yPos +2*Chunk.screenHeight; j += Chunk.screenHeight)
            {
                MakeChunkAt(0, j);
            }
        //}
    }
  
    void MakeChunkAt(int x, int y)
    {
        x = (int)(x / (float)Chunk.screenWidth) * Chunk.screenWidth;
        y = (int)(y / (float)Chunk.screenHeight) * Chunk.screenHeight;

        if (chunkMap.ContainsKey(new Vector2(x, y)) == false&&y<= ChangeScene.endOfLevel * Chunk.screenHeight)
        {
            if(y>= 0 && y <= ChangeScene.endOfLevel * Chunk.screenHeight)
            {
                GameObject chunk = Instantiate(Prefab[(int)y/Chunk.screenHeight], new Vector3(x, y, 0f), Quaternion.identity);
                chunkMap.Add(new Vector2(x, y), chunk.GetComponent<Chunk>());
            }        
        }   
        if (y-(2*Chunk.screenHeight)>bottomColliderPosition)
        {
            bottomColliderPosition= y-(2*Chunk.screenHeight);
        
            Debug.Log(bottomColliderPosition + "Bottom Collider");
        
            var list3 = new List<Vector2>();
            list3.Add(new Vector2((sideColliders[1].offset.x+Chunk.screenWidth),bottomColliderPosition));
            list3.Add(new Vector2((sideColliders[0].offset.x),bottomColliderPosition));
            sideColliders[2].SetPoints(list3);   
            cam.pos= bottomColliderPosition;
        }
    }

    void DeleteChunkAt()
    {
        List<Chunk> deleteChunks = new List<Chunk>(chunkMap.Values);
        Queue<Chunk> deleteQueue = new Queue<Chunk>();

        foreach (Chunk chunk in deleteChunks)
        {
            float distanceFromGameObject = Vector3.Distance(player.position, chunk.transform.position);

            if (distanceFromGameObject > renderDistance * Chunk.screenHeight)
                deleteQueue.Enqueue(chunk);
        }

        while (deleteQueue.Count > 0)
        {
            Chunk chunk = deleteQueue.Dequeue();
            chunkMap.Remove(chunk.transform.position);
            chunk.enabled = false;
            Destroy(chunk.gameObject);
        }
    }

    Chunk GetChunk(int x, int y)
    {
        x = Mathf.FloorToInt(x / (float)Chunk.screenWidth) * Chunk.screenWidth;
        y = Mathf.FloorToInt(y / (float)Chunk.screenHeight) * Chunk.screenHeight;

        Vector2 chunkPos = new Vector2(x, y);

        if (chunkMap.ContainsKey(chunkPos))
            return chunkMap[chunkPos];

        else return null;
    }
}
