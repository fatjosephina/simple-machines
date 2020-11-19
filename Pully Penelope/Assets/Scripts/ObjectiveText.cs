using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Takes care of the objective text's behavior.
/// </summary>
public class ObjectiveText : MonoBehaviour
{
    [Tooltip("The text of the objective.")]
    [SerializeField]
    private TMP_Text objectiveText;

    [Tooltip("The text of the quota.")]
    [SerializeField]
    private TMP_Text quotaText;

    [Tooltip("The start button.")]
    [SerializeField]
    private TMP_Text startButton;

    [Tooltip("The name of the level.")]
    [SerializeField]
    private string levelName = "Unknown";

    [Tooltip("The dark background object.")]
    [SerializeField]
    private Image darkBackground;

    private string quota;
    private float fadeTime = 3f;

    private void Start()
    {
        quota = quotaText.text;

        objectiveText.text = "Fulfill your loot quota and make your escape from the " + levelName + "! " + quota + ".";
    }

    /// <summary>
    /// Hides the start button and calls the coroutine to fade the objective text.
    /// </summary>
    public void StartGame()
    {
        startButton.enabled = false;
        darkBackground.enabled = false;
        StartCoroutine(FadeAwayCoroutine(objectiveText, fadeTime));
    }

    /// <summary>
    /// Makes the objective text fade away.
    /// </summary>
    private IEnumerator FadeAwayCoroutine(TMP_Text text, float duration)
    {
        float counter = 0;
        Color textColor = objectiveText.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);

            text.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        if (text != null)
        {
            Destroy(text.gameObject);
        }
    }
}
