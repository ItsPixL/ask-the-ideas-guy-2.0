using UnityEngine;
using LevelManager;

public abstract class Weapon : MonoBehaviour {
    public string weaponName;
    public int damage;
    public Sprite weaponSprite;

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
            Use(player.currentPosition, player.levelObject);
        }
    }
    public virtual void OnMouseOver() { // Highlighting the area of effect if right clicked automatically
        if (Input.GetMouseButtonDown(1)) {
            var player = FindFirstObjectByType<Character_Controller>();
            if (player != null && player.levelObject != null) {
                HighlightAOE(player.currentPosition, player.levelObject);
            }
        }
    }
}