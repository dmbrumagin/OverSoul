using UnityEngine;
using Player;
using System.Collections;
using Sounds;

// TODO go through this and move stuff we want (sounds, death animation), then remove this
public class Player2 : Character2d {
    
    private GameObject playerMenu;
    private bool attacking;
    private bool dashing;
    public GameObject attack;
    private SoundPlayer soundPlayer;
    private PlayerDamageHandler damage;

    public int dashLength;

    private void Start()
    {
        playerMenu = GameObject.FindGameObjectWithTag("PlayerMenu");
        handler = GetComponent<MenuHandler>();
        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
        damage = this.GetComponent<PlayerDamageHandler>();
    }

    private void FixedUpdate()
    {
        // TODO
        // animove();

        /*if (damage.currentHealth <= 0)
            animate.SetBool("death", true);
        
        if (Input.GetButtonDown("Attack") && attacking == false)
        {
            animate.SetBool("attack", true);
            StartCoroutine(AttackTimer());
        }*/

        Move2d();
    }

    private void Update ()
    {
        //possible issue TODO fixed update and update run at different points
        // TODO
        // GetInput();       
    }

    private void LateUpdate()
    {
       //TODO add bools and animations to controller animoveUpdate();
    }

    private void OnDisable()
    {
        //TODO might be unnecessary - there was a bug that caused the character to move slightly after the menu disable character movement.
        xinput = 0;
        yinput = 0;
    }

    private void GetInput()
    {
        if (Input.GetButtonDown("Dash") && dashing == false)
        {
            dashing = true;
            soundPlayer.Play("Dash"); // TODO move to DashController
            //TODO handle direction  StartCoroutine(DashToPosition(body, new Vector2(transform.position.x + (xinputlast * dashLength), transform.position.y), .1f));
            StartCoroutine(DashTimer());

        }
        else         
        {
            xinput = Input.GetAxisRaw("Horizontal");
            yinput = Input.GetAxisRaw("Vertical");
        }
    }


    public new void animove()
    {
        if (yinput == 0 && xinput == 0)
        {
            animate.SetLayerWeight(0, 1);
            animate.SetLayerWeight(1, 0);
            animate.SetLayerWeight(2, 0);
            animate.SetLayerWeight(3, 0);
        }
        else if (yinput != 0 || xinput != 0)
        {
            animate.SetLayerWeight(0, 0);
            animate.SetLayerWeight(1, 1);
            animate.SetLayerWeight(2, 0);
            animate.SetLayerWeight(3, 0);
            animate.SetFloat("y", yinput);
            animate.SetFloat("x", xinput);
        }

        if (Input.GetButton("Attack"))
        {
            animate.SetLayerWeight(0, 0);
            animate.SetLayerWeight(1, 0);
            animate.SetLayerWeight(2, 1);
            animate.SetLayerWeight(3, 0);
            animate.SetFloat("y", yinput);
            animate.SetFloat("x", xinput);
        }
        else if (Input.GetButton("AttackBow"))
        {
            animate.SetLayerWeight(0, 0);
            animate.SetLayerWeight(1, 0);
            animate.SetLayerWeight(2, 0);
            animate.SetLayerWeight(3, 1);
            animate.SetFloat("y", yinput);
            animate.SetFloat("x", xinput);
        }

    }

    void animoveUpdate()
    {
        //TODO remove - optimize
        if (animate.GetBool("attack") == true)        
            animate.SetBool("attack", false);
        
    }

    IEnumerator AttackTimer()
    {
        soundPlayer.Play("Attack"); // TODO move to AttackController
        attacking = true;

        //TODO add directions
        attack.SetActive(true);

        yield return new WaitForSeconds(.3f);
        attacking = false;

        attack.SetActive(false);        
    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(1.2f);
        dashing = false;
    }

    public IEnumerator DashToPosition(Rigidbody2D body, Vector2 position, float timeToMove)
    {
        var currentPos = body.position;
        var t = 0f;

        while (t < 1)
        {
            t += Time.deltaTime / timeToMove;
            body.AddForce(Vector2.Lerp(currentPos, position, t));
            yield return null;
        }
    }
}
