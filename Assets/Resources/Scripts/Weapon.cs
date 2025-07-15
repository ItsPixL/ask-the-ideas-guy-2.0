using UnityEngine;
using LevelManager;

public abstract class Weapon : MonoBehaviour {
    public string weaponName;
    public int damage;

    // Highlights the area of effect for this weapon
    public abstract void HighlightAOE((int, int) position, Level levelObject);

    // Applies the weapon's effect (e.g., damage) at the target position
    public abstract void Use((int, int) position, Level levelObject);
}