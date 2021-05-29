using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaminaBar : MonoBehaviour
{
    public GameObject stamina;
    public float time, timeNew;
    public GameObject sceneloader;

    private void Awake()
    {
        stamina = this.gameObject;
    }
    void OnEnable()
    {
        time = sceneloader.GetComponent<changescene>().RN;        
        timeNew = time;
    }
    
    void Update()
    {
        timeNew = sceneloader.GetComponent<changescene>().RN;

        if (timeNew<0)
        {
            timeNew = 0;
        }

        else
        {
                stamina.transform.localScale = new Vector3((timeNew / time), 1, 1);                        
        }
        
    }
   
}
