using Sounds;
using System.Collections;
using UnityEngine;
using Player;

public class PlayerDamageHandler : MonoBehaviour {
    
    private Rigidbody2D body;
    private Collision2D whatHitMe;
    [SerializeField]
    public float currentHealth;
    private bool vulnerable=true;
    private PlayerStats playerStats;
    private SpriteRenderer spritechar;
    private SoundPlayer soundPlayer;
    private DashController dashController;
    private BoxCollider2D playerCollider;

    public static PlayerDamageHandler playerDamage;

    void Awake()
    {
        playerDamage=this;
        dashController = GetComponent<DashController>();
        playerCollider = GetComponent<BoxCollider2D>();
        playerStats = GetComponent<PlayerStats>();
        spritechar = GetComponent<SpriteRenderer>();
        soundPlayer = GameObject.FindGameObjectWithTag("SoundController").GetComponent<SoundPlayer>();
        body = GetComponent<Rigidbody2D>();
        currentHealth = playerStats.PlayerHP;
    }
    

    private Color lastColor;
    IEnumerator invulnerableTimer()
    {
        vulnerable = false;
        lastColor = spritechar.color;
        spritechar.color = new Color(1, 0, 0, 1);
        yield return new WaitForSeconds(.3f);
        if (spritechar.color.Equals(new Color(1, 0, 0, 1))) {
            // don't set to last color if it changed
            spritechar.color = new Color(1, 1, 1, .5f);
        }
        yield return new WaitForSeconds(.2f);
         spritechar.color = new Color(1, 1, 1, 1);
         yield return new WaitForSeconds(.2f);
         spritechar.color = new Color(1, 1, 1, .5f);
         yield return new WaitForSeconds(.2f);
         spritechar.color = new Color(1, 1, 1, 1);
         yield return new WaitForSeconds(.2f);
        vulnerable = true;
    }

    public void Damaged(float enemyDamage)
    {
        //TODO reset attributes
        Debug.Log("enemy attack: " + enemyDamage+ " Player Vitality: "
        + (PlayerStats.StatTypeToPlayerStat[StatType.Vitality].getAmount() / 10)+" Player Defence: "
        + PlayerStats.StatTypeToPlayerStat[StatType.Defence].getAmount() + " Calculated Value: "
        +((PlayerStats.StatTypeToPlayerStat[StatType.Vitality].getAmount() / 10) 
        * PlayerStats.StatTypeToPlayerStat[StatType.Defence].getAmount())
        + PlayerStats.StatTypeToPlayerStat[StatType.Defence].getAmount());


        // TODO https://trello.com/c/jNqZ5VMl/214-cleanup-attributeslist-and-related-code
        if (enemyDamage > (((PlayerStats.StatTypeToPlayerStat[StatType.Vitality].getAmount() / 10) * PlayerStats.StatTypeToPlayerStat[StatType.Defence].getAmount())
         + PlayerStats.StatTypeToPlayerStat[StatType.Defence].getAmount()))
        {
            soundPlayer.Play("PlayerDamaged");
            
            var damage = enemyDamage -  (((PlayerStats.StatTypeToPlayerStat[StatType.Vitality].getAmount() / 10) * PlayerStats.StatTypeToPlayerStat[StatType.Defence].getAmount()) + PlayerStats.StatTypeToPlayerStat[StatType.Defence].getAmount());
            currentHealth = currentHealth - damage;

            if (vulnerable == true)
            {
                StartCoroutine(invulnerableTimer());
            }
        }
    }

    void OnCollisionEnter2D(Collision2D whatHitMe)
    {
        var enemyCollider = whatHitMe.gameObject.GetComponent<BoxCollider2D>();
        var isDashing = dashController.IsDashing();
        // Debug.Log(new { isDashing, hit = whatHitMe.gameObject.tag, whatHitMe });
        if (isDashing && whatHitMe.gameObject.tag == "Enemy") {
            Physics2D.IgnoreCollision(enemyCollider, playerCollider);
            dashController.RegisterDashDoneCallback(() => {
                Physics2D.IgnoreCollision(enemyCollider, playerCollider, false);
            });
        } else if (whatHitMe.gameObject.tag == "Enemy" && vulnerable == true) {
            // TODO should be in enemyAttack script
            var enemyDamage = whatHitMe.gameObject.GetComponent<EnemyDamageHandler>();
            Damaged(enemyDamage.attack);
        }
    }

    void OnCollisionStay2D(Collision2D whatHitMe)
    {
        if (whatHitMe.gameObject.tag == "Enemy" && vulnerable == true) {
            // TODO should be in enemyAttack script
            var enemyDamage = whatHitMe.gameObject.GetComponent<EnemyDamageHandler>();
            Damaged(enemyDamage.attack);
        }
    }


}
