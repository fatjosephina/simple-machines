using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's movement, as well as their grabbing and animations.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 4f;

    private Animator animator;
    private new Rigidbody2D rigidbody;
    private Vector3 positionChange;
    private bool grabInput = false;
    private bool isTouchingHandle;
    private GameObject handleObject;
    private Transform handleTransform;
    private string handleName;
    private string handleAxis;
    private Vector2 handleOrientation;
    private bool isGrabbingHandle;
    private bool shouldUpdateAnimation = true;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        GetInput();
    }

    /// <summary>
    /// Gets input from player and prevents limits them to one axis when holding onto the box.
    /// </summary>
    private void GetInput()
    {
        positionChange = Vector3.zero;
        positionChange.x = Input.GetAxisRaw("Horizontal");
        positionChange.y = Input.GetAxisRaw("Vertical");
        grabInput = Input.GetButton("Jump");
        if (handleName != null && grabInput)
        {
            if (handleAxis == "Vertical")
            {
                positionChange.x = handleOrientation.x;
            }
            else if (handleAxis == "Horizontal")
            {
                positionChange.y = handleOrientation.y;
            }
        }
    }

    private void FixedUpdate()
    {
        MoveAndAnimate();
    }

    /// <summary>
    /// Checks if the player is moving, and updates animations accordingly. If the player is grabbing onto
    /// a box handle, their animation will reflect this.
    /// </summary>
    private void MoveAndAnimate()
    {
        if (positionChange != Vector3.zero)
        {
            MovePlayer();
            if (!isGrabbingHandle)
            {
                animator.SetFloat("orientationX", positionChange.x);
                animator.SetFloat("orientationY", positionChange.y);
            }
            animator.SetBool("isMoving", true);
        }
        else
        {
            if (isGrabbingHandle && shouldUpdateAnimation)
            {
                animator.SetFloat("orientationX", handleOrientation.x);
                animator.SetFloat("orientationY", handleOrientation.y);
                shouldUpdateAnimation = false;
            }
            animator.SetBool("isMoving", false);
        }
        CheckIfGrabbing();
    }

    /// <summary>
    /// Moves the player as well as any objects attached to it.
    /// </summary>
    private void MovePlayer()
    {
        rigidbody.MovePosition(transform.position + positionChange.normalized * moveSpeed * Time.fixedDeltaTime);
        if (handleName != null && grabInput)
        {
            handleObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            handleObject.GetComponent<Rigidbody2D>().MovePosition(handleTransform.position + positionChange.normalized * moveSpeed * Time.deltaTime);
            handleObject.GetComponent<HandleParent>().attachedObject = gameObject;
            gameObject.GetComponent<HandleParent>().attachedObject = handleObject;
        }
        else if (handleName != null)
        {
            handleObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            handleObject.GetComponent<HandleParent>().attachedObject = null;
            gameObject.GetComponent<HandleParent>().attachedObject = null;
        }
    }

    /// <summary>
    /// Checks if the player is grabbing anything, and updates their animation and isGrabbingHandle variable
    /// to reflect this.
    /// </summary>
    private void CheckIfGrabbing()
    {
        if (grabInput)
        {
            animator.SetBool("isGrabbing", true);
            if (isTouchingHandle)
            {
                isGrabbingHandle = true;
            }
            else
            {
                isGrabbingHandle = false;
            }
        }
        else
        {
            animator.SetBool("isGrabbing", false);
            isGrabbingHandle = false;
            shouldUpdateAnimation = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Handle"))
        {
            isTouchingHandle = true;
            handleName = collision.gameObject.name;
            handleAxis = collision.gameObject.GetComponent<Handle>().HandleAxis;
            handleOrientation = collision.gameObject.GetComponent<Handle>().HandleOrientation;
            handleObject = collision.gameObject.transform.parent.gameObject;
            handleTransform = handleObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Handle"))
        {
            isTouchingHandle = false;
            handleName = null;
            handleAxis = null;
            handleOrientation = Vector2.zero;
            handleObject = null;
            handleTransform = null;
            isGrabbingHandle = false;
        }
    }
}
