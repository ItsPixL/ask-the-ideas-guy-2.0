using UnityEngine;

public class Camera_Manager : MonoBehaviour
{
    private int screenWidth;
    private int screenHeight;
    private Camera gameCamera;
    public float zoomFactor = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        gameCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        changeCameraSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.width != screenWidth || Screen.height != screenHeight)
        {
            screenWidth = Screen.width;
            screenHeight = Screen.height;
            changeCameraSize();
        }
    }

    private void changeCameraSize()
    {
        float orthographicSize = screenHeight / (zoomFactor*2f);
        float aspectRatio = (float)screenWidth / screenHeight;
        gameCamera.orthographicSize = orthographicSize;
        gameCamera.aspect = aspectRatio;
    }
}
