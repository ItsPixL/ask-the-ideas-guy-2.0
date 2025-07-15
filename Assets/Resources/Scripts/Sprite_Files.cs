using UnityEngine;

namespace Sprites
{
    public static class SpriteLibrary
    {
        public static Sprite squareSprite = Resources.Load<Sprite>("Sprites/Square");
        public static Sprite circleSprite = Resources.Load<Sprite>("Sprites/Circle");
        public static Sprite triangleSprite = Resources.Load<Sprite>("Sprites/Triangle");
        public static Sprite wallSprite = Resources.Load<Sprite>("Sprites/Circle"); // To be changed
        public static Sprite bruteSprite = Resources.Load<Sprite>("Sprites/Triangle"); // To be changed
        public static Sprite mainCharacterSprite = Resources.Load<Sprite>("Sprites/Main Character (before the volcano)");
        public static Sprite swordSprite = Resources.Load<Sprite>("Sprites/swordSprite"); // The weapon will not be displayed in the grids, but in the canvas
        public static Sprite spawnerSprite = Resources.Load<Sprite>("Sprites/Circle"); // To be changed
    }
}