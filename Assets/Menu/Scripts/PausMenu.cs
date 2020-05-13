using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausMenu : MonoBehaviour
{

    public static bool gamePaused = false;

    public GameObject pausMenuUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                Resume();
            }
            else
            {
                Paus();
            }
        }
    }

    public void Resume()
    {
        pausMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gamePaused = false;
    }

    void Paus()
    {
        pausMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gamePaused = true;
    }


    public void ExitToMenu()
    {
        pausMenuUI.SetActive(false);
        gamePaused = false;
        //SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
