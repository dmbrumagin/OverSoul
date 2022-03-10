using UnityEngine;

public class SceneToLoad : MonoBehaviour
{
    public string sceneName;
    public string loadPoint;
    public ChangeScene changescene;    

    private void Awake()
    {
        changescene = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<ChangeScene>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            changescene.LoadScene(sceneName+"@"+loadPoint);            
        }
    }
}
