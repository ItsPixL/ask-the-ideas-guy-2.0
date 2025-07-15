using UnityEngine;
using LevelManager;

public class Sword : Weapon {
    public Sword() {
        weaponName = "Sword";
        damage = 4;
    }

    // Highlights the tile north of the player (range 1, north)
    public override void HighlightAOE((int, int) position, Level levelObject) {
        int nx = position.Item1;
        int ny = position.Item2 + 1; // North is +1 in y
        var target = (nx, ny);

        if (levelObject.isInField(target)) {
            Landform lf = levelObject.terrainInfo[target];
            lf.colourSlot(Color.red, new Color(1f, 0.5f, 0.5f, 0.5f));
        }
    }

    // Deals damage to the tile north of the player
    public override void Use((int, int) position, Level levelObject) {
        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.InGame ||
            GameStateManager.Instance.CurrentInGameSubState != GameStateManager.InGameSubState.PlayerTurn) {
            Debug.LogWarning("Cannot attack when not in PlayerTurn state.");
            return;
        }
        var player = FindFirstObjectByType<Character_Controller>();
        player.playerTurns += 1;
        int nx = position.Item1;
        int ny = position.Item2 + 1;
        var target = (nx, ny);

        // Example: If there's an enemy at target, deal damage
        // You'd need to implement enemy lookup and damage logic here
        Debug.Log($"Sword used! Dealing {damage} damage to ({nx}, {ny})");
        player.checkTurnCount();
    }
}