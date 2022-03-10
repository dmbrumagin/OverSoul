using UnityEngine;
using System.Collections;

public class CrowPeckAttack : EnemyAttack
{
    public float force;
    private new Rigidbody2D rigidbody;
    // private new Transform transform;
    private Vector2 direction;

    void Awake() {
        rigidbody = GetComponent<Rigidbody2D>();
        // transform = GetComponent<Transform>();
    }

    public override IEnumerator Attack() {
        isAttacking = true;
        isReady = false;
        
        Debug.Log("PECK ATTACK");
        rigidbody.AddForce(force * direction); // * transform.localPosition); // TODO verify localPosition will get left/right
        yield return new WaitForSeconds(attackDuration);
        Debug.Log("PECK DONE");
        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        isReady = true;
        Debug.Log("DIVE READY");
    }

    public IEnumerator Attack(Vector2 direction) {
        this.direction = direction;
        return Attack();
    }
}