using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class LifeDeathAndTaxes : MonoBehaviour
{
    public string RandomChoice() {
        var rand = new System.Random();
        int index = rand.Next(3);
        if (index == 0) {
            return "Life";
        }
        else if (index == 1) {
            return "Death";
        }
        else {
            return "Taxes";
        }
    }

    public string SelectWinner(string player1, string player2) {
        /*
        Life ignores death
        Taxes ruin life
        Death settles all taxes
        */
        if (
            player1 != "Life" && player1 != "Death" && player1 != "Taxes" ||
            player2 != "Life" && player2 != "Death" && player2 != "Taxes") {
            Debug.LogWarning($"Invalid choice(s): {player1} vs {player2}");
            return "draw";
        }
        if (player1 == player2) {
            return "draw";
        } else if (
            player1 == "Life" && player2 == "Death" ||
            player1 == "Death" && player2 == "Taxes" ||
            player1 == "Taxes" && player2 == "Life") {
            return "player1";
        } else {
            return "player2";
        }
    }

    public string GetWinLossReason(string choice1, string choice2) {
        if (choice1 == "Life" && choice2 == "Death" || choice1 == "Death" && choice2 == "Life") {
            return "Life ignores Death";
        } else if (choice1 == "Death" && choice2 == "Taxes" || choice1 == "Taxes" && choice2 == "Death") {
            return "Death settles all taxes";
        } else /* if (choice1 == "Taxes" && choice2 == "Life" || choice1 == "Life" && choice2 == "Taxes") */ {
            return "Taxes ruin life";
        }
    }

    void OnEnable() {
        Lua.RegisterFunction("RandomChoice", this, SymbolExtensions.GetMethodInfo(() => RandomChoice()));
        Lua.RegisterFunction("SelectWinner", this, SymbolExtensions.GetMethodInfo(() => SelectWinner("", "")));
        Lua.RegisterFunction("GetWinLossReason", this, SymbolExtensions.GetMethodInfo(() => GetWinLossReason("", "")));
    }

    void OnDisable() {
        Lua.UnregisterFunction("RandomChoice");
        Lua.UnregisterFunction("SelectWinner");
        Lua.UnregisterFunction("GetWinLossReason");
    }
}
