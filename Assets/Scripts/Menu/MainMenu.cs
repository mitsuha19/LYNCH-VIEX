using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public SceneTransition sceneTransition;
    public string sceneName = "Game";

    public void PlayGame()
    {
        sceneTransition.FadeToScene(sceneName);    
         
    }

    public void LoadGame()
    { 

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

