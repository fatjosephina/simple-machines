﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the behavior of the press key object.
/// </summary>
public class PressKey : MonoBehaviour
{
    private bool checkForGrab = false;
    SpriteRenderer pressKeySprite;
    float fadeTime = 5f;

    private void Start()
    {
        pressKeySprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (checkForGrab == true)
        {
            if (Input.GetAxisRaw("Jump") != 0)
            {
                StartCoroutine(FadeAwayCoroutine(pressKeySprite, fadeTime));
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkForGrab = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            checkForGrab = false;
        }
    }

    /// <summary>
    /// Makes the game object fade away.
    /// </summary>
    private IEnumerator FadeAwayCoroutine(SpriteRenderer spriteRenderer, float duration)
    {
        float counter = 0;
        Color spriteColor = spriteRenderer.material.color;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);

            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }

        Destroy(this.gameObject);
    }
}
