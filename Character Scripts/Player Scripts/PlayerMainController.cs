using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using System.Threading.Tasks;

public class PlayerMainController : MonoBehaviour
{
    // PUBLIC SETTINGS ********************************************************
    public float moveForcePerFixedUpdate = 7.5f;

    // PRIVATE STATE **********************************************************
    private Rigidbody2D playerRigidBody;
    private Vector2 movementInput;
    private float controllerNoMovementThreshold = 0.1f;
    private float playerMinimumSpeed = 1f;
    private bool isMoving;
    private bool isPlayerFacingRight = true;


    // UNITY METHODS **********************************************************
    public void Awake()
    {
        playerRigidBody = GetComponent<Rigidbody2D>();
    }

    public void FixedUpdate()
    {
        UpdateIsPlayerFacingRight();
        StopSlowPlayerFromSlipping();
    }


    // EVENT HANDLERS & HELPERS ***********************************************

    public void Move(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        if (context.performed) isMoving = movementInput.magnitude > controllerNoMovementThreshold;
        else if (context.canceled) isMoving = false;
    }


    // PRIVATE METHODS ********************************************************

    private void UpdateIsPlayerFacingRight()
    {
        var wasPlayerFacingRight = isPlayerFacingRight;
        if (!isPlayerFacingRight && movementInput.x > controllerNoMovementThreshold) {
            isPlayerFacingRight = true;
        } else if (isPlayerFacingRight && movementInput.x < (-1 * controllerNoMovementThreshold)) {
            isPlayerFacingRight = false;
        }

        if (wasPlayerFacingRight != isPlayerFacingRight) {
            StartCoroutine(TemporarilyIncreaseNoMovementThreshold(5));
        }
    }

    protected IEnumerator TemporarilyIncreaseNoMovementThreshold(int multiplier)
    {
        var originalThreshold = controllerNoMovementThreshold + 0;
        controllerNoMovementThreshold = originalThreshold * multiplier;
        yield return new WaitForSeconds(.1f);
        controllerNoMovementThreshold = originalThreshold;
    }

    private void StopSlowPlayerFromSlipping()
    {
        if (movementInput.x < controllerNoMovementThreshold
        && playerRigidBody.velocity.magnitude < playerMinimumSpeed) {
            playerRigidBody.AddForce(new Vector2(-1 * playerRigidBody.velocity.x, 0));
        }
    }


    // PUBLIC METHODS *********************************************************

    public bool IsPlayerChangingDirection()
    {
        var isChangingDirection = playerRigidBody.velocity.x > 0 && movementInput.x < 0
            || playerRigidBody.velocity.x < 0 && movementInput.x > 0;
        return isChangingDirection;
    }

    public bool IsPlayerFacingRight()
    {
        return isPlayerFacingRight;
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public bool IsMoving()
    {
        return isMoving;
    }

    public Vector3 GetMovementInput()
    {
        return movementInput;
    }
}
