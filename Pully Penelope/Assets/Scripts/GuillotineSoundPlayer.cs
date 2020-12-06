using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuillotineSoundPlayer : MonoBehaviour
{
    [Tooltip("The sound that the guillotine makes.")]
    [SerializeField]
    private AudioSource guillotineSound;

    public void PlayGuillotineSound()
    {
        guillotineSound.Play();
    }
}
