using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPause = false;
    public GameObject pauseMenuUI;
    public FirstPersonController playerController;
    public GameObject Surti;
    public GameObject aimDot;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) )
        {
            if (GameisPause)
            {
                Resume();
            }
            else {
                Pause();
            }

        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPause = false;
        playerController.enabled = true;
        Surti.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        aimDot.SetActive(true);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPause = true;
        playerController.enabled = false;
        Surti.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        aimDot.SetActive(false);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit(); 
    }
}
