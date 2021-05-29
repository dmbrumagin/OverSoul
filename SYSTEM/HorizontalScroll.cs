using UnityEngine;

public class HorizontalScroll : MonoBehaviour
{
    public GameObject Player;
    public GameObject map;
    public float bounds;


    void Awake()
    {
        map = this.gameObject;
        Player = GameObject.FindGameObjectWithTag("Player");
        bounds=map.GetComponent<SpriteRenderer>().bounds.size.x/2;        
    }
    void Update()
    {
        if (Player.transform.position.x>bounds- .001f || Player.transform.position.x < -bounds + .001f)
        {
            if (Player.transform.position.x>0)
            {
                Player.transform.position = new Vector3(-Player.transform.position.x + .1f, Player.transform.position.y, Player.transform.position.z);
            }

            else
            {
                Player.transform.position = new Vector3(-Player.transform.position.x - .1f, Player.transform.position.y, Player.transform.position.z);
            }
        }
    }
}
