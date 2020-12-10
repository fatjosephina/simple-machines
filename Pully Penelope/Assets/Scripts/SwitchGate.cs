using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGate : MonoBehaviour
{
    private Animator animator;
    private SwitchObject switchObject;

    [SerializeField]
    private GameObject switchGameObject;

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
        switchObject = switchGameObject.GetComponent<SwitchObject>();
    }

    private void Update()
    {
        if (switchObject.isOn)
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
