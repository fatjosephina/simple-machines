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
    private bool canGrab;
    private GameObject handleObject;
    private Transform handleTransform;
    private string handleName;
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
        positionChange = Vector3.zero;
        positionChange.x = Input.GetAxisRaw("Horizontal");
        positionChange.y = Input.GetAxisRaw("Vertical");
        grabFloat = Input.GetAxisRaw("Jump");
        if (handleName != null && grabFloat != 0)
        {
            /*switch (handleName)
            {
                case "DHandle":
                    handleOrientation.x = 0;
                    positionChange.x = handleOrientation.x;
                    handleOrientation.y = 1;
                    break;
                case "UHandle":
                    handleOrientation.x = 0;
                    positionChange.x = handleOrientation.x;
                    handleOrientation.y = -1;
                    break;
                case "RHandle":
                    handleOrientation.x = -1;
                    handleOrientation.y = 0;
                    positionChange.y = handleOrientation.y;
                    break;
                case "LHandle":
                    handleOrientation.x = 1;
                    handleOrientation.y = 0;
                    positionChange.y = handleOrientation.y;
                    break;
            }*/
            /*if (handleAxis == "Vertical")
             * {
             *  positionChange.x = 0;
             * }
             * else if (handleAxis == "Horizontal")
             * {
             *  positionChange.y = 0;
             * }*/
        }
    }

    private void FixedUpdate()
    {
        MoveAndAnimate();
    }

    private void MoveAndAnimate()
    {
        //Debug.Log(isGrabbingHandle);
        if (positionChange != Vector3.zero)
        {
            MovePlayer();
            if (!isGrabbingHandle)
            {
                animator.SetFloat("moveX", positionChange.x);
                animator.SetFloat("moveY", positionChange.y);
            }
            /*else if (isGrabbingHandle && shouldUpdateAnimation)
            {
                Debug.Log("Work");
                animator.SetFloat("moveX", handleOrientation.x);
                animator.SetFloat("moveY", handleOrientation.y);
                //shouldUpdateAnimation = false;
            }*/
            animator.SetBool("isMoving", true);
        }
        else
        {
            if (isGrabbingHandle && shouldUpdateAnimation)
            {
                animator.SetFloat("moveX", handleOrientation.x);
                animator.SetFloat("moveY", handleOrientation.y);
                shouldUpdateAnimation = false;
            }
            animator.SetBool("isMoving", false);
        }
        CheckIfGrabbing();
    }

    private void MovePlayer()
    {
        rigidbody.MovePosition(transform.position + positionChange.normalized * moveSpeed * Time.fixedDeltaTime);
        if (handleName != null && grabFloat != 0)
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

    private void CheckIfGrabbing()
    {
        if (grabFloat != 0)
        {
            animator.SetBool("isGrabbing", true);
            if (canGrab)
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
            canGrab = true;
            handleName = collision.gameObject.name;
            handleObject = collision.gameObject.transform.parent.gameObject;
            handleTransform = handleObject.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Handle"))
        {
            canGrab = false;
            handleName = null;
            handleObject = null;
            handleTransform = null;
            isGrabbingHandle = false;
        }
    }
}
