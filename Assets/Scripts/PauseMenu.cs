using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private GameObject pauseCanvas;
    public static bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        pauseCanvas = GameObject.Find("PauseCanvas");
        pauseCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // PauseGame
    public void PauseGame()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    // ResumeGame
    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // BackToMainMenu
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
        isPaused = false;
    }

    // PauseQuit
    public void PauseQuit()
    {
        Application.Quit();
    }
}
