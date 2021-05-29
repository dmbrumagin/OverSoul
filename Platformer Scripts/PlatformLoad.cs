using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlatformLoad : MonoBehaviour
{
    public GameObject chunkGO;
    public static float renderDist=4;    
    public static System.Collections.Generic.List<Vector3> availablePlaces;
    static Dictionary<Vector2, Chunk> chunkMap;
    private GameObject[] bottom;
    private GameObject[] middle ;
    private GameObject[] top ;
    public static GameObject[] chest  { get; private set; }
    public static GameObject[] coin { get; private set; }
    public ItemObject[] items;
    public static GameObject[] enemylist { get; private set; }
    System.Collections.Generic.List<GameObject> prefabList = new System.Collections.Generic.List<GameObject>();
    public GameObject[] Prefab;   
    int prefabIndex;

    private void OnDisable()
    {
        AssetBundle.UnloadAllAssetBundles(false);
        prefabList.Clear();
    }
    private void OnEnable()
    {
     // items = AssetLoad.LoadAllItemsAt("/platform/" + changescene.test + "/items");
        chest = AssetLoad.LoadAllPrefabsAt("/chest");
        coin = AssetLoad.LoadAllPrefabsAt("/coin");
        bottom = AssetLoad.LoadAllPrefabsAt("/platform/" + changescene.PLATFORMFOLDER + "/bottom");
        middle = AssetLoad.LoadAllPrefabsAt("/platform/" + changescene.PLATFORMFOLDER + "/middle");
        top = AssetLoad.LoadAllPrefabsAt("/platform/" + changescene.PLATFORMFOLDER + "/top");
        enemylist = AssetLoad.LoadAllPrefabsAt("/platform/" + changescene.PLATFORMFOLDER + "/enemy");                
        Prefab = new GameObject[changescene.endOfLevel + 1];
        chunkMap = new Dictionary<Vector2, Chunk>();
        availablePlaces = new System.Collections.Generic.List<Vector3>();
        prefabIndex = Random.Range(0, bottom.Length - 1);
        Prefab[0] = bottom[prefabIndex];

        for (int i = 1; i < changescene.endOfLevel; i++)
        {
            prefabIndex = Random.Range(0, middle.Length);
            Prefab[i] = middle[prefabIndex];
        }
        
        prefabIndex = Random.Range(0, top.Length - 1);
        Prefab[changescene.endOfLevel] = top[prefabIndex];
        
        for (int i = 0; i < Prefab.Length; i++)
        {
            prefabList.Add(Prefab[i]);
        }       
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "platformer")
        {
            FindChunksToLoad();
            DeleteChunkAt();
        }
        

        /* float mouseX = (Camera.main.ScreenToWorldPoint(Input.mousePosition).x); 
         float mouseY = (Camera.main.ScreenToWorldPoint(Input.mousePosition).y);

         Tile t = GetTileAt(mouseX, mouseY);

         Debug.Log(t.x + ", " + t.y);

         if (Input.GetKeyDown("w") && t!=null)
         {
             if (t.type == Tile.Type.stone)
                 t.SetTileType(Tile.Type.dirt);
             else if (t.type == Tile.Type.dirt)
                 t.SetTileType(Tile.Type.stone);
         }*/
    }

    void FindChunksToLoad()
    {
        int yPos = (int)transform.position.y;
        if (yPos<0)
        {
            yPos = 0;
        }

        for (int i = 0; i < 1; i++)
        {
            for (int j = yPos; j < yPos +2*Chunk.screenHeight; j += Chunk.screenHeight)
            {
                MakeChunkAt(i, j);
            }
        }
    }
  
    void MakeChunkAt(int x, int y)
    {
        x = (int)(x / (float)Chunk.screenWidth) * Chunk.screenWidth;
        y = (int)(y / (float)Chunk.screenHeight) * Chunk.screenHeight;

        if (chunkMap.ContainsKey(new Vector2(x, y)) == false&&y<= changescene.endOfLevel * Chunk.screenHeight)
        {
            if (y == 0){
              
                int prefabIndex = 0;
                GameObject chunk = Instantiate(prefabList[prefabIndex], new Vector3(x, y, 0f), Quaternion.identity);

                chunkMap.Add(new Vector2(x, y), chunk.GetComponent<Chunk>());
                
            }

            if (y== changescene.endOfLevel * Chunk.screenHeight)
            {
                int prefabIndex = prefabList.Count-1;
              
                
                GameObject chunk = Instantiate(prefabList[prefabIndex], new Vector3(x, y, 0f), Quaternion.identity);

                chunkMap.Add(new Vector2(x, y), chunk.GetComponent<Chunk>());

            }

            if (y > changescene.endOfLevel * Chunk.screenHeight)
            {         
            }

            if(y>0&&y< changescene.endOfLevel * Chunk.screenHeight)
            {
                Debug.Log(prefabList.Count-1);
                int prefabIndex = UnityEngine.Random.Range(1, prefabList.Count-1);

               
                GameObject chunk = Instantiate(prefabList[prefabIndex], new Vector3(x, y, 0f), Quaternion.identity);

                chunkMap.Add(new Vector2(x, y), chunk.GetComponent<Chunk>());
            }        
        }      
    }

    void DeleteChunkAt()
    {
        System.Collections.Generic.List<Chunk> deleteChunks = new System.Collections.Generic.List<Chunk>(chunkMap.Values);
        Queue<Chunk> deleteQueue = new Queue<Chunk>();

        for (int i = 0; i < deleteChunks.Count; i++)
        {
            float distance = Vector3.Distance(transform.position, deleteChunks[i].transform.position);

            if (distance > renderDist * Chunk.screenHeight)
            {
                deleteQueue.Enqueue(deleteChunks[i]);
            }
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
        {
            return chunkMap[chunkPos];
        }

        else return null;
    }
    
    /*public Tile GetTileAt(int x,int y)
    {
        Chunk chunk = GetChunk(x, y);
        if (chunk != null)
        {
            return chunk.tiles[x - (int)chunk.transform.position.x, y - (int)chunk.transform.position.y];
        }
        return null;
    }

    public Tile GetTileAt(float x, float y)
    {
        int X = Mathf.FloorToInt(x);
        int Y = Mathf.FloorToInt(y);
        Chunk chunk = GetChunk(X, Y);
        if (chunk != null)
        {
            return chunk.tiles[X-(int)chunk.transform.position.x, Y-(int)chunk.transform.position.y];
        }
        return null;
    }*/
}
