using UnityEngine;
using System.Collections;

public class SlimeLungeAttack : EnemyAttack
{

    public override IEnumerator Attack()
    {
        isAttacking = true;
        // rigidbody.AddForce(force * attackVector * transform.localPosition); // TODO verify localPosition will get left/right
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }
}