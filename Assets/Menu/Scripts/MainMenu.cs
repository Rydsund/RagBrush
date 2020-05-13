using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour //Johan
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        //SceneManager.UnloadScene(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
    
    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
