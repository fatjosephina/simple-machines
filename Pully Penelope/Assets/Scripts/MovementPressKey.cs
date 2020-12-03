using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPressKey : MonoBehaviour
{
    SpriteRenderer pressKeySprite;
    float fadeTime = 5f;
    Animator animator;
    bool shouldSwitchAnimation = true;
    public bool shouldBeFast = false;
    private Color spriteColor;

    private void Start()
    {
        pressKeySprite = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        spriteColor = pressKeySprite.material.color;
    }

    private void Update()
    {
        if ((Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 ) && pressKeySprite.enabled)
        {
            StartCoroutine(FadeAwayCoroutine(pressKeySprite, fadeTime));
        }
        if (Time.frameCount % 500 == 0)
        {
            if (shouldSwitchAnimation)
            {
                animator.SetBool("shouldSwitchAnimation", true);
                shouldSwitchAnimation = false;
            }
            else
            {
                animator.SetBool("shouldSwitchAnimation", false);
                shouldSwitchAnimation = true;
            }
        }
        if (shouldBeFast)
        {
            StopCoroutine(FadeAwayCoroutine(pressKeySprite, fadeTime));
            pressKeySprite.enabled = true;
            pressKeySprite.color = spriteColor;
            animator.SetBool("shouldBeFast", true);
            shouldBeFast = false;
        }
    }

    /// <summary>
    /// Makes the game object fade away.
    /// </summary>
    private IEnumerator FadeAwayCoroutine(SpriteRenderer spriteRenderer, float duration)
    {
        float counter = 0;
        spriteRenderer.color = spriteColor;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, counter / duration);

            spriteRenderer.color = new Color(spriteColor.r, spriteColor.g, spriteColor.b, alpha);
            yield return null;
        }

        spriteRenderer.enabled = false;
        spriteRenderer.color = spriteColor;
    }
}
