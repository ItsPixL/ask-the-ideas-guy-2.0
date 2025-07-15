using System.Collections.Generic;
using UnityEngine;
using Sprites;
using LevelManager;

public class Character_Controller : MonoBehaviour
{
    public Landform landform;
    public Level levelObject;
    public (int, int) currentPosition = (0, 0);
    public (int, int) startingPosition = (0, 0);
    private Color movableOutlineColour = new Color(0.5f, 0.1f, 0.1f, 1f); // the colours of the outline when the character can move
    private Color movableFillColour = new Color(0.0f, 0.75f, 0.87f, 0.65f); // the colours of the square when the character can move
    public Color outlineColour = new Color(0.8f, 0.1f, 0.1f, 1f);
    public Color clickedOutlineColor = new Color(0.94f, 0.86f, 0.2f, 1f);
    public Color fillColour = new Color(0.35f, 0.75f, 0.87f, 0.65f);
    private GameObject characterSpriteObj = null;
    public List<(int, int)> highlightedSlots = new List<(int, int)>();
    public int health = 60;
    public int xp = 0;
    public List<int> xpRequirements = new List<int> { 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };
    public int currentLevel = 1;
    public int playerTurns = 0; // used to give the player multiple turns before the monster turn. must keep in mind that the first moveCharacter is just placing the character down
    public int allowedTurns; // number of turns the player gets per phase
    public (int, int) getStartingPosition()
    {
        // need to code this later
        startingPosition = (0, 0);
        currentPosition = startingPosition;
        Debug.Log($"Starting position set to: {startingPosition.Item1}, {startingPosition.Item2}");
        return startingPosition;
    }
    public void moveCharacter((int, int) newPosition, float scaleX = 0.05f, float scaleY = 0.05f)
    {
        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.InGame ||
            GameStateManager.Instance.CurrentInGameSubState != GameStateManager.InGameSubState.PlayerTurn)
        {
            Debug.LogWarning("Cannot highlight reachable grids when not in PlayerTurn state.");
            return;
        }
        playerTurns += 1;
        // Destroy the old sprite if it exists
        if (characterSpriteObj != null)
        {
            Destroy(characterSpriteObj);
        }
        // moving the character visually
        characterSpriteObj = levelObject.PlaceSpriteInSlot(newPosition, SpriteLibrary.mainCharacterSprite, scaleX, scaleY);
        currentPosition = newPosition;
        landform = levelObject.terrainInfo[newPosition];
        // clear highlights after moving
        ClearHighlights();
        // Checks if the player's used all of his turns.
        checkTurnCount();
    }

    public void ClearHighlights()
    {
        foreach (var slot in highlightedSlots)
        {
            if (levelObject.isInField(slot))
            {
                Landform lf = levelObject.terrainInfo[slot];
                lf.colourSlot(outlineColour, fillColour);
            }
        }
        highlightedSlots.Clear();
    }

    public void HighlightReachableGrids(int maxTravelDistance)
    {
        ClearHighlights();
        highlightedSlots.Clear();

        if (GameStateManager.Instance.CurrentState != GameStateManager.GameState.InGame ||
            GameStateManager.Instance.CurrentInGameSubState != GameStateManager.InGameSubState.PlayerTurn)
        {
            Debug.LogWarning("Cannot highlight reachable grids when not in PlayerTurn state.");
            return;
        }

        HashSet<(int, int)> visited = new();
        Queue<((int, int) pos, int dist)> queue = new();

        queue.Enqueue((currentPosition, 0));
        visited.Add(currentPosition);

        while (queue.Count > 0)
        {
            var (pos, dist) = queue.Dequeue();
            int x = pos.Item1;
            int y = pos.Item2;

            if (dist > 0)
            {
                // Only highlight tiles that aren't the starting point
                Landform lf = levelObject.terrainInfo[pos];
                lf.colourSlot(movableOutlineColour, movableFillColour);
                highlightedSlots.Add(pos);
                Debug.Log($"Reachable slot: ({x}, {y})");
            }

            if (dist == maxTravelDistance)
                continue; // stop if we've reached max range

            // 4-directional movement: N, S, E, W
            var directions = new (int, int)[] { (0, 1), (1, 0), (0, -1), (-1, 0) };

            foreach (var (dx, dy) in directions)
            {
                var next = (x + dx, y + dy);
                if (visited.Contains(next)) continue;
                if (!levelObject.isInField(next)) continue;

                Landform lf = levelObject.terrainInfo[next];
                if (!lf.canTravelThrough) continue;

                queue.Enqueue((next, dist + 1));
                visited.Add(next);
            }
        }
    }


    // xp related stuff
    public void AddXP(int amount)
    {
        xp += amount;
        Debug.Log($"Added {amount} XP. Total XP: {xp}");
        CheckLevelUp();
    }

    private void CheckLevelUp()
    {
        int requiredXP = xpRequirements[currentLevel - 1]; // the current level starts at 1
        while (xp >= requiredXP)
        {
            xp -= requiredXP; // reseting the XP to zero after leveling up
            currentLevel++;
            health += 10; // Will have to create a dictionary if the amount of health gained per level is different
            if (currentLevel - 1 <= xpRequirements.Count)
            {
                requiredXP = xpRequirements[currentLevel - 1];
            }
            else
            {
                Debug.Log("No more levels left");
            }
            Debug.Log($"Level up! New level: {currentLevel}, Health: {health}, XP for next level: {requiredXP}");
        }
    }

    public void checkTurnCount()
    {
        // Debug.Log($"Player turns used: {playerTurns}");
        // Debug.Log($"Allowed turns: {allowedTurns}");
        if (playerTurns >= allowedTurns)
        {
            GameStateManager.Instance.SetInGameSubState(GameStateManager.InGameSubState.MonsterTurn);
            playerTurns = 0;
        }
    }
}