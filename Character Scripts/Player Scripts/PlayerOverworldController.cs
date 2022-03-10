using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Threading.Tasks;

public class PlayerOverworldController : MonoBehaviour
{

    // PUBLIC SETTINGS ********************************************************
    public float verticalMovementMultiplier = 0.75f;


    // PRIVATE STATE **********************************************************

    private Rigidbody2D playerRigidBody;
    private PlayerMainController mainController;
    private DashController dashController;
    private BowController bowController;
    private float controllerNoMovementThreshold = 0.1f;
    private float playerMinimumSpeed = 1f;
    private float currentJumpCount, currentDashCount = 0;
    private bool isPlayerFacingRight = true;
    private float defaultPlayerGravity;


    // UNITY METHODS **********************************************************
    public void Awake()
    {
        mainController = GetComponent<PlayerMainController>();
        playerRigidBody = GetComponent<Rigidbody2D>();
        // bowController = GetComponent<BowController>();
        // jumpController = GetComponent<JumpController>();
        dashController = GetComponent<DashController>();
        defaultPlayerGravity = playerRigidBody.gravityScale;
    }

    public void OnEnable() {
        Debug.Log("ENABLED OVERWORLD CTRL");
        playerRigidBody.gravityScale = 0;
    }

    public void OnDisable() {
        Debug.Log("DISABLED OVERWORLD CTRL");
        playerRigidBody.gravityScale = defaultPlayerGravity;
    }

    public void FixedUpdate()
    {
        if (!dashController.IsDashing() && mainController.IsMoving()) {
            AddMovementForce();
        }
    }


    // HELPER METHODS *********************************************************

    protected void AddMovementForce()
    {
        var movementInput = mainController.GetMovementInput();
        var movement = new Vector2(movementInput.x, movementInput.y * verticalMovementMultiplier);
        movement = mainController.moveForcePerFixedUpdate * movement;
        playerRigidBody.AddForce(1 * movement);
    }
}
