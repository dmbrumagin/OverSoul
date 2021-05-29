using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneTrigger : MonoBehaviour
{
    changescene changescene;
    public string spawnPoint;
    public string sceneName;
    public bool is2d;


    void Start()
    {
        changescene = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<changescene>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag=="Player")
        {
            Debug.Log(collision.tag);

            if (Input.GetButtonDown("Submit"))
            {
                //changescene.SceneChange.SetTrigger("Show");
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

                if (is2d == true)
                {
                    changescene.LoadScene(sceneName + "@" + spawn + spawnPoint, is2d);
                }

                else
                {
                    changescene.LoadScene(sceneName + "@" + spawn + spawnPoint, is2d);
                }

            }
        }
    }
}
