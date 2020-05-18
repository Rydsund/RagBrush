using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// 
/// /Johan
/// </summary>
public class MainMenu : MonoBehaviour
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
