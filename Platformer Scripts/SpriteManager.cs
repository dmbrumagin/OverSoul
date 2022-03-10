using System.Collections.Generic;
using UnityEngine;

public class SpriteManager : MonoBehaviour {

    public static SpriteManager instance;
    Dictionary<string, Sprite> tileSprites;
    
    void Awake ()
    {        
        instance = this;
        tileSprites = new Dictionary<string, Sprite>();
        LoadSprites();
    }

    void LoadSprites()
    {
        Sprite[] spritestop = Resources.LoadAll<Sprite>("TEST");

        foreach (Sprite s in spritestop)
        {
            tileSprites.Add(s.name, s);
        }        
    }
    
    public Sprite GetSprite (TopTile tile)
    {      
        if (tileSprites.ContainsKey(tile.topTileType.ToString()))
            return tileSprites[tile.topTileType.ToString()];
        
       Debug.LogError("Unrecognized tile type:" + tile.topTileType.ToString());
       return null;        
    }   
}
