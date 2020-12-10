using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    private Animator animator;
    public bool isBeingPressed = false;
    private bool isPlayerTouching = false;
    public bool isBoxTouching = false;

    [Tooltip("The sound that the button object makes.")]
    [SerializeField]
    private AudioSource buttonSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isBoxTouching || isPlayerTouching)
        {
            isBeingPressed = true;
            animator.SetBool("isBeingPressed", true);
        }
        else
        {
            isBeingPressed = false;
            animator.SetBool("isBeingPressed", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = true;
            buttonSound.Play();
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxTouching = true;
            buttonSound.Play();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = false;
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxTouching = false;
        }
    }
}
