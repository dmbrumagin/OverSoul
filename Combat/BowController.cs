using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Player;

public enum BowState
{
    InActive,
    Nocking, // won't fire until complete
    Charging, // won't have full power until complete
    Charged,
    Firing // can't nock until complete (should automatically nock if player kept button held from here though)
}

public class BowController : MonoBehaviour
{

    // PUBLIC SETTINGS ********************************************************

    public GameObject arrow;
    public float launchForce, upwardForce;
    public Transform arrowSpawnPointRight, arrowSpawnPointLeft;
    public float nockTime, chargeTime, shootTime /* TODO change to fireTime */,
    baseDamage, velocityDamageMultiplier, maxAngle, aimSensitivity,
    bowMovementMultiplier = 0.5f;


    // PRIVATE STATE **********************************************************
    
    private BowState bowState;
    private bool isButtonHeld, shouldFireWhenReady, isAiming = false;
    private float controllerNoMovementThreshold = 0.1f;
    private Vector2 aimInput;
    private float bowAngle = 0;
    private PlayerMainController playerController;
    private BowState[] readyStates = { BowState.Charging, BowState.Charged };
    private IEnumerator nockingAndChargingAction;
    private float currentChargeTime = 0;


    // UNITY METHODS **********************************************************

    private void Awake() {
        playerController = GetComponent<PlayerMainController>();
    }

    private void Update()
    {
        
        if (isAiming) AdjustBowAngle();

        // TODO helper method
        if (bowState == BowState.Charging) {
            currentChargeTime += Time.deltaTime;
        } else if (bowState == BowState.Charged) {
            currentChargeTime = chargeTime;
        } else {
            currentChargeTime = 0;
        }

        var arrowShouldFire = shouldFireWhenReady && IsReady();
        if (arrowShouldFire) {
            StopNockingAndCharging();
            StartCoroutine(FireArrow());
        }
        
    }


    // EVENT METHODS **********************************************************

    public void Shoot(InputAction.CallbackContext context)
    {
        var arrowCount = PlayerStats.StatTypeToPlayerStat[StatType.Arrow].getAmount();
        if (arrowCount > 0) {
            if (context.performed) {
                isButtonHeld = true;
                if (bowState == BowState.InActive) StartNockingAndCharging();
            } else if (context.canceled) {
                if (bowState != BowState.InActive) shouldFireWhenReady = true;
                isButtonHeld = false;
            }
        } else {
            // TODO play sound and visually indicate unable to fire
            //   (probably flash arrow count on screen when we have that displayed)
        }
    }

    public void Aim(InputAction.CallbackContext context)
    {
        aimInput = context.ReadValue<Vector2>();
        if (context.performed) {
            isAiming = aimInput.magnitude > controllerNoMovementThreshold;
        } else if (context.canceled) {
            isAiming = false;
        }
    }


    // PUBLIC METHODS *********************************************************

    public bool IsReady() {
        return readyStates.Contains(bowState);
    }

    public bool IsActive() {
        return bowState != BowState.InActive;
    }

    public BowState GetBowState() {
        return bowState;
    }

    public float GetBowAngleRatioVsMax() {
        return bowAngle / maxAngle;
    }

    public float GetChargePercent() {
        return currentChargeTime / chargeTime;
    }

    public float GetBowMovementMultiplier() {
        return bowMovementMultiplier;
    }


    // PRIVATE METHODS ********************************************************

    private void AdjustBowAngle()
    {
        bowAngle += aimInput.y * aimSensitivity * Time.deltaTime;
        if (bowAngle > maxAngle) bowAngle = maxAngle;
        else if (bowAngle < -1 * maxAngle) bowAngle = -1 * maxAngle;
    }

    private void StartNockingAndCharging() {
        nockingAndChargingAction = NockAndCharge();
        StartCoroutine(nockingAndChargingAction);
    }
    private void StopNockingAndCharging() {
        if (nockingAndChargingAction != null) StopCoroutine(nockingAndChargingAction);
        nockingAndChargingAction = null;
    }
    
    
    private IEnumerator NockAndCharge()
    {
        bowState = BowState.Nocking;
        yield return new WaitForSeconds(nockTime);
        bowState = BowState.Charging;
        yield return new WaitForSeconds(chargeTime);
        bowState = BowState.Charged;
    }

    private IEnumerator FireArrow() {
        bowState = BowState.Firing;
        shouldFireWhenReady = false;

        var arrows = PlayerStats.StatTypeToPlayerStat[StatType.Arrow];
        arrows.setAmount(arrows.getAmount()-1);
        
        var isRight = playerController.IsPlayerFacingRight();
        var arrowSpawnPoint = isRight ? arrowSpawnPointRight : arrowSpawnPointLeft;
        var newArrow = Instantiate(arrow, arrowSpawnPoint.position, arrowSpawnPoint.rotation);

        var arrowController = newArrow.GetComponent<ArrowController>();
        arrowController.SetDamage(baseDamage, velocityDamageMultiplier);
        arrowController.SetDirection(isRight);
        
        if (!isRight) newArrow.transform.eulerAngles += 180f * Vector3.up;
        var direction = isRight ? 1 : -1;
        var bowAngleRadians = Mathf.Deg2Rad * bowAngle;
        var baseForce = launchForce * (chargeTime / 2 + currentChargeTime / 2) / chargeTime;
        var yForce = upwardForce + baseForce * Mathf.Sin(bowAngleRadians);
        var arrowForce = new Vector2(direction * baseForce, yForce);
        newArrow.GetComponent<Rigidbody2D>().AddForce(arrowForce);

        yield return new WaitForSeconds(shootTime);

        if (isButtonHeld && arrows.getAmount() > 0) StartNockingAndCharging(); // immediately charge a follow-up shot
        else bowState = BowState.InActive;

        bowAngle = 0; // TODO try disabling this? but then also reticle should always be controllable/visible (just maybe translucent)
    }
}
