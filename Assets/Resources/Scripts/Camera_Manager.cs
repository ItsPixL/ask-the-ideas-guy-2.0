using UnityEngine;
using UnityEngine.PlayerLoop;

public class Camera_Manager : MonoBehaviour
{
    private int prevScreenWidth = Screen.width;
    private int prevScreenHeight = Screen.height;
    private int screenWidth;
    private int screenHeight;
    private Camera gameCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenWidth = prevScreenWidth;
        screenHeight = prevScreenHeight;
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        changeCameraSize();
    }

    // Update is called once per frame
    void Update()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        if (screenWidth != prevScreenWidth || screenHeight != prevScreenHeight)
        {
            changeCameraSize();
        }
    }

    private void changeCameraSize()
    {
        float orthographicSize = screenHeight / 2f;
        float aspectRatio = (float)screenWidth / screenHeight;
        gameCamera.orthographicSize = orthographicSize;
        gameCamera.aspect = aspectRatio;
    }
}
