using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using PixelCrushers;


public class HealthBar : MonoBehaviour
{
    public GameObject health;
    public float healthMax, healthNew;
    public PlayerStats Player;
    public DAMAGE damage;

    private void Awake()
    {
        health = this.gameObject;
    }

    void OnEnable()
    {
        healthMax = Player.PlayerHP;       
        healthNew = damage.currentHealth;
    }

    void Update()
    {
        healthNew = damage.currentHealth;

        if (healthNew<0)
        {
            healthNew = 0;
        }

        else
        {
            health.transform.localScale = new Vector3((healthNew / healthMax), 1, 1);
        }       
    }
}
