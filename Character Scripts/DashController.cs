using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Threading.Tasks;

public class DashController : MonoBehaviour
{
    // PUBLIC SETTINGS ********************************************************
    public float totalDashes = 2,
    initialDashForce = 300,
    dashForcePerFixedUpdate = 12,
    verticalDashForceMultipler = 0.9f,
    dashTime = 0.5f,
    dashForceTime = 0.4f,
    dashInvulnerabilityTime = 0.45f,
    dashCooldownTime = 1.5f,
    dashPowerRolloffPerFixedUpdate = 0.15f,
    dashMovementTemporaryMass = 0.19f,
    dashCancelThreshold = 0.2f,
    dashCancelTime = 0.25f;


    // PRIVATE STATE **********************************************************
    private PlayerMainController playerController;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer playerSprite;
    private BowController bowController;
    private Vector2 movementInput, dashInput;
    private float defaultPlayerMass, jumpFixedUpdateCounter, dashFixedUpdateCounter;
    private float controllerNoMovementThreshold = 0.1f;
    private float currentDashCount = 0;
    private bool isDashing, isDashForce = false;


    // UNITY METHODS **********************************************************
    void Awake()
    {
        playerController = GetComponent<PlayerMainController>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        playerSprite = GetComponent<SpriteRenderer>();
        bowController = GetComponent<BowController>();
        defaultPlayerMass = playerRigidBody.mass;
    }

    public void FixedUpdate()
    {
        movementInput = playerController.GetMovementInput();
        if (isDashForce) AddHeldDashForce();
        if (isDashing) CheckDashCancel();
    }


    // EVENT HANDLERS & HELPERS ***********************************************
    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && currentDashCount < totalDashes)
        {
            currentDashCount++;
            dashFixedUpdateCounter = 0;
            dashInput = new Vector2(movementInput.x, movementInput.y);
            isDashing = true;
            isDashForce = true;
            playerRigidBody.mass = dashMovementTemporaryMass;
            playerSprite.color = new Color(1, 1, 1, 0.5f);

            var direction = movementInput.normalized;
            direction.y = verticalDashForceMultipler * direction.y;
            if (movementInput.magnitude < controllerNoMovementThreshold)
            {
                var leftRightDir = playerController.IsPlayerFacingRight() ? 1 : -1;
                direction = new Vector2(leftRightDir, 0);
                dashInput.x = leftRightDir * 0.75f;
            }
            playerRigidBody.AddForce(direction * initialDashForce);
            Physics2D.IgnoreLayerCollision(6, 7);
            DashTimer();
        }
    }


    // PUBLIC METHODS *********************************************************

    public bool IsDashing()
    {
        return isDashing;
    }

    public void RegisterDashDoneCallback(Action action)
    {
        dashDoneCallbacks.Add(action);
    }


    // PRIVATE METHODS ********************************************************

    private void CheckDashCancel()
    {
        var invertedNormalVelocity = -1 * playerRigidBody.velocity.normalized;

        if (invertedNormalVelocity.x < movementInput.x + dashCancelThreshold
        && invertedNormalVelocity.x > movementInput.x - dashCancelThreshold
        && invertedNormalVelocity.y < movementInput.y + dashCancelThreshold
        && invertedNormalVelocity.y > movementInput.y - dashCancelThreshold)
        {
            DashTimer(dashCancelTime);
        }
    }

    private IEnumerator lastDashTimer, lastDashForceTimer, lastEnableDashTimer;
    private void DashTimer(float dashCancelTime = 0f)
    {
        if (lastDashTimer != null)
        {
            StopCoroutine(lastDashTimer);
            lastDashTimer = null;

            StopCoroutine(lastDashForceTimer);
            lastDashForceTimer = null;
        }

        lastDashForceTimer = StopDashForceAfterSomeTime(dashCancelTime > 0 ? dashCancelTime : dashForceTime);
        StartCoroutine(lastDashForceTimer);

        lastDashTimer = StopDashAfterSomeTime(dashCancelTime > 0 ? dashCancelTime : dashForceTime);
        StartCoroutine(lastDashTimer);

        if (dashCancelTime == 0)
        {
            if (lastEnableDashTimer != null)
            {
                StopCoroutine(lastEnableDashTimer);
                lastEnableDashTimer = null;
            }

            lastEnableDashTimer = ResetDashesAfterSomeTime(dashCooldownTime);
            StartCoroutine(lastEnableDashTimer);
        }
    }

    private IEnumerator StopDashForceAfterSomeTime(float forceTime)
    {
        yield return new WaitForSeconds(dashForceTime);
        playerRigidBody.mass = defaultPlayerMass;
        isDashForce = false;
    }

    private IEnumerator StopDashAfterSomeTime(float dashTime)
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        Physics2D.IgnoreLayerCollision(6, 7, false);
        if (currentDashCount == totalDashes) playerSprite.color = new Color(0.6f, 0.7f, 1, 1);
        else playerSprite.color = new Color(1, 1, 1, 1);
        CallAndRemoveDashDoneCallbacks();
    }

    private IEnumerator ResetDashesAfterSomeTime(float dashCooldown)
    {
        yield return new WaitForSeconds(dashCooldown);
        currentDashCount = 0;
        playerSprite.color = new Color(1, 1, 1, 1);
    }

    private void AddHeldDashForce()
    {
        dashFixedUpdateCounter++;

        // get average between player movement input and physics
        // (dashInput + playerRigidBody.velocity).normalized;

        var direction = dashInput.normalized;
        direction.y = verticalDashForceMultipler * direction.y;
        var force = dashForcePerFixedUpdate - (dashFixedUpdateCounter * dashPowerRolloffPerFixedUpdate);
        var bowActiveMovementMultiplier = bowController.IsActive() ? bowController.GetBowMovementMultiplier() : 1;
        if (force < 0f) return;
        else playerRigidBody.AddForce(direction * force * bowActiveMovementMultiplier);
    }

    private List<Action> dashDoneCallbacks = new List<Action>();
    private void CallAndRemoveDashDoneCallbacks()
    {
        dashDoneCallbacks.ForEach(cb => cb());
        dashDoneCallbacks = new List<Action>();
    }
}
