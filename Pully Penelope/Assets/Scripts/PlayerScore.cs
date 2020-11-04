using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    [SerializeField]
    private TMP_Text quotaText;
    private int quota = 3;

    private Color32 red = new Color32(255, 84, 84, 255);
    private Color32 green = new Color32(56, 216, 76, 255);
    private int flashRepeat = 4;
    private float flashDuration = 0.25f;
    [SerializeField]
    private Button button;
    SpriteRenderer spriteRenderer;
    private float dieDuration = 1f;
    private bool isDead = false;

    private void Start()
    {
        isDead = false;
        quotaText = GameObject.FindWithTag("Quota").GetComponent<TMP_Text>();
        quotaText.text = "Quota : " + quota;
        button = quotaText.GetComponent<Button>();
        button.enabled = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (quota == 0 && !isDead)
        {
            quotaText.color = green;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            quota--;
            quotaText.text = "Quota : " + quota;
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Goal"))
        {
            if (quota == 0)
            {
                quotaText.text = "You Win! Press here to replay!";
                button.enabled = true;
            }
            else
            {
                StartCoroutine(FlashRedCo());
            }
        }

        if (collision.gameObject.CompareTag("Blade"))
        {
            quotaText.text = "You Lose! Press here to replay!";
            button.enabled = true;
            quotaText.color = red;
            StartCoroutine(DieCo());
        }
    }

    private IEnumerator FlashRedCo()
    {
        Color white = quotaText.color;
        for (int i = 0; i < flashRepeat; i++)
        {
            quotaText.color = red;
            yield return new WaitForSeconds(flashDuration);
            quotaText.color = white;
            yield return new WaitForSeconds(flashDuration);
        }
        yield return null;
    }

    private IEnumerator DieCo()
    {
        isDead = true;
        spriteRenderer.color = red;
        yield return new WaitForSeconds(dieDuration);
        Destroy(gameObject);
        yield return null;
    }
}
