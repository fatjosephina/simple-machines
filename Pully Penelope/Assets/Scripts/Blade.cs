using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    private bool isGameWon = false;

    private void OnEnable()
    {
        QuotaText.GameWon += OnGameWon;
    }

    private void OnDisable()
    {
        QuotaText.GameWon -= OnGameWon;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!isGameWon)
            {
                collision.gameObject.GetComponent<Death>().isDead = true;
            }
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<EnemyMovement>().isBeingGrabbed)
            {
                collision.gameObject.GetComponent<Death>().isDead = true;
            }
        }
    }

    /// <summary>
    /// Sets the boolean to true so that the player cannot die when they have already won.
    /// </summary>
    private void OnGameWon()
    {
        isGameWon = true;
    }
}
