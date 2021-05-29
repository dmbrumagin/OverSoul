using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tilepositions : MonoBehaviour
{
    public  Tilemap tileMap = null;    

    void Awake()
    {
        tileMap = transform.GetComponent<Tilemap>();
        tilePos(PlatformLoad.availablePlaces);
    }
 
    public void tilePos(List<Vector3> availablePlaces)
    {
        for (int n = tileMap.cellBounds.xMin; n <tileMap.cellBounds.xMax; n++)
        {
            for (int p = tileMap.cellBounds.yMin; p <tileMap.cellBounds.yMax; p++)
            {               
                Vector3Int localPlace = (new Vector3Int(n, p, 0));
                Vector3 place = tileMap.CellToWorld(localPlace);
                
                if (tileMap.HasTile(localPlace))
                {
                    availablePlaces.Add(place);                    
                }
                else
                {
                    //No tile at "place"
                }
            }
        }
    }
}
