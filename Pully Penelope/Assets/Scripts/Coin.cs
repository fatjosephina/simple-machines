using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public static event Action CoinCollected;

    [Tooltip("The sound to be played when the coin is collected.")]
    [SerializeField]
    private AudioSource collectionSound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CoinCollected?.Invoke();
            collectionSound.Play();
            Destroy(gameObject);
        }
    }
}
