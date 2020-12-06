using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryMusicChange : MonoBehaviour
{
    [SerializeField]
    private AudioClip victoryMusic;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        QuotaText.GameWon += OnGameWon;
    }

    private void OnDisable()
    {
        QuotaText.GameWon -= OnGameWon;
    }

    /// <summary>
    /// Changes the music if the game is won.
    /// </summary>
    private void OnGameWon()
    {
        audioSource.Stop();
        audioSource.clip = victoryMusic;
        audioSource.loop = false;
        audioSource.Play();
    }
}
