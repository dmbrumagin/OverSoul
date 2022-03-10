using UnityEngine;
using UnityEngine.SceneManagement;

public class changescenePlatformer : MonoBehaviour {
    
    private ChangeScene changeScene;   
    private GameObject player;
    private void Start()
    {
        changeScene = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<ChangeScene>(); 
        player = GameObject.FindGameObjectWithTag("Player");       
    }
    
    public void LoadScene (string name)
    {
        changeScene.LoadScene(name);
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (SceneManager.GetActiveScene().name == "platformer" && collision.gameObject.tag=="Player")
        {
            changeScene.newRange = true;
            LoadScene(PixelCrushers.SaveSystem.lastScene);                    
        }
        else
            Destroy(collision.gameObject);
        
    }
  
}
