using System;
using UnityEngine.Tilemaps;
public class TopTile:Tile {

    public enum TopTileType { TEST,empty}
    public TopTileType topTileType;

    static Type GetRandomEnum<Type>()
    {
        System.Array A = System.Enum.GetValues(typeof(Type));
        Type V = (Type)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
        
    }

    public TopTile(int x, int y, int z, int type2)
    {
        this.topTileType = GetRandomEnum<TopTileType>();
    }
       /* if (z == 0)
        {
            
            switch (type2)
            {
                    case 1:
                    type = Type.TEST;
                    break;

                    case 2:
                    type = Type.TEST2;
                    break;

                    case 3:
                    type = Type.TEST3;
                    break;

                    case 4:
                    type = Type.TEST4;
                    break;

                    case 5:
                    type = Type.TEST5;
                    break;

                    case 6:
                    type = Type.TEST6;
                    break;

                    case 7:
                    type = Type.TEST7;
                    break;

                    case 8:
                    type = Type.Blob;
                    break;

                   // default:
                   // type = Type.TEST2;
                   // break;
            }
        }      */
    
   
    /*public Type SetTileType (Tile.Type type)
    {
       
        switch (type)
        {
            case Type.dirt:
                connectToNeighbor = true;
                break;

            default:
                connectToNeighbor = false;
                break;
        }
        
        this.type = type;

        OnTileTypeChange(this);
        Tile[] neighbors = GetNeighbors();
        for (int i = 0; i < neighbors.Length; i++)
        {
            neighbors[i].OnTileTypeChange(neighbors[i]);
        }
    }*/

    /*public Tile[] GetNeighbors(bool diagonals=false)
    {
        Tile[] neighbors;
        if (diagonals)
        {
            neighbors = new Tile[8];
            neighbors[0] = PlatformLoad.instance.GetTileAt(x, y + 1);
            neighbors[1] = PlatformLoad.instance.GetTileAt(x+1, y );
            neighbors[2] = PlatformLoad.instance.GetTileAt(x, y - 1);
            neighbors[3] = PlatformLoad.instance.GetTileAt(x-1, y);
            neighbors[4] = PlatformLoad.instance.GetTileAt(x+1, y + 1);
            neighbors[5] = PlatformLoad.instance.GetTileAt(x-1, y + 1);
            neighbors[6] = PlatformLoad.instance.GetTileAt(x+1, y - 1);
            neighbors[7] = PlatformLoad.instance.GetTileAt(x-1, y - 1);
        }
        else
        {
            neighbors = new Tile[4];
            neighbors[0] = PlatformLoad.instance.GetTileAt(x, y + 1);
            neighbors[1] = PlatformLoad.instance.GetTileAt(x + 1, y);
            neighbors[2] = PlatformLoad.instance.GetTileAt(x, y - 1);
            neighbors[3] = PlatformLoad.instance.GetTileAt(x - 1, y);
        }
        return neighbors;
    }
    */
    /*public void RegisterOnTileTypeChange(Action<Tile> callback)
    {
        OnTileTypeChange += callback;
    }*/
}
