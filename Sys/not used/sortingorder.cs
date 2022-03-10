using UnityEngine;

public class sortingorder : MonoBehaviour {
    
    SpriteRenderer image;

	void Start ()
    {
        image = GetComponent<SpriteRenderer>();
	}
	
	void Update ()
    {
        image.sortingOrder = (-(int)(100 * transform.position).y);
    }
}
