using UnityEngine;
using System.Collections;

public class CrowDiveAttack : EnemyAttack
{
    public float force;
    public float temporaryGravity = 2f;
    private new Rigidbody2D rigidbody;
    private float defaultGravity;
    private Vector2 direction;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        defaultGravity = rigidbody.gravityScale + 0;
    }
    
    public override IEnumerator Attack()
    {
        isAttacking = true;
        isReady = false;
        Debug.Log("DIVE ATTACK");

        rigidbody.gravityScale = temporaryGravity; // TODO verify

        // TODO update numbers so it works, cleanup
        //   gravity should handle y force

        rigidbody.AddForce(force * direction); // TODO verify localPosition will get left/right
        yield return new WaitForSeconds(attackDuration / 4);

        rigidbody.AddForce(force * direction);
        yield return new WaitForSeconds(attackDuration / 4);

        rigidbody.AddForce(force * direction);
        yield return new WaitForSeconds(attackDuration / 4);

        rigidbody.AddForce(force * direction);
        yield return new WaitForSeconds(attackDuration / 4);

        rigidbody.gravityScale = defaultGravity;

        var recoveryDirection = new Vector2(direction.x * -1, 1).normalized;
        rigidbody.AddForce(force * recoveryDirection);

        Debug.Log("DIVE DONE");
        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        isReady = true;
        Debug.Log("DIVE READY");
    }

    public IEnumerator Attack(Vector2 direction) {
        // TODO scaled y down so gravity does some of the work
        this.direction = new Vector2(direction.x, direction.y / 6).normalized;
        return Attack();
    }

}