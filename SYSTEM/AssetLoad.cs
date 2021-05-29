using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Player;
public class AssetLoad : MonoBehaviour
{

    public static GameObject[] LoadAllPrefabsAt(string path)

    {
        if (path!=null)
        {
            AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();

            if (path != "")
            {
                if (path.EndsWith("/"))
                {
                    path = path.TrimEnd('/');
                }
            }

#if unityeditor == false
            string assetPath = Application.streamingAssetsPath + path;
#else        
            string assetPath =  Application.dataPath + "Assets/AssetBundles/" + path;

            public int targetFrameRate = 60;
            private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = targetFrameRate;
    }

    private void Update()
    {
        if (Application.targetFrameRate != targetFrameRate)
        {
            Application.targetFrameRate = targetFrameRate;
        }
    }
#endif
            var assetBundle = AssetBundle.LoadFromFile(assetPath);
            GameObject[] prefab = assetBundle.LoadAllAssets<GameObject>();
            
            return prefab;
        }

        else
        {
            return null;
        }
    }

    public static GameObject LoadPrefab(string path,string name)
    {
        if (path != null)
        {
            AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();
           
            if (path != "")
            {
                if (path.EndsWith("/"))
                {
                    path = path.TrimEnd('/');
                }
            }
#if unityeditor == false

            string assetPath = Application.streamingAssetsPath + path;
#else
        string assetPath =  Application.dataPath + "Assets/AssetBundles/" + path;
#endif
            var assetBundle = AssetBundle.LoadFromFile(assetPath);
            GameObject prefab = assetBundle.LoadAsset<GameObject>(name);

            return prefab;
        }
        else
        {
            return null;
        }

    }
    
    public static ItemObject[] LoadAllItemsAt(string path)

    {
        if (path != null)
        {

            AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();

            if (path != "")
            {
                if (path.EndsWith("/"))
                {
                    path = path.TrimEnd('/');
                }
            }
#if unityeditor == false

            string assetPath = Application.streamingAssetsPath + path;
#else
        string assetPath =  Application.dataPath + "Assets/AssetBundles/" + path;
#endif
            var assetBundle = AssetBundle.LoadFromFile(assetPath); 
            ItemObject[] prefab = assetBundle.LoadAllAssets<ItemObject>();     

            return prefab;
        }
        else
        {
            return null;
        }


    }

}
   



