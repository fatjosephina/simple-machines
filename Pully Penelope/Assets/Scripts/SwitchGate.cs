using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchGate : MonoBehaviour
{
    private Animator animator;
    private SwitchObject switchObject;

    [SerializeField]
    private GameObject switchGameObject;

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
        }
        else
        {
            animator.SetBool("shouldOpen", false);
        }
    }
}
