using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    Snake snake;
    GameObject[] pauseObjects;
    GameObject[] finishObjects;

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("ShowOnPause");
        finishObjects = GameObject.FindGameObjectsWithTag("ShowOnFinish");
        HidePaused();
        HideFinished();

        if (SceneManager.GetActiveScene().name == "MainGame")
        {
            snake = GameObject.FindGameObjectWithTag("Player").GetComponent<Snake>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // If player is dead, end the game
        if (Time.timeScale == 1 && snake != null && !snake.IsAlive())
        {
            FinishGame();
        }

        // Use the 'p' button to pause and unpause the game
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePaused();
        }

        // Use the 's' button to save the game
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }

        // Use the 'l' button to load the game
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }

        // Use the 'r' button to rewind the game
        if (Input.GetKeyDown(KeyCode.R))
        {
            RewindGame();
        }
    }

    public void TogglePaused()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ShowPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HidePaused();
        }
    }

    public void ShowPaused()
    {
        foreach (GameObject gameObject in pauseObjects)
        {
            gameObject.SetActive(true);
        }
    }

    public void HidePaused()
    {
        foreach (GameObject gameObject in pauseObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public void FinishGame()
    {
        Time.timeScale = 0;
        ShowFinished();
    }

    public void ShowFinished()
    {
        foreach (GameObject gameObject in finishObjects)
        {
            gameObject.SetActive(true);
        }
    }

    public void HideFinished()
    {
        foreach (GameObject gameObject in finishObjects)
        {
            gameObject.SetActive(false);
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SaveGame()
    {
        if (snake != null)
        {
            snake.SaveGame();
            Debug.Log("Game Saved!");
        }
    }

    private void LoadGame()
    {
        if (snake != null)
        {
            snake.LoadGame();
            Debug.Log("Game Loaded!");
        }
    }

    private void RewindGame()
    {
        if (snake != null)
        {
            snake.Rewind();
        }
    }
}
