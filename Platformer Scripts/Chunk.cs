using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour {

    public static int screenWidth = 20;
    public static int screenHeight = 14;
    Dictionary<Tile, GameObject> tileGOMap;
    int generate;
    public Tile[,] tiles;   

    void Awake()
    {      
        tileGOMap = new Dictionary<Tile, GameObject>();        
        tiles = new Tile[screenWidth, screenHeight];      
    }

   void Start()
    {
        GenerateTile(PlatformLoad.availablePlaces);
    }

    public void GenerateTile(List<Vector3> availablePlaces)
    {
        for (int i = 0; i < screenWidth; i++)
        {            
            for (int j = (int)transform.position.y; j <screenHeight+transform.position.y; j++)
            {                
               var vec = new Vector3((float)i, (float)j, (float)0);
               generate = Random.Range(0, 3);//Mathf.RoundToInt(//Mathf.PerlinNoise(590f, .03f/ UnityEngine.Random.value+1));
               var vec2 = new Vector3((float)i, (float)j + 1, (float)0);

                if (generate == 1&&availablePlaces.Contains(vec) && !availablePlaces.Contains(vec2))
                {
                    generate =  Mathf.RoundToInt(10 * Mathf.PerlinNoise(890f, 1 / UnityEngine.Random.value + 1));
                    tiles[i, j- (int)transform.position.y] = new Tile( i,  j , 0, generate);                    
                    tiles[i, j-(int)transform.position.y].RegisterOnTileTypeChange(OnTileTypeChange);
                    
                    
                    GameObject tileGO = new GameObject("Top_Tile" + tiles[i, j - (int)transform.position.y].x + "_" + tiles[i,j - (int)transform.position.y].y +"_"+generate);
                    if (generate == 8 &&transform.position.y>10)
                    {                        
                        Instantiate(PlatformLoad.enemylist[Random.Range(0,PlatformLoad.enemylist.Length-1)], vec2, Quaternion.identity);
                    }

                    if (generate == 7 && transform.position.y > 10&&Random.Range(0,4)==3)
                    {
                        Instantiate(PlatformLoad.chest[Random.Range(0, PlatformLoad.chest.Length - 1)], vec2, Quaternion.identity);
                    }

                    if (generate == 2 && transform.position.y > 10)
                    {
                        Instantiate(PlatformLoad.coin[Random.Range(0, PlatformLoad.coin.Length - 1)], vec2, Quaternion.identity);
                    }

                    tileGO.transform.position = new Vector3(tiles[i, j - (int)transform.position.y].x, tiles[i, j - (int)transform.position.y].y, tiles[i, j - (int)transform.position.y].z);                
                    tileGO.transform.SetParent(this.transform, true);
                    tileGO.AddComponent<SpriteRenderer>();
                    tileGO.AddComponent<BoxCollider2D>();
                    tileGO.AddComponent<Animator>();
                    tileGOMap.Add(tiles[i, j - (int)transform.position.y], tileGO);
                    OnTileTypeChange(tiles[i, j - (int)transform.position.y]);
                }              
            }
        }
    }
    
    void OnTileTypeChange(Tile tile)
    {
      //  if (tile.type == Tile.Type.stone)
      //  {
     //       BoxCollider2D collider = tileGOMap[tile].GetComponent<BoxCollider2D>();
     //       collider.size = new Vector2(1, 1);
      //  }
        SpriteRenderer spriteRenderer = tileGOMap[tile].GetComponent<SpriteRenderer>();       
        spriteRenderer.sortingLayerName = "HUD";
        spriteRenderer.sprite = SpriteManager.instance.GetSprite(tile);
    }    
}
