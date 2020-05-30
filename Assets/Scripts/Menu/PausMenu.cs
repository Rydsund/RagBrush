using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 
/// /Johan
/// </summary>
public class PausMenu : MonoBehaviour
{

    public static bool gamePaused = false;

    [SerializeField]
    private GameObject pausMenuUI;

    [SerializeField]
    private GameObject optionsMenuUI;

    [SerializeField]
    private GameObject controlMenueUI;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused)
            {
                if(pausMenuUI.activeSelf == true)
                {
                    Resume();
                }
                else if(optionsMenuUI.activeSelf == true)
                {                    
                    pausMenuUI.SetActive(true);
                    optionsMenuUI.SetActive(false);
                }
                else if(controlMenueUI.activeSelf == true)
                {
                    optionsMenuUI.SetActive(true);
                    controlMenueUI.SetActive(false);
                }
            }
            else
            {
                Paus();
            }
        }
    }

    /// <summary>
    /// Resumes gameplay
    /// Johan
    /// </summary>
    public void Resume()
    {
        pausMenuUI.SetActive(false);
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        gamePaused = false;
    }

    /// <summary>
    /// Pauses gameplay
    /// Johan
    /// </summary>
    void Paus()
    {
        pausMenuUI.SetActive(true);
        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        gamePaused = true;
    }

    /// <summary>
    /// Exits to menu
    /// Johan
    /// </summary>
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
