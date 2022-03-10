using System.Collections;
using UnityEngine;
using Player;
using Sounds;
using System;

public class DamageEnemy : MonoBehaviour {

    private Rigidbody2D body;
    private SpriteRenderer spritechar;
    private bool vulnerable = true;
    private bool isDead = false;
    private GameObject player;
    [SerializeField]
    public int health;
    [SerializeField]
    public int attack;
    [SerializeField]
    private int enemyDefence;
    private SoundPlayer soundPlayer;
    private float timeBeforeDestroy = 0.25f;
    
    void Start ()
    {
        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
        body = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        spritechar = GetComponent<SpriteRenderer>();
    }

    public void Update() {
        CheckDeath();
    }
    
    void CheckDeath()
    {
        if (!isDead && health <= 0)
        {
            isDead = true;
            soundPlayer.Play("DeathSmall");
            StartCoroutine(DelayedDestroyThis());
        }
    }

    public IEnumerator DelayedDestroyThis() {
        spritechar.color = new Color(0.5f, 0, 0, 1);
        yield return new WaitForSeconds(timeBeforeDestroy);
        Destroy(this.gameObject);
    }

    IEnumerator invulnerableTimer()
    {
        vulnerable = false;
        spritechar.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(DelayedChangeColor(1, 1, 1, 1));
        vulnerable = true;
    }

    private IEnumerator DelayedChangeColor(int r, int g, int b, int a) {
        yield return new WaitForSeconds(0.15f);
        spritechar.color = new Color(r,g,b,a);
    }

    private float knockbackDelay = 0.02f, damageDelay = 0.03f;
    private IEnumerator DelayedKnockback(Collider2D whatHitMe)
    {
        yield return new WaitForSeconds(knockbackDelay);
        if (whatHitMe != null && whatHitMe.gameObject.tag == "Attack")
        {
            var attackController = whatHitMe.GetComponentInParent<AttackController>();
            Vector3 knockback = attackController.CalculateEnemyKnockback();
            var rigidBody = gameObject.GetComponent<Rigidbody2D>();
            rigidBody.AddForce(knockback);
        } else if (whatHitMe != null && whatHitMe.gameObject.tag == "Arrow") {
            var arrowController = whatHitMe.GetComponent<ArrowController>();
            if (!arrowController.IsStuck()) {
                Vector3 knockback = arrowController.CalculateEnemyKnockback();
                var rigidBody = gameObject.GetComponent<Rigidbody2D>();
                rigidBody.AddForce(knockback);
            }
        }
    }

    private void CalculateDamage(Collider2D whatHitMe, bool isInitialDamage = false) {
        // TODO https://trello.com/c/gRC12RCf/211-use-player-attack-stat-for-sword-bow
        if (whatHitMe != null && whatHitMe.gameObject.tag == "Attack" && vulnerable == true)
        {
            var attackController = whatHitMe.GetComponentInParent<AttackController>();
            var damage = (int)attackController.CalculateEnemyDamage(isInitialDamage);
            if (damage > enemyDefence) {
                health -= damage; // TODO damage - enemyDefence?
                if (vulnerable == true) StartCoroutine(invulnerableTimer());
            }
            else damage = 0;
        } else if (whatHitMe != null && whatHitMe.gameObject.tag == "Arrow" && vulnerable == true) {
            var arrowController = whatHitMe.GetComponent<ArrowController>();
            if (!arrowController.IsStuck()) {
                var damage = (int)arrowController.CalculateEnemyDamage();
                arrowController.Destroy();
                if (damage > enemyDefence) {
                    health -= damage; // TODO damage - enemyDefence?
                    if (vulnerable == true) StartCoroutine(invulnerableTimer());
                }
            }
        }
    }

    private IEnumerator DelayedDamage(Collider2D whatHitMe, bool isInitialDamage = false) {
        yield return new WaitForSeconds(damageDelay);
        CalculateDamage(whatHitMe, isInitialDamage);
    }

    void OnTriggerEnter2D(Collider2D whatHitMe)
    {
        if (whatHitMe == null) return;
        StartCoroutine(DelayedDamage(whatHitMe, true));
        StartCoroutine(DelayedKnockback(whatHitMe));
    }

    void OnTriggerStay2D(Collider2D whatHitMe)
    {
        if (whatHitMe == null) return;
        CalculateDamage(whatHitMe);
    }

}
