using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PenelopediaTrigger : MonoBehaviour
{
    [Tooltip("The text element to be changed.")]
    [SerializeField]
    private TMP_Text penelopediaText;

    [Tooltip("The new text which represents the name of the room's object.")]
    [SerializeField]
    private new string name;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        penelopediaText.text = name;
    }
}
