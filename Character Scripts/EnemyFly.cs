using UnityEngine;
using Player;

public class EnemyFly : Character2d
{
    [SerializeField]
    private float followDist;    
    private Vector3 charPos;
    private Vector3 enemyPos;
    private Vector3 newPos;
    private bool follow;
    private GameObject Player; 
            
    void Start()
    {
        speed = 5;
        followDist = 3;
        Player = GameObject.FindGameObjectWithTag("Player");
        handler = Player.GetComponent<MenuHandler>();
        animate = GetComponent<Animator>();
        follow = false;
    }

    void Update()
    {
        if (!follow) checkFollow();
        else enemyFollow();
        animove();
    }
    public void FixedUpdate() {
        if (follow) Move2d();
    }

    public void checkFollow()
    {
        // TODO if enemy was hit, should follow
        // Debug.Log("checkFollow");
        var charPos = Player.transform.position;
        var enemyPos = transform.position;
        var newPos =Vector3.Distance(charPos, enemyPos);

        if(newPos<followDist)
            follow = true;              
    }

    public new void Move2d()
    {
        var moveVector = new Vector2(xinput, yinput);      
        body.AddForce(moveVector);           
    }
    
    void enemyFollow()
    {
        if (follow ==true)
        {
            Vector3 charPos = Player.transform.position;
            Vector3 enemyPos = transform.position;
            
            
            if (charPos.x - enemyPos.x >= 0 && charPos.y - enemyPos.y >= 0)
            {
                xinput = speed;
                yinput = speed;
            }

            else if (charPos.x - enemyPos.x >= 0 && charPos.y - enemyPos.y <= 0)
            {
                xinput = speed;
                yinput = -speed;
            }

            else if (charPos.x - enemyPos.x <= 0 && charPos.y - enemyPos.y >= 0)
            {
                xinput = -speed;
                yinput = speed;
            }

            else if (charPos.x - enemyPos.x <= 0 && charPos.y - enemyPos.y <= 0)
            {
                xinput = -speed;
                yinput = -speed;
            }
        }
            
    }
}