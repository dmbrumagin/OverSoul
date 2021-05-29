using UnityEngine;
using UnityEngine.SceneManagement;

public class changescenePlatformer : MonoBehaviour {
    
    public changescene SceneLoader;   
    public GameObject Player;

    private void Awake()
    {
        SceneLoader = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<changescene>(); 
        Player = SceneLoader.Player;       
    }
    
    public void LoadScene (string name)
    {
        if (name ==  SceneLoader.LastScene)
        {
            Player.transform.position = SceneLoader.playerPosition;
        }

        SceneLoader.LoadScene(name);       
    }
  
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if (SceneManager.GetActiveScene().name == "platformer"&&collision.gameObject.tag=="Player")
        {
            SceneLoader.newRange = true;
            LoadScene( SceneLoader.LastScene);                    
        }

        else
        {
            Destroy(collision.gameObject);
        }
    }
  
}
