using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4f;

    private Animator animator;
    private new Rigidbody2D rigidbody;
    private Vector3 positionChange;
    private float grabFloat = 0;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        positionChange = Vector3.zero;
        positionChange.x = Input.GetAxisRaw("Horizontal");
        positionChange.y = Input.GetAxisRaw("Vertical");
        grabFloat = Input.GetAxisRaw("Jump");
    }

    private void FixedUpdate()
    {
        MoveAndAnimate();
    }

    private void MoveAndAnimate()
    {
        if (positionChange != Vector3.zero)
        {
            MovePlayer();
            animator.SetFloat("moveX", positionChange.x);
            animator.SetFloat("moveY", positionChange.y);
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        CheckIfGrabbing();
    }

    private void MovePlayer()
    {
        rigidbody.MovePosition(transform.position + positionChange.normalized * moveSpeed * Time.deltaTime);
    }

    private void CheckIfGrabbing()
    {
        if (grabFloat != 0)
        {
            animator.SetBool("isGrabbing", true);
        }
        else
        {
            animator.SetBool("isGrabbing", false);
        }
    }
}
