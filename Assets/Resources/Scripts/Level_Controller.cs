using System.Collections.Generic;
using LevelManager;
using UnityEngine;

public class Level_Controller : MonoBehaviour
{
    private int screenWidth;
    private int screenHeight;
    public int levelNum = 0;
    public Level levelObject;
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
            (int, int) slotClicked = levelObject.calculateClick(Input.mousePosition);
            Debug.Log(slotClicked);
        }
    }

    public void initLevelDetails()
    {
        if (levelNum == 1)
        {
            levelObject = new Level(8, 8, (32.5f, 7.5f), (60f, 85f), GameObject.Find("Main Camera"));
            Dictionary<string, List<(int, int)>> modifications = new Dictionary<string, List<(int, int)>>
            {
                { "none", new List<(int, int)> { (4, 7) } }
            };
            levelObject.modifyLandforms(modifications);
            levelObject.designLandforms(new Color(0.8f, 0.1f, 0.1f, 1f), new Color(0.35f, 0.75f, 0.87f, 0.65f));
            levelObject.scaleLandforms((2f, 3f));
        }
    }
}
