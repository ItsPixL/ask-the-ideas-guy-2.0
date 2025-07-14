using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    public void NextScene() { // going to the next level.
        Debug.Log("Next Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PreviousScene() { // going to the previous level.
        Debug.Log("Previous Scene");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
    public void QuitGame() { // closing the game
        Debug.Log("END Game");
        Application.Quit();
    }
    public void ResumeCurrentScene() { // restarting the level. make it resume the level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}