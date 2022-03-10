using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using PixelCrushers;
using Player;
public class LoadSceneTrigger : MonoBehaviour
{
    ChangeScene changeScene;
    public string spawnPoint;
    public string sceneName;
    public string lastScene;

    public InputAction okAction;


    void Start()
    {
        changeScene = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<ChangeScene>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            
           if( InputDeviceManager.DefaultGetButtonDown("Enter")){
                 submit(); }
            // var isOk = okAction.ReadValue<bool>();
            
               
        }
    }
    public void submit()
    {

        var name = this.gameObject.name;

        if (name.Contains("."))
        {
            var strings = name.Split('.');
            sceneName = strings[0];
            var spawnpointName = (strings.Length > 1) ? strings[1] : null;
        }

        else
        {
            sceneName = name;
        }

        var spawn = SceneManager.GetActiveScene().name;
        if(sceneName!="platformer")
        changeScene.LoadScene(sceneName + "@" + spawn + spawnPoint, lastScene);
        else{
             changeScene.LoadScene(sceneName);
        }

    }
}
