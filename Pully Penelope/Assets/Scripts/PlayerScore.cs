using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controls the player's quota, as well as their loss and win.
/// </summary>
public class PlayerScore : MonoBehaviour
{
    [Tooltip("The text of the quota.")]
    [SerializeField]
    private TMP_Text quotaText;
    private int quota = 3;

    private Color white;
    private Color32 red = new Color32(255, 84, 84, 255);
    private Color32 green = new Color32(56, 216, 76, 255);
    private int flashRepeat = 4;
    private float flashDuration = 0.25f;

    [Tooltip("The win and loss button.")]
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
        white = quotaText.color;
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
                StartCoroutine(FlashRedCoroutine());
            }
        }

        if (collision.gameObject.CompareTag("Blade"))
        {
            quotaText.text = "You Lose! Press here to replay!";
            button.enabled = true;
            quotaText.color = red;
            StartCoroutine(DieCoroutine());
        }
    }

    /// <summary>
    /// Makes the quota text flash red when the player has not fulfilled their quota.
    /// </summary>
    private IEnumerator FlashRedCoroutine()
    {
        for (int i = 0; i < flashRepeat; i++)
        {
            quotaText.color = red;
            yield return new WaitForSeconds(flashDuration);
            quotaText.color = white;
            yield return new WaitForSeconds(flashDuration);
        }
        yield return null;
    }

    /// <summary>
    /// Kills the player by turning them red and then destroying them.
    /// </summary>
    private IEnumerator DieCoroutine()
    {
        isDead = true;
        spriteRenderer.color = red;
        yield return new WaitForSeconds(dieDuration);
        Destroy(gameObject);
        yield return null;
    }
}
