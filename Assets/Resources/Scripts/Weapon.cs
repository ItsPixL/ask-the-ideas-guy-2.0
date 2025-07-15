using UnityEngine;
using LevelManager;
using System.Collections.Generic;

public abstract class Weapon : MonoBehaviour {
    public string weaponName;
    public int damage;
    public int range;
    public Sprite weaponSprite;
    // highlighting logic
    private List<(int, int)> highlightedTiles = new();
    private Level highlightedLevel = null;
    // normal colours
    public Color outlineColour = new Color(0.8f, 0.1f, 0.1f, 1f);
    public Color fillColour = new Color(0.35f, 0.75f, 0.87f, 0.65f);

    // Highlights the area of effect for this weapon
    public abstract void HighlightAOE((int, int) position, Level levelObject);

    // Applies the weapon's effect (e.g., damage) at the target position
    public abstract void Use((int, int) position, Level levelObject);

    // Spawns a weapon GameObject with the given sprite at the specified position
    public static T SpawnWeapon<T>(Vector3 worldPosition, Sprite sprite, float scaleX = 0.5f, float scaleY = 0.5f) where T : Weapon { // making it generic so that it can be used for any weapon type
        GameObject weaponGO = new GameObject(typeof(T).Name); // creating the gameobject
        T weapon = weaponGO.AddComponent<T>(); // adding the weapon script
        weapon.weaponSprite = sprite; // assigning the sprite to the weapon

        SpriteRenderer sr = weaponGO.AddComponent<SpriteRenderer>(); // adding the sprite renderer
        sr.sprite = sprite; // setting the sprite image

        weaponGO.AddComponent<BoxCollider2D>(); // adding a collider for mouse interaction

        weaponGO.transform.localScale = new Vector3(scaleX, scaleY, 1f); // scaling the weapons

        weaponGO.transform.position = worldPosition; // placing the gameobject in the world
        return weapon;
    }
    public virtual void OnMouseDown() { // Using the weapon if left clicked automatically
        // You need to use the player position and levelObject
        var player = FindFirstObjectByType<Character_Controller>();
        if (player != null && player.levelObject != null) {
            ClearHighlight();
            Use(player.currentPosition, player.levelObject);
        }
    }
    public virtual void OnMouseOver() { // Highlighting the area of effect if right clicked automatically
        if (Input.GetMouseButtonDown(1)) {
            var player = FindFirstObjectByType<Character_Controller>();
            if (player != null && player.levelObject != null) {
                if (highlightedTiles.Count > 0) { // if there are highlighted tiles, clear them
                    ClearHighlight();
                } else { // if it is not highlighted, highlight it
                    ClearHighlight(); // in case something else was highlighted
                    HighlightAOE(player.currentPosition, player.levelObject);
                    highlightedLevel = player.levelObject;
                }
            }
        }
    }
    private void ClearHighlight() {
        if (highlightedLevel != null) {
            foreach (var tile in highlightedTiles) {
                if (highlightedLevel.isInField(tile)) {
                    Landform lf = highlightedLevel.terrainInfo[tile];
                    lf.colourSlot(outlineColour, fillColour); // Reset tile appearance
                }
            }
        }

        highlightedTiles.Clear();
        highlightedLevel = null;
    }
    public void TrackHighlight((int, int) tile) {
        highlightedTiles.Add(tile);
    }
}