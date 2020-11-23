using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGate : MonoBehaviour
{
    private Animator animator;
    private ButtonObject buttonObject;

    [SerializeField]
    private GameObject buttonGameObject;

    private void Start()
    {
        animator = GetComponent<Animator>();
        buttonObject = buttonGameObject.GetComponent<ButtonObject>();
    }

    private void Update()
    {
        if (buttonObject.isBeingPressed)
        {
            animator.SetBool("shouldOpen", true);
        }
        else
        {
            animator.SetBool("shouldOpen", false);
        }
    }
}
