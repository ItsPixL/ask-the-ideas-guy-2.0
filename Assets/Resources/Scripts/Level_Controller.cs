using System.Collections.Generic;
using LevelManager;
using UnityEngine;
using Sprites;

public class Level_Controller : MonoBehaviour
{
    private int screenWidth;
    private int screenHeight;
    public int levelNum = 0;
    private int playerTurns;
    private int monsterTurns;
    private bool isPlayerTurn;
    private int turnsRemaining;
    private Level levelObject;
    private bool isSelected = false;
    private (int, int) prevSelectedSlot = (int.MinValue, int.MinValue);
    private Color defaultOutline = new Color(0.8f, 0.1f, 0.1f, 1f);
    private Color clickedOutline = new Color(0.94f, 0.86f, 0.2f, 1f); // Outline for the square that has been clicked on.
    private Color otherOutline; // Use this for other outlines (e.g. available movement spaces). 
    private Color fillColour = new Color(0.35f, 0.75f, 0.87f, 0.65f);
    private Dictionary<(int, int), Color> prevLandformsOutlined = new Dictionary<(int, int), Color>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        initLevelDetails();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != screenWidth || Screen.height != screenHeight)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            levelObject.scaleLandforms((2f, 3f));
        }
        if (Input.GetMouseButtonDown(0))
        {
            respondToMouse();
        }
    }

    public void respondToMouse()
    {
        (int, int) selectedSlot = levelObject.calculateClick(Input.mousePosition);
        Dictionary<(int, int), Color> currLandformsOutlined = new Dictionary<(int, int), Color>();
        bool isValid = levelObject.isInField(selectedSlot);
        foreach (var item in prevLandformsOutlined)
        {
            changeLandformColour(item.Key, defaultOutline, fillColour);
        }
        prevLandformsOutlined = new Dictionary<(int, int), Color>();
        if (isValid)
        {
            if (selectedSlot == prevSelectedSlot && isSelected)
            {
                isSelected = false;
            }
            else
            {
                currLandformsOutlined = new Dictionary<(int, int), Color>
                {
                    { selectedSlot, clickedOutline }
                };
                isSelected = true;
                prevLandformsOutlined = currLandformsOutlined;
            }
        }
        else
        {
            isSelected = false;
        }
        foreach (var item in currLandformsOutlined)
        {
            changeLandformColour(item.Key, item.Value, fillColour);
        }
        prevSelectedSlot = selectedSlot;
    }

    public void changeLandformColour((int, int) slotCoord, Color outlineColour, Color fillColour)
    {
        Landform currLandform = levelObject.terrainInfo[slotCoord];
        currLandform.colourSlot(outlineColour, fillColour);
    }

    public void initLevelDetails()
    {
        if (levelNum == 0) // Level 0 will be treated as testing grounds.
        {
            levelObject = new Level(8, 8, (32.5f, 7.5f), (60f, 85f), GameObject.Find("Main Camera"));
            // A test case to check that modifications work.
            Dictionary<string, List<(int, int)>> modifications = new Dictionary<string, List<(int, int)>>
            {
                { "none", new List<(int, int)> { (4, 7) }},
                {"wall", new List<(int, int)>{(1, 2), (2, 2), (3, 2), (4, 2)}}
            };
            levelObject.modifyLandforms(modifications);
            levelObject.designLandforms(new Color(0.8f, 0.1f, 0.1f, 1f), new Color(0.35f, 0.75f, 0.87f, 0.65f));
            levelObject.PlaceSpriteInSlot((3, 5), SpriteLibrary.squareSprite); // must place the sprite after the landforms are designed
            levelObject.PlaceSpriteInSlot((3, 6), SpriteLibrary.circleSprite);
            levelObject.PlaceSpriteInSlot((4, 5), SpriteLibrary.triangleSprite);
            levelObject.scaleLandforms((2f, 3f));
        }
    }
}
