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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadMenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void LoadLevelSelectScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void LoadControlsScene()
    {
        SceneManager.LoadScene("ControlsScene");
    }

    public void LoadCreditsScene()
    {
        SceneManager.LoadScene("CreditsScene");
    }

    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
