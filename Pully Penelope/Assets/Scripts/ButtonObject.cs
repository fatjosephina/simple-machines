using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonObject : MonoBehaviour
{
    private Animator animator;
    public bool isBeingPressed = false;
    private bool isPlayerTouching = false;
    public bool isBoxTouching = false;

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
            Debug.Log("Player enter");
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxTouching = true;
            Debug.Log("Box enter");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = false;
            Debug.Log("Player exit");
        }
        if (collision.gameObject.CompareTag("Box"))
        {
            isBoxTouching = false;
            Debug.Log("Box exit");
        }
    }
}
