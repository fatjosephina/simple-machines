using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [Tooltip("The pause menu containing all of the text to activate for the pause.")]
    [SerializeField]
    private GameObject pauseMenu;

    [Tooltip("The dark background object.")]
    [SerializeField]
    private Image darkBackground;

    private bool isPaused = false;
    private bool gameStarted = false;

    private void Start()
    {
        pauseMenu.SetActive(false);
    }

    private void Update()
    {
        if (gameStarted)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                isPaused = !isPaused;
            }
        }

        if (isPaused)
        {
            ActivatePause();
        }
        else
        {
            DeactivatePause();
        }
    }

    /// <summary>
    /// Pauses the game.
    /// </summary>
    public void ActivatePause()
    {
        Time.timeScale = 0;
        if (gameStarted)
        {
            AudioListener.pause = true;
            pauseMenu.SetActive(true);
        }
        darkBackground.enabled = true;
        isPaused = true;
    }

    /// <summary>
    /// Unpauses the game.
    /// </summary>
    public void DeactivatePause()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;
        pauseMenu.SetActive(false);
        darkBackground.enabled = false;
        isPaused = false;
        gameStarted = true;
    }
}
