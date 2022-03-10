using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Threading.Tasks;

public class PlayerPlatformerController : MonoBehaviour
{
    // PUBLIC SETTINGS ********************************************************
    public float fallingMovementPerFixedUpdate = 0.75f * 13,
    changeDirectionMovementMultipler = 5f;



    // PRIVATE STATE **********************************************************
    protected Rigidbody2D playerRigidBody;
    private PlayerMainController mainController;
    private BowController bowController;
    private JumpController jumpController;
    protected DashController dashController;
    
    protected float controllerNoMovementThreshold = 0.1f;
    protected float playerMinimumSpeed = 1f;
    protected float currentJumpCount, currentDashCount = 0;
    protected bool isPlayerFacingRight = true;
    private float fallingNoMovementThreshold = 0.001f;


    // UNITY METHODS **********************************************************
    public void Awake()
    {
        mainController = GetComponent<PlayerMainController>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        bowController = GetComponent<BowController>();
        jumpController = GetComponent<JumpController>();
        dashController = GetComponent<DashController>();
    }

    public void OnEnable() {
        Debug.Log("ENABLED COMBAT");
        bowController.enabled = true;
        jumpController.enabled = true;
    }

    public void OnDisable() {
        Debug.Log("DISABLED COMBAT");
        bowController.enabled = false;
        jumpController.enabled = false;
    }

    public void FixedUpdate()
    {
        var isDashing = dashController.IsDashing();
        if (!isDashing && mainController.IsMoving()) AddMovementForce();
        if (!isDashing && IsFalling()) AddFastFallForce();
    }


    // HELPER METHODS *********************************************************

    protected void AddMovementForce()
    {
        var movementInput = mainController.GetMovementInput();
        var movement = mainController.moveForcePerFixedUpdate * new Vector2(movementInput.x, 0);
        if (mainController.IsPlayerChangingDirection()) {
            playerRigidBody.AddForce(changeDirectionMovementMultipler * movement);
        } else {
            var bowActiveMovementMultiplier = bowController.IsActive() ? bowController.GetBowMovementMultiplier() : 1;
            playerRigidBody.AddForce(1 * movement * bowActiveMovementMultiplier);
        }
    }

    private void AddFastFallForce()
    {
        if (fallingMovementPerFixedUpdate > 0)
        {
            var jumpForceVector = new Vector2(0, -1 * fallingMovementPerFixedUpdate);
            playerRigidBody.AddForce(jumpForceVector);
        }
    }


    // PUBLIC METHODS *********************************************************

    public void AddAgentMovementForce(float xDirection){
        var movement = mainController.moveForcePerFixedUpdate * new Vector2(xDirection, 0);
            playerRigidBody.AddForce(changeDirectionMovementMultipler * movement);
    }

    public bool IsFalling()
    {
        return playerRigidBody.velocity.y < 0
        && Math.Abs(playerRigidBody.velocity.y) > fallingNoMovementThreshold;
    }
}
