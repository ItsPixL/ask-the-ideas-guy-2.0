using System.Collections.Generic;
using UnityEngine;
using Sprites;

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

        public void changeSlotTransform((float, float) slotCenter, (float, float) slotLocalScale, (float, float) outlinePercent)
        {
            if (slotOutline && slotFill)
            {
                outlinePercent = (outlinePercent.Item1 / 100, outlinePercent.Item2 / 100);
                slotOutline.transform.position = new Vector3(slotCenter.Item1, slotCenter.Item2, 0);
                slotOutline.transform.localScale = new Vector3(slotLocalScale.Item1, slotLocalScale.Item2, 0);
                slotFill.transform.localScale = new Vector3(1 - outlinePercent.Item1, 1 - outlinePercent.Item2, 1);
            }
        }
    }

    public class Level
    {
        private int squaresX;
        private int squaresY;
        private Dictionary<(int, int), Landform> terrainInfo = new Dictionary<(int, int), Landform>();
        private GameObject cameraGameObject;
        private Camera levelCamera;

        public Level(int squaresX, int squaresY, GameObject cameraGameObject)
        {
            this.squaresX = squaresX;
            this.squaresY = squaresY;
            this.cameraGameObject = cameraGameObject;
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

        public void scaleLandforms((float, float) screenStart, (float, float) screenPercent, (float, float) outlinePercent)
        {
            screenStart = (screenStart.Item1 / 100f, screenStart.Item2 / 100f);
            screenPercent = (screenPercent.Item1 / 100f, screenPercent.Item2 / 100f);
            (float, float) percentPerSlot = (screenPercent.Item1 / squaresX, screenPercent.Item2 / squaresY);
            float screenLeft = levelCamera.transform.position.x - (levelCamera.orthographicSize * levelCamera.aspect);
            float screenBottom = levelCamera.transform.position.y - levelCamera.orthographicSize;
            float zoomFactor = cameraGameObject.GetComponent<Camera_Manager>().zoomFactor;
            foreach (var item in terrainInfo)
            {
                (int, int) currCoord = item.Key;
                Landform currLandform = item.Value;
                float screenPercentX = screenStart.Item1 + percentPerSlot.Item1 * (currCoord.Item1 + 0.5f);
                float screenPercentY = screenStart.Item2 + percentPerSlot.Item2 * (currCoord.Item2 + 0.5f);
                (float, float) slotCenter = (screenLeft + Screen.width / zoomFactor * screenPercentX, screenBottom + Screen.height / zoomFactor * screenPercentY);
                (float, float) slotSize = (Screen.width / zoomFactor * percentPerSlot.Item1, Screen.height / zoomFactor * percentPerSlot.Item2);
                currLandform.changeSlotTransform((slotCenter.Item1, slotCenter.Item2), (slotSize.Item1, slotSize.Item2), outlinePercent);
            }
        }

        public (int, int) calculateClick(Vector3 mousePos)
        {
            Vector3 worldPos = levelCamera.ScreenToWorldPoint(mousePos);
            Debug.Log(worldPos);
            return (0, 0);
        }
    }
}