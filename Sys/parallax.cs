using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parallax : MonoBehaviour
{
    public float parallaxEffect;
    public float spriteLen, origin;
    public GameObject cam;

    // Start is called before the first frame update
    void Start()
    {
        spriteLen = this.GetComponent<SpriteRenderer>().bounds.size.x;
        origin = transform.position.x;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distance = cam.transform.position.x * parallaxEffect;
        transform.position = new Vector3(origin + distance, transform.position.y, transform.position.z);
    }
}
