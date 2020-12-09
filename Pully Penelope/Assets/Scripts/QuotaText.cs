﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuotaText : MonoBehaviour
{
    private TMP_Text quotaText;
    private Button button;

    private Death player;
    private bool playerDead;

    [Tooltip("The quota to be used in the script.")]
    [SerializeField]
    private int quota = 3;

    private Color white;
    private Color32 red = new Color32(255, 84, 84, 255);
    private Color32 green = new Color32(56, 216, 76, 255);
    private int flashRepeat = 4;
    private float flashDuration = 0.25f;
    private bool isAlreadyFlashing = false;

    public static event Action QuotaMet;
    public static event Action GameWon;
    public static event Action GameLost;

    [Tooltip("The sound to be played upon meeting the quota.")]
    [SerializeField]
    private AudioSource quotaCollectionSound;
    private bool hasQuotaCollectionSoundPlayed = false;

    [Tooltip("The image under the quota button.")]
    [SerializeField]
    private Image quotaImage;

    private void Start()
    {
        quotaText = GetComponent<TMP_Text>();
        quotaText.text = "Quota : " + quota;
        white = quotaText.color;
        button = quotaText.GetComponent<Button>();
        button.enabled = false;
        quotaImage.enabled = false;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Death>();
    }

    private void OnEnable()
    {
        Goal.GoalEntered += OnGoalEntered;
        Coin.CoinCollected += OnCoinCollected;
    }

    private void OnDisable()
    {
        Goal.GoalEntered -= OnGoalEntered;
        Coin.CoinCollected -= OnCoinCollected;
    }

    private void Update()
    {
        playerDead = player.isDead;
        if (quota <= 0 && !playerDead)
        {
            QuotaMet?.Invoke();
            if (!hasQuotaCollectionSoundPlayed)
            {
                quotaCollectionSound.Play();
                hasQuotaCollectionSoundPlayed = true;
            }
            quotaText.color = green;
        }
        if (playerDead)
        {
            GameLost?.Invoke();
            quotaText.text = "You Lose! Pause (ESC/P) to restart!";
            button.enabled = true;
            quotaImage.enabled = true;
            quotaText.color = red;
        }
    }

    /// <summary>
    /// Controls if the player wins or if the quota text will flash red
    /// </summary>
    private void OnGoalEntered()
    {
        if (quota <= 0)
        {
            GameWon?.Invoke();
            quotaText.text = "You Win! Press here to go to the main menu!";
            button.enabled = true;
            quotaImage.enabled = true;
        }
        else
        {
            if (!isAlreadyFlashing)
            {
                StartCoroutine(FlashRedCoroutine());
            }
        }
    }

    /// <summary>
    /// Controls the quota
    /// </summary>
    private void OnCoinCollected()
    {
        quota--;
        quotaText.text = "Quota : " + quota;
    }

    /// <summary>
    /// Makes the quota text flash red
    /// </summary>
    private IEnumerator FlashRedCoroutine()
    {
        isAlreadyFlashing = true;
        for (int i = 0; i < flashRepeat; i++)
        {
            quotaText.color = red;
            yield return new WaitForSeconds(flashDuration);
            quotaText.color = white;
            yield return new WaitForSeconds(flashDuration);
        }
        isAlreadyFlashing = false;
        yield return null;
    }
}
