using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the spawn game object.
/// </summary>
public class Spawn : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCharacter;

    private void Awake()
    {
        Instantiate(playerCharacter, gameObject.transform.position, gameObject.transform.rotation);
    }
}
