using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the player's movement, as well as their grabbing and animations.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    [Tooltip("The speed of movement.")]
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

    private AudioSource attachSound;
    private bool hasPlayedAttachSound = false;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("orientationX", 0);
        animator.SetFloat("orientationY", -1);
        attachSound = GameObject.Find("AttachSound").GetComponent<AudioSource>();
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
        if (handleObject != null && handleObject.CompareTag("Box"))
        {
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
            if (handleObject.CompareTag("Box"))
            {
                handleObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                handleObject.GetComponent<Rigidbody2D>().MovePosition(handleTransform.position + positionChange.normalized * moveSpeed * Time.deltaTime);
                handleObject.GetComponent<HandleParent>().attachedObject = gameObject;
                gameObject.GetComponent<HandleParent>().attachedObject = handleObject;
            }
            else if (handleObject.CompareTag("Enemy"))
            {
                handleObject.GetComponent<EnemyMovement>().isBeingGrabbed = true;
                handleObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                handleObject.GetComponent<Rigidbody2D>().MovePosition(handleTransform.position + positionChange.normalized * moveSpeed * Time.deltaTime);
                handleObject.GetComponent<HandleParent>().attachedObject = gameObject;
                gameObject.GetComponent<HandleParent>().attachedObject = handleObject;
            }
        }
        else if (handleName != null)
        {
            if (handleObject.CompareTag("Box"))
            {
                handleObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
                handleObject.GetComponent<HandleParent>().attachedObject = null;
                gameObject.GetComponent<HandleParent>().attachedObject = null;
            }
            else if (handleObject.CompareTag("Enemy"))
            {
                handleObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
                handleObject.GetComponent<HandleParent>().attachedObject = null;
                gameObject.GetComponent<HandleParent>().attachedObject = null;
            }
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
                if (!hasPlayedAttachSound)
                {
                    attachSound.Play();
                    hasPlayedAttachSound = true;
                }
            }
            else
            {
                isGrabbingHandle = false;
                hasPlayedAttachSound = false;
            }
        }
        else
        {
            animator.SetBool("isGrabbing", false);
            isGrabbingHandle = false;
            shouldUpdateAnimation = true;
            hasPlayedAttachSound = false;
        }
    }


    /// <summary>
    /// Sets handle data to reflect the handle which the player entered
    /// </summary>
    public void HandleEntry(string nameOfHandle, string axisOfHandle, Vector2 orientationOfHandle, GameObject parentOfHandle)
    {
        isTouchingHandle = true;
        handleName = nameOfHandle;
        handleAxis = axisOfHandle;
        handleOrientation = orientationOfHandle;
        handleObject = parentOfHandle;
        handleTransform = handleObject.transform;
    }

    /// <summary>
    /// Sets handle data to reflect that the player exited the handle
    /// </summary>
    public void HandleExit()
    {
        isTouchingHandle = false;
        handleName = null;
        handleAxis = null;
        handleOrientation = Vector2.zero;
        handleObject = null;
        handleTransform = null;
        isGrabbingHandle = false;
    }

    private void OnDestroy()
    {
        if (Camera.main != null)
        {
            Camera.main.GetComponent<AudioListener>().enabled = true;
        }
    }
}
