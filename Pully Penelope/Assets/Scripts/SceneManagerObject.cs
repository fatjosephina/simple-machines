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
    /// Loads the first level, called the Rookie's Ridge in-game.
    /// </summary>
    public void LoadLevel1Scene()
    {
        SceneManager.LoadScene("Level1Scene");
    }

    /// <summary>
    /// Loads the second level, called the Grandfather Grotto in-game.
    /// </summary>
    public void LoadLevel2Scene()
    {
        SceneManager.LoadScene("Level2Scene");
    }

    /// <summary>
    /// Loads the third level, called the Cave of Courage in-game.
    /// </summary>
    public void LoadLevel3Scene()
    {
        SceneManager.LoadScene("Level3Scene");
    }

    /// <summary>
    /// Loads the sample scene, called the Test Level in-game.
    /// </summary>
    public void LoadSampleScene()
    {
        SceneManager.LoadScene("SampleScene");
    }

    /// <summary>
    /// Loads the first custom bonus scene, called the Penelopedia in-game.
    /// </summary>
    public void LoadPenelopediaScene()
    {
        SceneManager.LoadScene("PenelopediaScene");
    }

    /// <summary>
    /// Loads the second custom bonus scene, called the !TOP SECRET! in-game.
    /// </summary>
    public void LoadTopSecretScene()
    {
        SceneManager.LoadScene("TopSecretScene");
    }
}
