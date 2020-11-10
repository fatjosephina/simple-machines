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
}
