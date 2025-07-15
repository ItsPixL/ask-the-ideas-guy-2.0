using LevelManager;
using UnityEngine;
using MonsterManager;
using Sprites;
using System.Collections.Generic;

public class Level_Controller : MonoBehaviour
{
    private int screenWidth;
    private int screenHeight;
    public int levelNum = 0;
    private Level levelObject;
    private bool isSelected = false;
    private (int, int) prevSelectedSlot = (-1, -1);
    public Color outlineColour = new Color(0.8f, 0.1f, 0.1f, 1f);
    public Color clickedOutlineColor = new Color(0.94f, 0.86f, 0.2f, 1f);
    public Color fillColour = new Color(0.35f, 0.75f, 0.87f, 0.65f);
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

    public void respondPlayerPos((int, int) selectedSlot, bool isValid)
    {
        Landform prevLandform = null;
        if (levelObject.terrainInfo.TryGetValue(prevSelectedSlot, out Landform output))
        {
            prevLandform = output;
        }
        if (isValid)
        {
            Landform currLandform = levelObject.terrainInfo[selectedSlot];
            if (selectedSlot == prevSelectedSlot)
            {
                if (isSelected)
                {
                    currLandform.colourSlot(outlineColour, fillColour);
                    prevLandform.colourSlot(outlineColour, fillColour);
                }
                else
                {
                    currLandform.colourSlot(clickedOutlineColor, fillColour);
                }
                isSelected = !isSelected;
            }
            else
            {
                if (prevLandform != null)
                {
                    prevLandform.colourSlot(outlineColour, fillColour);
                }
                currLandform.colourSlot(clickedOutlineColor, fillColour);
                isSelected = true;
            }
        }
        else
        {
            if (prevLandform != null)
            {
                prevLandform.colourSlot(outlineColour, fillColour);
            }
            isSelected = false;
        }
        prevSelectedSlot = selectedSlot;
    }

    public void playerMovement((int, int) selectedSlot, bool isReachable)
    {
        if (isReachable)
        {
            levelObject.characterController.moveCharacter(selectedSlot);
            isSelected = false;
        }
        if (selectedSlot == levelObject.characterController.currentPosition && isSelected)
        {
            levelObject.characterController.HighlightReachableGrids(3);
        }
        else
        {
            levelObject.characterController.ClearHighlights();
        }
    }


    public void respondToMouse()
    {
        (int, int) selectedSlot = levelObject.calculateClick(Input.mousePosition);
        bool isValid = levelObject.isInField(selectedSlot);
        bool isReachable = levelObject.characterController.highlightedSlots.Contains(selectedSlot);
        respondPlayerPos(selectedSlot, isValid);
        playerMovement(selectedSlot, isReachable);
    }


    public void initLevelDetails() {
        if (levelNum == 0) { // Level 0 will be used as testing grounds.
            GameStateManager.Instance.SetState(GameStateManager.GameState.InGame);
            GameStateManager.Instance.SetInGameSubState(GameStateManager.InGameSubState.PlayerTurn);
            levelObject = new Level(8, 8, (32.5f, 7.5f), (60f, 85f), GameObject.Find("Main Camera"));
            GameObject characterGO = new GameObject("Player");
            characterGO.AddComponent<Character_Controller>();
            levelObject.characterController = characterGO.GetComponent<Character_Controller>();
            levelObject.characterController.allowedTurns = 5;
            // A test case to check that modifications work.
            /* Dictionary<string, List<(int, int)>> modifications = new Dictionary<string, List<(int, int)>>
            {
                { "none", new List<(int, int)> { (4, 7) } }
            };
            levelObject.modifyLandforms(modifications); */
            levelObject.designLandforms(new Color(0.8f, 0.1f, 0.1f, 1f), new Color(0.35f, 0.75f, 0.87f, 0.65f));
            // levelObject.PlaceSpriteInSlot((3, 5), SpriteLibrary.squareSprite); // must place the sprite after the landforms are designed
            // levelObject.PlaceSpriteInSlot((3, 6), SpriteLibrary.circleSprite);
            // levelObject.PlaceSpriteInSlot((4, 5), SpriteLibrary.triangleSprite);
            List<int> bruteBasicStats = new List<int>() { 25, 1, 16, 1, 3 };
            MonsterSpawner testSpawner = new MonsterSpawner(SpriteLibrary.spawnerSprite, (4, 7), "brute", bruteBasicStats, levelObject);
            testSpawner.displaySpawner();
            levelObject.scaleLandforms((2f, 3f));
            levelObject.characterController.levelObject = levelObject;
            (int, int) startingPosition = levelObject.characterController.getStartingPosition();
            levelObject.characterController.moveCharacter(startingPosition);
            levelObject.characterController.AddXP(100); // Adding some XP for testing

        }
    }
}