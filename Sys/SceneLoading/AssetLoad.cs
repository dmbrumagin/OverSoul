using UnityEngine;
using InventoryRelated;

public class AssetLoad : MonoBehaviour
{
    public static GameObject[] LoadAllPrefabsAt(AssetBundle bundle)

    {
        if (bundle)
        {        
            var assetBundle = bundle;
            
            return assetBundle.LoadAllAssets<GameObject>();
        }

        else
            return null;        
    }


    public static AssetBundle LoadAssetBundle(string path)

    {
        if (path != null)
        {
            AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();
            
            if (path != "")
                if (path.EndsWith("/"))
                    path = path.TrimEnd('/');

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

            return AssetBundle.LoadFromFile(assetPath);
        }

        else
            return null;
    }
    
    public static GameObject LoadPrefabAt(string path, string name)
    {
        if (path != null)
        {
            AssetBundle[] bundles = Resources.FindObjectsOfTypeAll<AssetBundle>();

            if (path != "")
                if (path.EndsWith("/"))
                    path = path.TrimEnd('/');

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
            return null;
    }
    public static ItemObject[] LoadAllItemsAt(AssetBundle bundle)

    {
        if (bundle)
        {
            var assetBundle = bundle;

            return assetBundle.LoadAllAssets<ItemObject>();
        }

        else
            return null;
    }
}
   



