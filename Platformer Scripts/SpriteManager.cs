using System.Collections;
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
        Sprite[] sprites =   Resources.LoadAll<Sprite>("Placeholder Tiles");        

        foreach (Sprite s in sprites)
        {
            tileSprites.Add(s.name, s);            
        }

        Sprite[] spritestop = Resources.LoadAll<Sprite>("TEST");

        foreach (Sprite s in spritestop)
        {
            tileSprites.Add(s.name, s);
        }        
    }
    
    public Sprite GetSprite (Tile tile)
    {      
        if (tileSprites.ContainsKey(tile.type.ToString()))
            return tileSprites[tile.type.ToString()];
        
       Debug.LogError("Unrecognized tile type:" + tile.type.ToString());
       return null;        
    }   
}
