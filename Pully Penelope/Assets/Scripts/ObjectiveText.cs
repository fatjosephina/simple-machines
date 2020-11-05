using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ObjectiveText : MonoBehaviour
{
    [SerializeField]
    private TMP_Text objectiveText;

    [SerializeField]
    private TMP_Text quotaText;

    [SerializeField]
    private string levelName = "Unknown";

    private string quota;
    private float fadeTime = 5f;
    private float waitTime = 1f;

    private void Start()
    {
        quota = quotaText.text;

        objectiveText.text = "Fulfill your loot quota and make your escape from the " + levelName + "! " + quota + ".";

        StartCoroutine(FadeAwayCo(objectiveText, fadeTime));
    }

    private IEnumerator FadeAwayCo(TMP_Text text, float duration)
    {
        yield return new WaitForSeconds(waitTime);

        float counter = 0;
        Color textColor = objectiveText.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);

            text.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
            yield return null;
        }

        Destroy(text.gameObject);
    }
}
