using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;
using System.Linq;

public class ProjectileReticleController : MonoBehaviour
{
    public float reticleDistance;
    private SpriteRenderer reticleSprite;
    private BowController bowController;
    private PlayerMainController playerController;
    private Color startColor = new Color(0, 0.5f, 1, 1);
    private float timeLeft;
    private void Start()
    {
        reticleSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        var player = GameObject.FindGameObjectWithTag("Player");
        bowController = player.GetComponent<BowController>();
        playerController = player.GetComponent<PlayerMainController>();
        reticleSprite.color = new Color(0, 255, 255, 1);
        timeLeft = bowController.nockTime + bowController.chargeTime;
    }

    private bool wasActiveLastFrame;
    private void Update()
    {
        if (bowController.enabled && bowController.IsActive()) {
            wasActiveLastFrame = true;
            reticleSprite.enabled = true;

            var bowRatio = bowController.GetBowAngleRatioVsMax();
            var isFacingRight = playerController.IsPlayerFacingRight();
            var xDirection = isFacingRight ? 1 : -1;
            var yDirection = bowController.GetBowAngleRatioVsMax();

            var playerPosition = playerController.GetPosition();

            var maxAngleRadians = Mathf.Deg2Rad * bowController.maxAngle;
            var xPosition = xDirection * Mathf.Cos(maxAngleRadians) * reticleDistance;
            var yPosition = yDirection * Mathf.Sin(maxAngleRadians) * reticleDistance;

            transform.position = playerPosition + new Vector3(xPosition, yPosition, transform.position.z);
            var targetColor = new Color(1, 1, 1, 1);
            reticleSprite.color = Color.Lerp(reticleSprite.color, targetColor, Time.deltaTime / timeLeft);
            timeLeft -= Time.deltaTime;
        } else if (wasActiveLastFrame) {
            reticleSprite.enabled = false;
            reticleSprite.color = startColor;
            timeLeft = bowController.nockTime + bowController.chargeTime; // get max charge time from bowController
        }
    }


}
