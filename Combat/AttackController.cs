using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AttackController : MonoBehaviour
{
    // PUBLIC SETTINGS ********************************************************
    public float attackWidth, attackLength, attackGracePeriod,
    attackDistanceFromCenter, attackStartRotation, attackMaxRotation,
    knockbackFromDamage = 100f, knockbackBaseForce = 100f,
    initialDamageMultiplier = 20f, damageMultiplier = 10f,
    maxDamage = 50f, minDamage = 1f;


    // PRIVATE STATE **********************************************************
    private bool isSwinging = false;
    private PlayerMainController playerController;
    private Transform attackTriggerTransform;

    private Quaternion startRotation, maxRotation;
    private float maxRotationTime = 0.4f; // TODO verify
    private float enemyKnockbackVerticalRatio = 0.1f;


    // UNITY METHODS **********************************************************

    public void Awake()
    {
        playerController = gameObject.GetComponentInParent<PlayerMainController>();
        attackTriggerTransform = gameObject.GetComponentInChildren<BoxCollider2D>().transform;
        attackTriggerTransform.localScale.Set(attackWidth, attackLength, 0);
        attackTriggerTransform.Translate(attackDistanceFromCenter, 0, 0);

        InitializeAttackSwingQuaternions();
    }

    private Quaternion currentRotation;
    private float currentRotationTime = 0;
    public void Update()
    {
        CheckGracePeriodAttack();
        if (isSwinging) {
            PositionAttackObject();

            if (currentRotationTime >= maxRotationTime) {
                isSwinging = false;
                currentRotationTime = 0;
                currentRotation = startRotation;
            } else {
                currentRotationTime += Time.deltaTime;
                currentRotation = Quaternion.Lerp(currentRotation, maxRotation, maxRotationTime);
            }
        }
    }


    // PRIVATE HELPER METHODS *************************************************

    private void InitializeAttackSwingQuaternions()
    {
        var x = gameObject.transform.rotation.eulerAngles.x;
        var y = gameObject.transform.rotation.eulerAngles.y;
        startRotation = Quaternion.Euler(x, y, attackStartRotation);
        maxRotation = Quaternion.Euler(x, y, attackMaxRotation);
    }

    DateTime? timeOfLastAttack;
    private void CheckGracePeriodAttack() {
        if (timeOfLastAttack != null) {
            var elapsedTime = DateTime.Now - (DateTime)timeOfLastAttack;
            if (elapsedTime.TotalSeconds < attackGracePeriod) {
                isSwinging = true;
                timeOfLastAttack = null;
            }
        }
    }

    private void PositionAttackObject() {
        var isRight = playerController.IsPlayerFacingRight();

        // move attack object left or right depending on character direction
        if (attackTriggerTransform.localPosition.x < 0 && isRight
        || attackTriggerTransform.localPosition.x > 0 && !isRight) {
            var direction = isRight ? 1 : -1;
            attackTriggerTransform.Translate(direction * 2 * attackDistanceFromCenter, 0, 0);
        }
    }


    // PUBLIC METHODS *********************************************************

    public bool IsSwinging() {
        return isSwinging;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        // TODO find other attack code (enable trigger?) and move here
        if (context.performed) {
            isSwinging = true;
        }
    }

    public Vector3 CalculateEnemyKnockback() {
        var enemyKnockback = knockbackBaseForce + CalculateEnemyDamage() / maxDamage * knockbackFromDamage;
        var direction = playerController.IsPlayerFacingRight() ? 1 : -1;
        var knockLeftRight = direction * enemyKnockback;
        var knockDown = -1 * enemyKnockback * enemyKnockbackVerticalRatio;
        return new Vector3(knockLeftRight, knockDown, 0);
    }

    public float CalculateEnemyDamage(bool isInitialDamage = false)
    {
        // increase damage as swing ends
        var multiplier = isInitialDamage ? initialDamageMultiplier : damageMultiplier;
        var damage = multiplier * currentRotationTime / maxRotationTime;
        if (damage < minDamage) damage = minDamage;
        else if (damage > maxDamage) damage = maxDamage;
        return damage;
    }
}
