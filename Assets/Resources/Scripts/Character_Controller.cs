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
    public (int, int) getStartingPosition() {
        // need to code this later
        startingPosition = (0, 0);
        currentPosition = startingPosition;
        Debug.Log($"Starting position set to: {startingPosition.Item1}, {startingPosition.Item2}");
        return startingPosition;
    }
    public void moveCharacter((int, int) newPosition) {
        // need to code this later
        Debug.Log($"Moving character to position: {newPosition.Item1}, {newPosition.Item2}");
        currentPosition = newPosition;
    }
    
    // public bool isCharacterInSlot((int, int) slotCoord) {
    //     bool hasMainCharacterSprite = false;
    //     foreach (Transform child in landform.slotOutline.transform) {
    //         SpriteRenderer spriteRenderer = child.GetComponent<SpriteRenderer>();
    //         if (spriteRenderer != null && spriteRenderer.sprite == SpriteLibrary.mainCharacterSprite) {
    //             hasMainCharacterSprite = true;
    //             break;
    //         }
    //     }
    //     if (!hasMainCharacterSprite) {
    //         return false; // No main character sprite found in the slot
    //     }
    //     else {
    //         OnMouseDown(); // Call the method to highlight reachable grids
    //         return hasMainCharacterSprite && currentPosition == slotCoord;
    //     }
    // }
    // private void OnMouseDown() {
    //     int maxTravelDistance = 3; // Example value, or pass as parameter
    //     HighlightReachableGrids(maxTravelDistance);
    // }

    public void HighlightReachableGrids(int maxTravelDistance) {
        List<(int, int)> reachable = new List<(int, int)>();
        for (int dx = -maxTravelDistance; dx <= maxTravelDistance; dx++) {
            for (int dy = -maxTravelDistance; dy <= maxTravelDistance; dy++) {
                int nx = currentPosition.Item1 + dx;
                int ny = currentPosition.Item2 + dy;
                if ((nx, ny) == currentPosition) continue; // skipping the current position that the player is in
                if (Mathf.Abs(dx) + Mathf.Abs(dy) <= maxTravelDistance) { // Manhattan distance
                    if (levelObject.hasSelectedSlot((nx, ny))) {
                        reachable.Add((nx, ny));
                        Debug.Log($"Reachable slot: ({nx}, {ny})");
                        // Highlighting the slot
                        Landform lf = levelObject.terrainInfo[(nx, ny)];
                        lf.colourSlot(movableOutlineColour, movableFillColour);
                    }
                }
            }
        }
    }
}