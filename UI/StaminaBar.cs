using UnityEngine;

public class StaminaBar : MonoBehaviour
{
    public GameObject stamina;
    public float time, timeNew;
    public ChangeScene changeScene;

    private void Start()
    {
        changeScene = GameObject.FindGameObjectWithTag("SceneLoader").GetComponent<ChangeScene>();
       // time = changeScene.countDownRange;
        stamina = this.gameObject;
    }
    
    void Update()
    {
       // timeNew = changeScene.countDownRange;

        if (timeNew > 0)
            stamina.transform.localScale = new Vector3((timeNew / time), 1, 1);
              
        else
            timeNew = 0;
    }   
}
