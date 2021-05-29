using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject health;
    public float healthMax;
    public float healthNew;  
    public DamageEnemy damage;

    void OnEnable()
    {
        healthMax =damage.currentHealth;
        health = this.gameObject;
        healthNew = damage.currentHealth;
    }
    
    void Update()
    {
        healthNew = damage.currentHealth;
        health.transform.localScale = new Vector3((healthNew / healthMax), 1, 1);
    }
   
}
