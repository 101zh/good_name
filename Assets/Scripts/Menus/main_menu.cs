using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class main_menu : MonoBehaviour
{
    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    public void LoadScene3(){
        Time.timeScale = 1f;
        SceneManager.LoadScene(2, LoadSceneMode.Single);
    }

    public void SkipTheTutorial()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("Quit Button Pressed");
        Application.Quit();
    }


}
