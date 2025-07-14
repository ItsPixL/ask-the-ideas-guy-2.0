using UnityEngine;

namespace Sprites
{
    public static class SpriteLibrary
    {
        public static Sprite squareSprite = Resources.Load<Sprite>("Sprites/Square");
        public static Sprite circleSprite = Resources.Load<Sprite>("Sprites/Circle");
        public static Sprite triangleSprite = Resources.Load<Sprite>("Sprites/Triangle");
        public static Sprite mainCharacterSprite = Resources.Load<Sprite>("Sprites/Main Character (before the volcano)");
    }
}