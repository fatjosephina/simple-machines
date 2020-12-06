using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controls the player's quota, as well as their loss and win.
/// </summary>
public class Death : MonoBehaviour
{
    private Color32 red = new Color32(255, 84, 84, 255);
    SpriteRenderer spriteRenderer;
    private float dieDuration = 1f;
    public bool isDead = false;
    private bool isAlreadyDying = false;

    [Tooltip("The sound to be made upon destruction.")]
    [SerializeField]
    private AudioSource deathSound;

    private void Start()
    {
        isDead = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (deathSound == null)
        {
            deathSound = GameObject.Find("DeathSound").GetComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isDead && !isAlreadyDying)
        {
            StartCoroutine(DieCoroutine());
        }
    }

    /// <summary>
    /// Kills the player by turning them red and then destroying them.
    /// </summary>
    private IEnumerator DieCoroutine()
    {
        isAlreadyDying = true;
        spriteRenderer.color = red;
        deathSound.Play();
        yield return new WaitForSeconds(dieDuration);
        Destroy(gameObject);
        yield return null;
    }
}
