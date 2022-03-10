using UnityEngine;
using Player;

public class Character2d : MonoBehaviour
{
    public Animator animate;
    [SerializeField]
    public float speed;
    public int jumpHeight = 20;
    [SerializeField]
    private float offseta;
    public Rigidbody2D body;
    Vector2 moveVector;
    public float xinput, yinput;
    public MenuHandler handler;

    void Start()
    {
        offseta = .5f;
        body = GetComponent<Rigidbody2D>();       
    }

    private void OnEnable()
    {
        if (GetComponent<PlayerStats>())
        {
            // TODO implement speed attribute in new playerController
            // https://trello.com/c/iOVuSWRz/207-implement-speed-attribute-in-new-playercontroller
            // speed = 3 + GetComponent<PlayerStats>().attributesList[1].amount / 150f;
        }
    }

    void Update()
    {
        animove();
        Move2d();
        // TODO REFACTOR
        /*if (Input.GetKeyDown("space")) // TODO gamepad
        {
            Jump();
        }*/
    }

    public void animove()
    {

        if (animate.layerCount >= 2)
        {

            if (animate.GetLayerWeight(1) == 0 && (yinput == 0 || xinput == 0))
            {

            }
            else if (yinput == 0 && xinput == 0)
            {
                animate.SetLayerWeight(1, 0);
            }
            if (yinput != 0 || xinput != 0)
            {
                animate.SetLayerWeight(1, 1);
                animate.SetFloat("y", yinput);
                animate.SetFloat("x", xinput);
            }
        }
    }

    public void Move2d()
    {
        moveVector.x = xinput;
        
        if (body.velocity.magnitude < 1) {
            var movement = new Vector2(moveVector.x, 0);
            body.AddForce(1 * movement);
        }
    }

    public void Jump() {
        body.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
    }
  
}