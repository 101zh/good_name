using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pause_menu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public static bool playerIsDead = false;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject deathScreenUI;

    public delegate void RetryButton();
    public static event RetryButton OnRetry;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsMenuUI.activeSelf && playerIsDead)
            {
                optionsMenuUI.SetActive(false);
                deathScreenUI.SetActive(true);
            }
            else if (gameIsPaused && optionsMenuUI.activeSelf)
            {
                optionsMenuUI.SetActive(false);
                pauseMenuUI.SetActive(true);
            }
            else if (gameIsPaused)
            {
                resume();
            }
            else
            {
                pause();
            }
        }
    }

    public void resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;
    }

    void pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;
    }

    public void Retry()
    {
        deathScreenUI.SetActive(false);
        DestroyAllProjectiles();
        playerIsDead = false;
        OnRetry?.Invoke();
    }

    private void PlayerDeath()
    {
        deathScreenUI.SetActive(true);
        playerIsDead = true;
    }

    public void BackToLastMenu()
    {
        if (playerIsDead)
        {
            deathScreenUI.SetActive(true);
            optionsMenuUI.SetActive(false);
        }
        else
        {
            pauseMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
        }
    }

    private void DestroyAllProjectiles()
    {
        GameObject[] projectiles = GameObject.FindGameObjectsWithTag("Projectile");
        for (int i = 0; i < projectiles.Length; i++)
        {
            Destroy(projectiles[i]);
        }
    }

    public void LoadTitleMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        playerIsDead=false;
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
