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
            if (handleName == "DHandle")
            {
                handleOrientation.x = 0;
                positionChange.x = handleOrientation.x;
                handleOrientation.y = 1;
            }
            else if (handleName == "UHandle")
            {
                handleOrientation.x = 0;
                positionChange.x = handleOrientation.x;
                handleOrientation.y = -1;
            }
            else if (handleName == "RHandle")
            {
                handleOrientation.x = -1;
                handleOrientation.y = 0;
                positionChange.y = handleOrientation.y;
            }
            else if (handleName == "LHandle")
            {
                handleOrientation.x = 1;
                handleOrientation.y = 0;
                positionChange.y = handleOrientation.y;
            }
            //positionChange.x = handleOrientation.x;
            //positionChange.y = handleOrientation.y;
            //handleTransform.position += positionChange;
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
        /*if (handleName == "DHandle")
        {
            handleOrientation.x = 0;
            handleOrientation.y = 1;
        }
        else if (handleName == "UHandle")
        {
            handleOrientation.x = 0;
            handleOrientation.y = -1;
        }
        else if (handleName == "RHandle")
        {
            handleOrientation.x = -1;
            handleOrientation.y = 0;
        }
        else if (handleName == "LHandle")
        {
            handleOrientation.x = -1;
            handleOrientation.y = 0;
        }*/
        //if (handleTransform != null)
        //{
        //Vector2 D = transform.position - handleTransform.position; // line from crate to player
        //float dist = D.magnitude;
        //Vector2 pullDir = D.normalized; // short blue arrow from crate to player
        //if (dist > 50) handleTransform = null; // lose tracking if too far
        //else if (dist > 3)
        //{ don't pull if too close
        // this is the same math to apply fake gravity. 10 = normal gravity
        //float pullF = 10;
        // for fun, pull a little bit more if further away:
        // (so, random, optional junk):
        //float pullForDist = (dist - 3) / 2.0f;
        //if (pullForDist > 20) pullForDist = 20;
        //pullF += pullForDist;
        // Now apply to pull force, using standard meters/sec converted
        //    into meters/frame:
        //handleTransform.GetComponent<Rigidbody2D>().velocity += pullDir * (pullF * Time.deltaTime);
        //}
        //}
    }

    private void MovePlayer()
    {
        rigidbody.MovePosition(transform.position + positionChange.normalized * moveSpeed * Time.deltaTime);
        if (handleName != null && grabFloat != 0)
        {
            handleObject.GetComponent<Rigidbody2D>().MovePosition(handleTransform.position + positionChange.normalized * moveSpeed * Time.deltaTime);
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
        canGrab = true;
        handleName = collision.gameObject.name;
        handleObject = collision.gameObject.transform.parent.gameObject;
        handleTransform = handleObject.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        canGrab = false;
        handleName = null;
        handleObject = null;
        handleTransform = null;
        isGrabbingHandle = false;
    }
}
