using UnityEngine;
using LevelManager;

public abstract class Weapon : MonoBehaviour {
    public string weaponName;
    public int damage;
    public Sprite weaponSprite;
    // highlighting logic
    private (int, int)? highlightedTile = null;
    private Level highlightedLevel = null;
    // normal colours
    public Color outlineColour = new Color(0.8f, 0.1f, 0.1f, 1f);
    public Color fillColour = new Color(0.35f, 0.75f, 0.87f, 0.65f);

    // Highlights the area of effect for this weapon
    public abstract void HighlightAOE((int, int) position, Level levelObject);

    // Applies the weapon's effect (e.g., damage) at the target position
    public abstract void Use((int, int) position, Level levelObject);

    // Spawns a weapon GameObject with the given sprite at the specified position
    public static T SpawnWeapon<T>(Vector3 worldPosition, Sprite sprite) where T : Weapon { // making it generic so that it can be used for any weapon type
        GameObject weaponGO = new GameObject(typeof(T).Name); // creating the gameobject
        T weapon = weaponGO.AddComponent<T>(); // adding the weapon script
        weapon.weaponSprite = sprite; // assigning the sprite to the weapon

        SpriteRenderer sr = weaponGO.AddComponent<SpriteRenderer>(); // adding the sprite renderer
        sr.sprite = sprite; // setting the sprite image

        weaponGO.AddComponent<BoxCollider2D>(); // adding a collider for mouse interaction

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
                var target = (player.currentPosition.Item1, player.currentPosition.Item2 + 1); // Same tile as Sword logic

                if (highlightedTile.HasValue && highlightedTile.Value == target) { // if it is already highlighted, unhighlight it
                    ClearHighlight();
                } else { // if it is not highlighted, highlight it
                    ClearHighlight(); // in case something else was highlighted
                    HighlightAOE(player.currentPosition, player.levelObject);
                    highlightedTile = target;
                    highlightedLevel = player.levelObject;
                }
            }
        }
    }
    private void ClearHighlight() {
        if (highlightedTile.HasValue && highlightedLevel != null) {
            var tile = highlightedTile.Value;
            if (highlightedLevel.isInField(tile)) {
                Landform lf = highlightedLevel.terrainInfo[tile];
                lf.colourSlot(outlineColour, fillColour); // Reset to normal (or use default)
            }
        }
        highlightedTile = null;
        highlightedLevel = null;
    }
}