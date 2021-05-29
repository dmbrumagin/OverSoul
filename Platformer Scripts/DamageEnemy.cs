using System.Collections;
using UnityEngine;
using Player;
using Sounds;

public class DamageEnemy : MonoBehaviour {

    public Rigidbody2D body;
    public SpriteRenderer spritechar;
    public int currentHealth;
    public bool damageIncurred=false;
    Collider2D whatHitMe;
    public Vector3 dir;
    bool vulnerable=true;
    public int attack;
    public int playerAttack;
    public GameObject Player;
    public int enemyDefence;
    public SoundPlayer soundPlayer;
    
    void Start ()
    {
        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
        body = GetComponent<Rigidbody2D>();
        Player = GameObject.Find("Player2d");
        spritechar = GetComponent<SpriteRenderer>();
    }  
    
    void Update ()
    {
        CheckDamage();  
    }    

    void CheckDamage()
    {
        if (currentHealth <= 0)
        {
            soundPlayer.Play("DeathSmall");
            Destroy(this.gameObject);
        }

        else
        {           
            return;
        }       
    }

    IEnumerator invulnerableTimer()
    {
        vulnerable = false;
        spritechar.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(.11f);
        spritechar.color = new Color(1, 1, 1, 1);
        vulnerable = true;
    }

    void Damaged(int playerAttack)
    {
        damageIncurred = false;

        if (playerAttack>enemyDefence)
        {
            currentHealth = currentHealth - (playerAttack - (enemyDefence ));
            dir = transform.position - Player.transform.position;
            body.AddForce(dir * 2000);

            if (vulnerable == true)
            {
                StartCoroutine(invulnerableTimer());
            }
        }                       
    }

    void OnTriggerEnter2D(Collider2D whatHitMe)
    {
        if (whatHitMe.gameObject.tag == "Attack"&& vulnerable == true)
        {
            Debug.Log("player attack" + Player.GetComponent<PlayerStats>().attributesList[4].amount);
            Debug.Log("player strength" + Player.GetComponent<PlayerStats>().attributesList[3].amount);
            playerAttack = ((Player.GetComponent<PlayerStats>().attributesList[4].amount / 10) * Player.GetComponent<PlayerStats>().attributesList[3].amount) + Player.GetComponent<PlayerStats>().attributesList[3].amount;
            Damaged(playerAttack);
        }
    }

    void OnTriggerStay2D(Collider2D whatHitMe)
    {
        if (whatHitMe.gameObject.tag == "Attack" && vulnerable == true)
        {
            Debug.Log("player attack" + Player.GetComponent<PlayerStats>().attributesList[4].amount);
            Debug.Log("player strength" + Player.GetComponent<PlayerStats>().attributesList[3].amount);
            playerAttack = ((Player.GetComponent<PlayerStats>().attributesList[4].amount / 10) * Player.GetComponent<PlayerStats>().attributesList[3].amount)+ Player.GetComponent<PlayerStats>().attributesList[3].amount;
            Damaged(playerAttack);
        }
    }

}
