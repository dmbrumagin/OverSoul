using UnityEngine;

public class charactersortlayer : MonoBehaviour {    

    private SpriteRenderer image;    

    void Start ()
    {
        image = GetComponentInParent<SpriteRenderer>();
	}
	
	void Update ()
    {
        image.sortingOrder = (-(int)(100 * transform.parent.position).y);
    }
    
}
