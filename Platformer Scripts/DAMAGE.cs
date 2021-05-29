using Sounds;
using System.Collections;
using UnityEngine;

public class DAMAGE : MonoBehaviour {
    
    public Rigidbody2D body;
    public float currentHealth;
    public bool damageIncurred=false;
    Collision2D whatHitMe;
    private float damage;
    private float enemyDamage;
    bool vulnerable=true;
    public Player.PlayerStats playerStats;
    public SpriteRenderer spritechar;
    public SoundPlayer soundPlayer;

    void Awake()
    {

        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = playerStats.PlayerHP;
    }

    void Update()
    {
        if (damageIncurred == false)
        {
            CheckDamage();
        }
    }

    
    
    void CheckDamage()
    {
        damageIncurred = true;    
        return;               
    }

    IEnumerator invulnerableTimer()
    {
        vulnerable = false;
        spritechar.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(.1f);
        spritechar.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(.9f);
        vulnerable = true;
    }

    void Damaged(float enemyDamage)
    {
        Debug.Log(enemyDamage + "damage"+ (((playerStats.attributesList[5].amount / 10) * playerStats.attributesList[2].amount) + playerStats.attributesList[2].amount) );
        if (enemyDamage > (((playerStats.attributesList[5].amount / 10) * playerStats.attributesList[2].amount) + playerStats.attributesList[2].amount))
        {
            Debug.Log(enemyDamage+"damage");
            soundPlayer.Play("PlayerDamaged");
            
            damage = enemyDamage -  (((playerStats.attributesList[5].amount / 10) * playerStats.attributesList[2].amount) + playerStats.attributesList[2].amount);
            currentHealth = currentHealth - damage;

            if (vulnerable == true)
            {
                StartCoroutine(invulnerableTimer());
            }
        }
        damageIncurred = false;
    }

    void OnCollisionEnter2D(Collision2D whatHitMe)
    {
        if (whatHitMe.gameObject.tag == "Enemy"&& vulnerable == true)
        {
            enemyDamage = whatHitMe.gameObject.GetComponent<DamageEnemy>().attack;
            Damaged(enemyDamage);
        }
    }

    void OnCollisionStay2D(Collision2D whatHitMe)
    {
        if (whatHitMe.gameObject.tag == "Enemy" && vulnerable == true)
        {
            enemyDamage = whatHitMe.gameObject.GetComponent<DamageEnemy>().attack;
            Damaged(enemyDamage);
        }
    }


}
