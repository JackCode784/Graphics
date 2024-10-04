using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Funzioni per pulsanti del Main Menu
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        // Load main scene by name
        SceneManager.LoadSceneAsync("Main Level");
    }

    public void QuitGame()
    {
        // to do
        Application.Quit();
    }
}
