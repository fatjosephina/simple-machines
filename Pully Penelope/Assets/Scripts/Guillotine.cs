using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the behavior of the guillotines.
/// </summary>
public class Guillotine : MonoBehaviour
{
    private GameObject playerCharacter;
    
    [SerializeField]
    private float offset;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerCharacter = GameObject.FindWithTag("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (playerCharacter != null)
        {
            if (playerCharacter.transform.position.y > transform.position.y - offset)
            {
                spriteRenderer.sortingLayerName = "Player";
            }
            else if (playerCharacter.transform.position.y <= transform.position.y - offset)
            {
                spriteRenderer.sortingLayerName = "Default";
            }
        }
    }
}
