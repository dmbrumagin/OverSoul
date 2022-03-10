using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Threading.Tasks;

public class JumpController : MonoBehaviour
{
    // PUBLIC SETTINGS ********************************************************
    public float totalJumps = 2,
    jumpTime = 0.4f,
    jumpGracePeriod = 0.1f,
    initialJumpForce = 450,
    jumpForcePerFixedUpdate = 13,
    jumpPowerRolloffPerFixedUpdate = 0.0325f;


    // PRIVATE STATE **********************************************************
    private PlayerPlatformerController playerController;
    private Rigidbody2D playerRigidBody;
    private BowController bowController;
    private float jumpFixedUpdateCounter;
    private float fallingNoMovementThreshold = 0.001f;
    private float currentJumpCount = 0;
    private bool isJumping, isOnGround = false;


    // UNITY METHODS **********************************************************

    void Awake()
    {
        playerController = GetComponent<PlayerPlatformerController>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        bowController = GetComponent<BowController>();
    }

    public void Update()
    {
        UpdateIsOnGround();
    }

    public void FixedUpdate()
    {
        if (isJumping) AddHeldJumpForce();
    }


    // EVENT HANDLERS & HELPERS ***********************************************
    private DateTime? timeOfLastJump;
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && currentJumpCount < totalJumps) Jump();
        else if (context.performed) timeOfLastJump = DateTime.Now;
        else if (context.canceled) isJumping = false;
    }

    // PRIVATE METHODS ********************************************************
    private bool wasFallingLastFrame;
    private float noVerticalMovementForTime = 0;
    private void UpdateIsOnGround()
    {
        // TODO this can maybe be cleaned up to only use noVerticalMovementForTime threshold?
        // TODO sometimes can get 1 extra jump
        var noVerticalMovement = Math.Abs(playerRigidBody.velocity.y) < fallingNoMovementThreshold;
        if (noVerticalMovement) noVerticalMovementForTime += Time.deltaTime;
        else noVerticalMovementForTime = 0;
        // Debug.Log(new { noVerticalMovementForTime });
        if (wasFallingLastFrame && noVerticalMovement || noVerticalMovementForTime > .2f)
        {
            isOnGround = true;
            currentJumpCount = 0;

            if (timeOfLastJump != null)
            {
                var elapsedTime = DateTime.Now - (DateTime)timeOfLastJump;
                if (elapsedTime.TotalSeconds < jumpGracePeriod)
                {
                    Jump();
                    timeOfLastJump = null;
                }
            }
        }
        wasFallingLastFrame = playerController.IsFalling();
    }
    private void Jump()
    {
        currentJumpCount++;
        jumpFixedUpdateCounter = 0;
        playerRigidBody.AddForce(new Vector2(0, initialJumpForce));
        isJumping = true;
        isOnGround = false;
        JumpTimer();
    }
    private IEnumerator lastJumpTimer;
    private void JumpTimer()
    {
        if (lastJumpTimer != null) StopCoroutine(lastJumpTimer);
        lastJumpTimer = StopJumpAfterSomeTime(jumpTime);
        StartCoroutine(lastJumpTimer);
    }
    private IEnumerator StopJumpAfterSomeTime(float jumpTime)
    {
        yield return new WaitForSeconds(jumpTime);
        isJumping = false;
    }

    private void AddHeldJumpForce()
    {
        jumpFixedUpdateCounter++;
        var force = jumpForcePerFixedUpdate - jumpFixedUpdateCounter * jumpPowerRolloffPerFixedUpdate;

        var bowActiveMovementMultiplier = bowController.IsActive() ? bowController.GetBowMovementMultiplier() : 1;
        playerRigidBody.AddForce(new Vector2(0, force * bowActiveMovementMultiplier));
    }


    // PUBLIC METHODS *********************************************************
    public bool IsJumping()
    {
        return isJumping;
    }

    public bool IsOnGround()
    {
        return isOnGround;
    }

    public float GetJumpGracePeriod()
    {
        return jumpGracePeriod;
    }
}
