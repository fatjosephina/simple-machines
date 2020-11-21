using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalArrow : MonoBehaviour
{
    private Image arrowSprite;
    private bool gameFinished = false;

    private void Start()
    {
        arrowSprite = GetComponent<Image>();
        arrowSprite.enabled = false;
    }
    private void OnEnable()
    {
        QuotaText.QuotaMet += OnQuotaMet;
        QuotaText.GameWon += OnGameFinished;
        QuotaText.GameLost += OnGameFinished;
    }

    private void OnDisable()
    {
        QuotaText.QuotaMet -= OnQuotaMet;
        QuotaText.GameWon -= OnGameFinished;
        QuotaText.GameLost -= OnGameFinished;
    }

    /// <summary>
    /// If the game is not finished, enables the arrow sprite
    /// </summary>
    private void OnQuotaMet()
    {
        if (!gameFinished)
        {
            arrowSprite.enabled = true;
        }
    }

    /// <summary>
    /// Disables the arrow sprite since the game is finished (won or lost)
    /// </summary>
    private void OnGameFinished()
    {
        gameFinished = true;
        arrowSprite.enabled = false;
    }
}
