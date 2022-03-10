using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class ArrowController : MonoBehaviour
{
    public float knockbackBaseForce = 100f;
    private float baseDamage, velocityDamageMultiplier;
    private Rigidbody2D rigidBody;
    private bool isFacingRight = true, isStuck = false;

    private void Awake() {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    
    public void SetDamage(float baseDamage, float velocityDamageMultiplier) {
        this.baseDamage = baseDamage;
        this.velocityDamageMultiplier = velocityDamageMultiplier;
    }

    public bool IsStuck() {
        return isStuck;
    }

    public void SetDirection(bool isRight) {
        isFacingRight = isRight;
    }

    public float CalculateEnemyDamage() {
        // TODO https://trello.com/c/gRC12RCf/211-use-player-attack-stat-for-sword-bow
        // NOTE: rigidBody.velocity.magnitude tends to be 5-10
        var damage = baseDamage + rigidBody.velocity.magnitude * velocityDamageMultiplier;
        Debug.Log(new { damage, vel = rigidBody.velocity.magnitude });
        return damage;
    }

    private float enemyKnockbackVerticalRatio = 0.1f;
    public Vector3 CalculateEnemyKnockback() {
        var enemyKnockback = knockbackBaseForce + CalculateEnemyDamage();
        var direction = isFacingRight ? 1 : -1;
        var knockLeftRight = direction * enemyKnockback;
        var knockDown = -1 * enemyKnockback * enemyKnockbackVerticalRatio;
        return new Vector3(knockLeftRight, knockDown, 0);
    }

    public void Destroy() {
        Destroy(gameObject, 0.05f); // TODO fix race condition
    }

    void OnCollisionEnter2D(Collision2D whatHitMe)
    {
        if (whatHitMe != null && whatHitMe.gameObject.tag != "Arrow") {
            var collider = GetComponent<BoxCollider2D>();
            if (whatHitMe.gameObject.tag != "Player") {
                StickArrow();
            }
        }
    }

    private void StickArrow() {
        isStuck = true;
        Destroy(GetComponent<Rigidbody2D>());

        var collider = GetComponent<BoxCollider2D>();
        collider.isTrigger = true;

        var physicalCollider = this.transform.GetChild(0).GetComponent<BoxCollider2D>();
        physicalCollider.isTrigger = true;
    }

    void OnCollisionStay2D(Collision2D whatHitMe) {

    }
}
