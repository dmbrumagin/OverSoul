using UnityEngine;

public class CameraControler : MonoBehaviour {

    public Transform targetTransform;
    public Vector3 camtempVec3 = new Vector3();

    private void Awake()
    {
        camtempVec3.y = this.transform.position.y;
        camtempVec3.z = this.transform.position.z;
    }

    void FixedUpdate()
    {
        camtempVec3.x = targetTransform.position.x;        
        this.transform.position = camtempVec3;
    }
}

