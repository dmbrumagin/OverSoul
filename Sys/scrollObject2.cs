using UnityEngine;

public class scrollObject2 : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = transform.parent.GetComponent<SpriteRenderer>().sprite;
        this.transform.position= new Vector3(-this.GetComponentInParent<SpriteRenderer>().bounds.size.x,transform.position.y,transform.position.z);
    }
}
