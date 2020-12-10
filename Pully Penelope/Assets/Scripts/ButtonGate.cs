using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonGate : MonoBehaviour
{
    private Animator animator;
    private ButtonObject buttonObject;

    [Tooltip("The button game object.")]
    [SerializeField]
    private GameObject buttonGameObject;

    [Tooltip("The sound to be played when the gate opens.")]
    [SerializeField]
    private AudioSource openGateSound;
    private bool openGateSoundHasPlayed = false;

    [Tooltip("The sound to be played when the gate closes.")]
    [SerializeField]
    private AudioSource closeGateSound;
    private bool closeGateSoundHasPlayed = false;

    private bool gateSoundHasPlayedOnce = false;

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
            closeGateSoundHasPlayed = false;
            if (!openGateSoundHasPlayed)
            {
                openGateSound.Play();
                openGateSoundHasPlayed = true;
                gateSoundHasPlayedOnce = true;
            }
        }
        else
        {
            animator.SetBool("shouldOpen", false);
            openGateSoundHasPlayed = false;
            if (gateSoundHasPlayedOnce)
            {
                if (!closeGateSoundHasPlayed)
                {
                    closeGateSound.Play();
                    closeGateSoundHasPlayed = true;
                }
            }
        }
    }
}
