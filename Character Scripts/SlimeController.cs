using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class SlimeController : MonoBehaviour
{
    public float speed = 5, followDist = 3, jumpHeight = 20;
    private Animator animator;
    private Rigidbody2D body;
    private GameObject Player;
    private float xinput, yinput;
    private bool isFollow = false;

    public void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
        if (!isFollow) CheckFollow();
        else EnemyFollow();

        Animate(); // TODO https://trello.com/c/YLTKulXB/210-move-enemy-animations-to-different-classes
    }
    public void FixedUpdate()
    {
        if (isFollow) Move2d();
    }

    public void Animate()
    {
        // Debug.Log("is animating" +yinput + " " + xinput);
        if (yinput == 0 && xinput == 0) {
            animator.SetLayerWeight(1, 0);
        } else if (yinput != 0 || xinput != 0) {
            animator.SetLayerWeight(1, 1);
            animator.SetFloat("y", yinput);
            animator.SetFloat("x", xinput);
        }
    }

    public void Move2d()
    {
        if (body.velocity.magnitude < 1) {
            var movement = new Vector2(xinput*speed, yinput*speed);
            body.AddForce(1 * movement);
        }
    }

    public void Jump() {
        body.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
    }

    public void CheckFollow()
    {
    
        // Debug.Log("checkFollow");
        var charPos = Player.transform.position;
        var enemyPos = transform.position;
        var newPos =Vector3.Distance(charPos, enemyPos);

        if(newPos<followDist)
            isFollow = true;              
    
    }
    
    void EnemyFollow()
    {
        if (isFollow)
        {
            Vector3 charPos = Player.transform.position;
            Vector3 enemyPos = transform.position;
            
            if (charPos.x - enemyPos.x >= 0 && charPos.y - enemyPos.y >= 0) {
                xinput = 1;
                yinput = 1;
            } else if (charPos.x - enemyPos.x >= 0 && charPos.y - enemyPos.y <= 0) {
                xinput = 1;
                yinput = -1;
            } else if (charPos.x - enemyPos.x <= 0 && charPos.y - enemyPos.y >= 0) {
                xinput = -1;
                yinput = 1;
            } else if (charPos.x - enemyPos.x <= 0 && charPos.y - enemyPos.y <= 0) {
                xinput = -1;
                yinput = -1;
            }
        }
    }
}
