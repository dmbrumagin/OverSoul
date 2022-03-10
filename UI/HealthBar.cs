using UnityEngine;
using Player;


public class HealthBar : MonoBehaviour
{
    private GameObject health;
    private float healthMax, healthNew;
    private PlayerStats PlayerStats;
    private GameObject player;
    private PlayerDamageHandler damage;

    private void Awake()
    {
        health = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats = player.GetComponent<PlayerStats>();
        damage = player.GetComponent<PlayerDamageHandler>();
    }

    void OnEnable()
    {
        healthMax = PlayerStats.PlayerHP;       
        healthNew = damage.currentHealth;
    }

    void Update()
    {
        healthNew = damage.currentHealth;

        if (healthNew<0)
            healthNew = 0;

        health.transform.localScale = new Vector3((healthNew / healthMax), 1, 1);           
    }
}
