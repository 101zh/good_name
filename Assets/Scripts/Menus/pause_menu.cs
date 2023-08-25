using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause_menu : MonoBehaviour
{
    public static bool gamePaused = false;
    public static bool playerDead = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject deathScreenUI;

    public delegate void Retry();
    public static event Retry OnRetry;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !deathScreenUI.activeSelf)
        {
            if (gamePaused && optionsMenuUI.activeSelf)
            {
                optionsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else if (gamePaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
        else
        {
            if (optionsMenuUI.activeSelf)
            {
                optionsMenuUI.SetActive(false);
                deathScreenUI.SetActive(true);
            }
        }
    }

    public void resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gamePaused = false;
    }

    void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gamePaused = true;
    }

    void RetryWave()
    {
        OnRetry?.Invoke();
    }

    private void PlayerDeath()
    {
        deathScreenUI.SetActive(true);
    }

    public void LoadMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }

    private void OnEnable()
    {
        Health.OnPlayerDie += PlayerDeath;
    }

    private void OnDisable()
    {
        Health.OnPlayerDie -= PlayerDeath;
    }
}
