using Sounds;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Player;
using LevelManager;
using PixelCrushers.DialogueSystem.Demo.Wrappers;
using InventoryRelated;

public class PlayerControlManager : MonoBehaviour {
    public enum PlayerControlType { Platformer, Overworld }
    public void Awake () {
        var sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "platformer") SetPlayerControls(PlayerControlType.Platformer);
        else if (sceneName == "1") SetPlayerControls(PlayerControlType.Overworld);
        else if (sceneName == "shop") SetPlayerControls(PlayerControlType.Overworld);
    }

    private void SetPlayerControls(PlayerControlType controlType) {
        Debug.Log(new { msg = "SWITCHING", controlType });
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player)
        {
            var playerPlatformerController = player.GetComponent<PlayerPlatformerController>();
            var playerOverworldController = player.GetComponent<PlayerOverworldController>();
            if (controlType == PlayerControlType.Platformer)
            {
                playerPlatformerController.enabled = true;
                playerOverworldController.enabled = false;
            }
            else if (controlType == PlayerControlType.Overworld)
            {
                playerPlatformerController.enabled = false;
                playerOverworldController.enabled = true;
            }
        }
        else
        {
            Debug.LogError("Failed to locate player during awake");
        }
    }
}