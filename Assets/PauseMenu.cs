using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameisPause = false;
    public GameObject pauseMenuUI;
    public GameObject Player;
    public GameObject Surti;

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

    void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPause = false;
        Player.SetActive(false);
        Surti.SetActive(false);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPause = true;
        Player.SetActive(true);
        Surti.SetActive(true);
    }
}
