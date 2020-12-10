using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchObject : MonoBehaviour
{
    private Animator animator;
    public bool isOn = false;

    [Tooltip("The sound that the switch object makes.")]
    [SerializeField]
    private AudioSource switchSound;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            FlipSwitch();
        }
    }

    /// <summary>
    /// If the switch is on, sets it to off, if it is off, sets it to on.
    /// </summary>
    private void FlipSwitch()
    {
        switchSound.Play();
        if (animator.GetBool("isOn"))
        {
            animator.SetBool("isOn", false);
            isOn = false;
        }
        else
        {
            animator.SetBool("isOn", true);
            isOn = true;
        }
    }
}
