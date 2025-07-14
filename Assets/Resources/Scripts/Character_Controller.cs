using System.Collections.Generic;
using UnityEngine;
using Sprites;
using LevelManager;
using UnityEngine.TextCore.Text;

public class Character_Controller : MonoBehaviour {
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
    public List<int> xpRequirements = new List<int> { 100, 250, 500, 900, 1400 };
    public int currentLevel = 1;
    public (int, int) getStartingPosition() {
        // need to code this later
        startingPosition = (0, 0);
        currentPosition = startingPosition;
        Debug.Log($"Starting position set to: {startingPosition.Item1}, {startingPosition.Item2}");
        return startingPosition;
    }
    public void moveCharacter((int, int) newPosition, float scaleX = 0.05f, float scaleY = 0.05f) {
        // Destroy the old sprite if it exists
        if (characterSpriteObj != null) {
            GameObject.Destroy(characterSpriteObj);
        }
        // moving the character visually
        characterSpriteObj = levelObject.PlaceSpriteInSlot(newPosition, SpriteLibrary.mainCharacterSprite, scaleX, scaleY);
        currentPosition = newPosition;
        landform = levelObject.terrainInfo[newPosition];
        // clear highlights after moving
        ClearHighlights();
    }
    
    public void ClearHighlights() {
        foreach (var slot in highlightedSlots) {
            if (levelObject.hasSelectedSlot(slot)) {
                Landform lf = levelObject.terrainInfo[slot];
                lf.colourSlot(outlineColour, fillColour);
            }
        }
        highlightedSlots.Clear();
    }

    public void HighlightReachableGrids(int maxTravelDistance) {
        ClearHighlights();
        highlightedSlots.Clear();
        for (int dx = -maxTravelDistance; dx <= maxTravelDistance; dx++) {
            for (int dy = -maxTravelDistance; dy <= maxTravelDistance; dy++) {
                int nx = currentPosition.Item1 + dx;
                int ny = currentPosition.Item2 + dy;
                if ((nx, ny) == currentPosition) continue; // skip current position
                if (Mathf.Abs(dx) + Mathf.Abs(dy) <= maxTravelDistance) { // Manhattan distance
                    if (levelObject.hasSelectedSlot((nx, ny))) {
                        Landform lf = levelObject.terrainInfo[(nx, ny)];
                        if (lf.isWall) continue; // skip walls
                        highlightedSlots.Add((nx, ny));
                        Debug.Log($"Reachable slot: ({nx}, {ny})");
                        lf.colourSlot(movableOutlineColour, movableFillColour);
                    }
                }
            }
        }
    }
    
    // xp related stuff
    public void AddXP(int amount) {
        xp += amount;
        Debug.Log($"Added {amount} XP. Total XP: {xp}");
        CheckLevelUp();
    }

    private void CheckLevelUp() {
        int requiredXP = xpRequirements[currentLevel - 1]; // the current level starts at 1
        while (xp >= requiredXP) {
            xp -= requiredXP; // reseting the XP to zero after leveling up
            currentLevel++;
            health += 10; // Will have to create a dictionary if the amount of health gained per level is different
            if (currentLevel - 1 <= xpRequirements.Count) {
                requiredXP = xpRequirements[currentLevel - 1];
            } else {
                Debug.Log("No more levels left");
            }
            Debug.Log($"Level up! New level: {currentLevel}, Health: {health}, XP for next level: {requiredXP}");
        }
    }
}