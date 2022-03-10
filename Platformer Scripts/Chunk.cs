using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.Tilemaps;

public class Chunk : MonoBehaviour {

public enum Condition{
    left,
     middle,
     right
 }
    //TODO Move static vars to proper home
    public static int screenWidth = 20;
    public static int screenHeight = 14;

    public Condition[] topConditions;
    public Condition[] bottomConditions;
    //left to right   
    public Tilemap tilemap;

    void Awake()
    {       
        tilemap = transform.GetChild(0).GetChild(0).GetComponent<Tilemap>();      
    }

   void Start()
    {
        GenerateTile(PlatformLoad.tileLocations);
        updateTileSprites(PlatformLoad.tileLocations);
    }

    public void GenerateTile(List<Vector3> tileLocations)
    {
        for (int i = 0; i < screenWidth; i++)
        {            
            for (int j = (int)transform.position.y; j <screenHeight+transform.position.y; j++)
            {                
                var vectorOfTile = new Vector3((float)i, (float)j, (float)0);
                var vectorOfTileAbove = new Vector3((float)i, (float)j + 1, (float)0);
               
                if(tileLocations.Contains(vectorOfTile) 
                &&!tileLocations.Contains(vectorOfTileAbove))
                { 
                    var generate =  Mathf.RoundToInt(10 * Mathf.PerlinNoise(890f, 1 / UnityEngine.Random.value + 1));

                    if (generate == 8 &&transform.position.y>10)
                        Instantiate(PlatformLoad.enemylist[Random.Range(0,PlatformLoad.enemylist.Count-1)], vectorOfTileAbove, Quaternion.identity);

                    if (generate == 7 && transform.position.y > 10 && Random.Range(0,4)==0)
                        Instantiate(PlatformLoad.chest, vectorOfTileAbove, Quaternion.identity);

                    if (generate == 2 && transform.position.y > 10)
                        Instantiate(PlatformLoad.coin, vectorOfTileAbove, Quaternion.identity);                    
                }      
            }
        }
    }   
    public void updateTileSprites(List<Vector3> tileLocations){
        foreach (Vector3 position in tileLocations){
            var vectorConversion = new Vector3Int ((int)position.x,(int)position.y,(int)position.z);
            Tile tile = ScriptableObject.CreateInstance<Tile>();

            //TODO load array of sprites to change
            //use setTiles instead with array of Tiles/Sprites.
            //currently just sets back to tile until implementation
            
            tile.sprite=tilemap.GetSprite(vectorConversion);
            tilemap.SetTile(vectorConversion,tile);
        }
    }
}
