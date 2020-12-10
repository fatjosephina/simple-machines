using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathGate : MonoBehaviour
{
    private Animator animator;
    private Death enemy;

    [SerializeField]
    private GameObject enemyObject;

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
        enemy = enemyObject.GetComponent<Death>();
    }

    private void Update()
    {
        if (enemy.isDead)
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
