using System.Collections.Generic;
using UnityEngine;
using Sprites;
using Unity.Burst;
using Unity.Mathematics;

namespace LevelManager
{
    public class Landform
    {
        public bool isWall;
        public Sprite spriteOnTop = null;
        private GameObject slotOutline = null;
        private SpriteRenderer outlineRenderer = null;
        private GameObject slotFill = null;
        private SpriteRenderer fillRenderer = null;

        public Landform(bool isWall)
        {
            this.isWall = isWall;
        }

        public void designSlot(Sprite sprite)
        {
            slotOutline = new GameObject("Slot Outline");
            outlineRenderer = slotOutline.AddComponent<SpriteRenderer>();
            outlineRenderer.sprite = sprite;
            outlineRenderer.sortingOrder = 1;
            slotFill = new GameObject("Slot Fill");
            slotFill.transform.parent = slotOutline.transform;
            fillRenderer = slotFill.AddComponent<SpriteRenderer>();
            fillRenderer.sprite = sprite;
            fillRenderer.sortingOrder = 2;
        }

        public void colourSlot(Color outlineColour, Color fillColour)
        {
            if (outlineRenderer && fillRenderer)
            {
                outlineRenderer.color = outlineColour;
                fillRenderer.color = fillColour;
            }
        }

        public void changeSlotTransform(Vector2 slotCenter, (float, float) slotLocalScale, (float, float) outlinePercent)
        {
            if (slotOutline && slotFill)
            {
                outlinePercent = (outlinePercent.Item1 / 100, outlinePercent.Item2 / 100);
                slotOutline.transform.position = new Vector3(slotCenter.x, slotCenter.y, 0);
                slotOutline.transform.localScale = new Vector3(slotLocalScale.Item1, slotLocalScale.Item2, 0);
                slotFill.transform.localScale = new Vector3(1 - outlinePercent.Item1, 1 - outlinePercent.Item2, 1);
            }
        }
    }

    public class Level
    {
        private int squaresX;
        private int squaresY;
        private (float, float) screenStart;
        private (float, float) screenPercent;
        private (float, float) percentPerSlot;
        private Dictionary<(int, int), Landform> terrainInfo = new Dictionary<(int, int), Landform>();
        private GameObject cameraGameObject;
        private Camera levelCamera;

        public Level(int squaresX, int squaresY, (float, float) screenStart, (float, float) screenPercent, GameObject cameraGameObject)
        {
            this.squaresX = squaresX;
            this.squaresY = squaresY;
            this.screenStart = (screenStart.Item1/100f, screenStart.Item2/100f);
            this.screenPercent = (screenPercent.Item1/100f, screenPercent.Item2/100f);
            this.cameraGameObject = cameraGameObject;
            percentPerSlot = (this.screenPercent.Item1 / squaresX, this.screenPercent.Item2 / squaresY);
            levelCamera = cameraGameObject.GetComponent<Camera>();
            initLandforms();
        }

        private void initLandforms()
        {
            for (int i = 0; i < squaresX; i++)
            {
                for (int j = 0; j < squaresY; j++)
                {
                    (int, int) currCoord = (i, j);
                    terrainInfo.Add(currCoord, new Landform(false));
                }
            }
        }
        public void modifyLandforms(Dictionary<string, List<(int, int)>> modifications)
        {
            foreach (var item in modifications)
            {
                string landformType = item.Key;
                if (landformType == "wall")
                {
                    foreach ((int, int) coord in item.Value)
                    {
                        terrainInfo[coord].isWall = true;
                    }
                }
                else if (landformType == "none")
                {
                    foreach ((int, int) coord in item.Value)
                    {
                        terrainInfo.Remove(coord);
                    }
                }

            }
        }

        public void designLandforms(Color outlineColour, Color fillColour)
        {
            foreach (var item in terrainInfo)
            {
                Landform currLandform = item.Value;
                currLandform.designSlot(SpriteLibrary.squareSprite);
                currLandform.colourSlot(outlineColour, fillColour);
            }
        }

        public void scaleLandforms((float, float) outlinePercent)
        {
            float screenLeft = levelCamera.transform.position.x - (levelCamera.orthographicSize * levelCamera.aspect);
            float screenBottom = levelCamera.transform.position.y - levelCamera.orthographicSize;
            float zoomFactor = cameraGameObject.GetComponent<Camera_Manager>().zoomFactor;
            foreach (var item in terrainInfo)
            {
                (int, int) currCoord = item.Key;
                Landform currLandform = item.Value;
                float screenPercentX = screenStart.Item1 + percentPerSlot.Item1 * (currCoord.Item1 + 0.5f);
                float screenPercentY = screenStart.Item2 + percentPerSlot.Item2 * (currCoord.Item2 + 0.5f);
                Vector2 slotCenter = new Vector2(screenLeft + Screen.width / zoomFactor * screenPercentX, screenBottom + Screen.height / zoomFactor * screenPercentY);
                (float, float) slotSize = (Screen.width / zoomFactor * percentPerSlot.Item1, Screen.height / zoomFactor * percentPerSlot.Item2);
                currLandform.changeSlotTransform(slotCenter, (slotSize.Item1, slotSize.Item2), outlinePercent);
            }
        }

        public (int, int) calculateClick(Vector3 mousePos)
        {
            Vector2 worldPos = (Vector2)levelCamera.ScreenToWorldPoint(mousePos);
            float distX = worldPos.x - (levelCamera.transform.position.x - (levelCamera.orthographicSize * levelCamera.aspect));
            float decimalX = distX / (levelCamera.orthographicSize * 2 * levelCamera.aspect);
            float distY = worldPos.y - (levelCamera.transform.position.y - levelCamera.orthographicSize);
            float decimalY = distY / (levelCamera.orthographicSize * 2);
            int slotCoordX = (int)math.floor((decimalX-screenStart.Item1)/percentPerSlot.Item1);
            int slotCoordY = (int)math.floor((decimalY-screenStart.Item2)/percentPerSlot.Item2);
            return (slotCoordX, slotCoordY);
        }
    }
}