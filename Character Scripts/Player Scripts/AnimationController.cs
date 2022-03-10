using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Animator playerAnimator;
    public Animator attackAnimator;
    private PlayerMainController playerController;
    private PlayerOverworldController playerOverworldController;
    private DashController dashController;
    private AttackController attackController;
    private BowController bowController;
    private const float movementThreshold = 0.1f;

    private enum PlayerAnimatorLayers { Stand, Run, Attack, Bow }

    public void Start()
    {
        playerController = GetComponent<PlayerMainController>();
        playerOverworldController = GetComponent<PlayerOverworldController>();
        dashController = GetComponent<DashController>();
        attackController = GetComponentInChildren<AttackController>();
        bowController = GetComponent<BowController>();
    }
    public void Update()
    {
        Animate();
    }

    private void Animate()
    {
        SetDirection();

        if (attackController.IsSwinging()) {
            SetPlayerAnimation(PlayerAnimatorLayers.Attack);
            attackAnimator.gameObject.SetActive(true);
        } else {
            attackAnimator.gameObject.SetActive(false);
            if (bowController.enabled && bowController.IsActive()) {
                SetPlayerAnimation(PlayerAnimatorLayers.Bow);

                if (bowController.IsReady()) playerAnimator.speed = 0;
                else playerAnimator.speed = 1;

            } else if (playerController.IsMoving() || dashController.IsDashing()) {
                SetPlayerAnimation(PlayerAnimatorLayers.Run);
            } else {
                SetPlayerAnimation(PlayerAnimatorLayers.Stand);
            }
        }
    }

    private void SetDirection() {
        if (playerOverworldController.enabled) {
            playerAnimator.SetFloat("y", playerController.GetMovementInput().y);
        }
        var isRight = playerController.IsPlayerFacingRight();
        playerAnimator.SetFloat("x", isRight ? 1 : -1);
        attackAnimator.SetFloat("x", isRight ? 1 : -1);
    }

    private void SetPlayerAnimation(PlayerAnimatorLayers layer) {

        playerAnimator.SetLayerWeight(0, 0);
        playerAnimator.SetLayerWeight(1, 0);
        playerAnimator.SetLayerWeight(2, 0);
        playerAnimator.SetLayerWeight(3, 0);

        if (layer == PlayerAnimatorLayers.Stand) {
            playerAnimator.SetLayerWeight(0, 1);
        } else if (layer == PlayerAnimatorLayers.Run) {
            playerAnimator.SetLayerWeight(1, 1);
        } else if (layer == PlayerAnimatorLayers.Attack) {
            playerAnimator.SetLayerWeight(2, 1);
        } else if (layer == PlayerAnimatorLayers.Bow) {
            playerAnimator.SetLayerWeight(3, 1);
        }
    }
}
