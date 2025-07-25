using UnityEngine;
using LevelManager;
using MonsterManager;

public class Dagger : Weapon {
    public Dagger() {
        weaponName = "Dagger";
        damage = 2;
        range = 2;
    }

    // Highlights the tile north of the player (range 2, north)
    public override void HighlightAOE((int, int) position, Level levelObject) {
        int nx = position.Item1;
        int ny = position.Item2;

        for (int i = 1; i <= range; i++) {
            var target = (nx, ny + i);

            // Checking if tile is valid (in bounds and not a wall)
            if (!levelObject.isInField(target)) break;

            Landform lf = levelObject.terrainInfo[target];
            if (!lf.canTravelThrough) { // if the tile cannot be seen through, skip it
                Debug.Log($"Cannot attack through {lf.GetType().Name} at ({target.Item1}, {target.Item2})");
                break;  // Stop checking further tiles
            }
            
            if (levelObject.isInField(target)) {
                lf.colourSlot(Color.red, new Color(1f, 0.5f, 0.5f, 0.5f));

                // Add to base class tracking
                var weapon = this as Weapon;
                weapon?.TrackHighlight(target);
            }
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
        int px = position.Item1;
        int py = position.Item2;

        var monsterData = levelObject.returnStuffByType(typeof(Monster), typeof(MonsterSpawner));

        for (int i = 1; i <= range; i++) { // checking each tile in range
            var target = (px, py + i);

            // Checking if tile is valid (in bounds and not a wall)
            if (!levelObject.isInField(target)) break;

            Landform lf = levelObject.terrainInfo[target];
            if (!lf.canTravelThrough) { // if the tile cannot be seen through, skip it
                Debug.Log($"Cannot attack through {lf.GetType().Name} at ({target.Item1}, {target.Item2})");
                break;  // Stop checking further tiles
            }
            
            foreach (var (pos, monster, health) in monsterData) {
                // Debug.Log($"Thing: {monster.GetType().Name} at ({pos.Item1}, {pos.Item2}) has {health} HP");
                if (pos == target && monster is IHasHealth hasHealth) { // checking if monster is in the target position and has health
                    hasHealth.Health -= damage;
                    Debug.Log($"Dealt {damage} damage to {monster.GetType().Name} at ({pos.Item1}, {pos.Item2}). Remaining health: {hasHealth.Health}");
                    if (hasHealth.Health <= 0) { // replace with checking if monster died function when created
                        Debug.Log($"{monster.GetType().Name} at ({pos.Item1}, {pos.Item2}) has been defeated!");
                        // Remove the monster from the level
                    }
                    return; // Exit after applying damage
                }
            }
        }

        player.checkTurnCount();
    }
}