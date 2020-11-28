using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Manages the scenes.
/// </summary>
public class SceneManagerObject : MonoBehaviour
{
    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    public void LoadLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    /// <summary>
    /// Loads the controls scene.
    /// </summary>
    public void LoadControlsScene()
    {
        SceneManager.LoadScene("ControlsScene");
    }

    /// <summary>
    /// Loads the credits scene.
    /// </summary>
    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    /// <summary>
    /// Loads the sample scene, called the Test Level in-game.
    /// </summary>
    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
