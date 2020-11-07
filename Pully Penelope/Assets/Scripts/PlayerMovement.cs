using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// As you can tell by the fields, this class essentially handles 5 things: movement, physics, animation,
    /// grabbing, and handles. It also affects other game objects, namely the handleObject.
    /// </summary>
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

    /// <summary>
    /// Start gets the rigidbody and animator.
    /// </summary>
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update checks for the horizontal axis, vertical axis, and jump axis (spacebar). Hori. and vert. are
    /// used for movement, while jump is used for grabbing. It also checks if the player is grabbing by
    /// verifying the handleName (name of game object which can be grabbed) and grabFloat (whether or not
    /// the player is pressing down the spacebar). If they are being grabbed, then it determines the
    /// handleOrientation, which is used to determine which directions the player can move while grabbing
    /// the handleObject (and to set the animation later on).
    /// </summary>
    private void Update()
    {
        positionChange = Vector3.zero;
        positionChange.x = Input.GetAxisRaw("Horizontal");
        positionChange.y = Input.GetAxisRaw("Vertical");
        grabFloat = Input.GetAxisRaw("Jump");
        if (handleName != null && grabFloat != 0)
        {
            switch (handleName)
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
            }
        }
    }

    /// <summary>
    /// FixedUpdate calls the function to move and animate. The reason why animation happens here
    /// and not in Update is because it does not make sense to have the character animate (for example,
    /// a walking animation) unless it is actually doing what the animation is trying to represent
    /// (for example, walking).
    /// </summary>
    private void FixedUpdate()
    {
        MoveAndAnimate();
    }

    /// <summary>
    /// If the player is actually trying to move, MoveAndAnimate() will call MovePlayer(). Then, if they are
    /// not grabbing the handle, the animator will be set to reflect the positionChange. The reason why this
    /// does not happen when they are grabbing is because you cannot pull a chair with your back touching the
    /// chair and your hands in front of you. Whether they are grabbing or not, the animator is set to reflect
    /// that they are moving. If they are not moving, but they are grabbing and the bool shouldUpdateAnimation
    /// is true, then they will change directions. This is because I want it to be impossible to move the
    /// handleObject horizontally while grabbing the vertical handles and vice versa. So, if the player attempts
    /// this, their orientation will change using the handleOrientation. Whether they are grabbing or not, their
    /// moving animation is set to false. Finally, it checks if the player is grabbing.
    /// </summary>
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

    /// <summary>
    /// MovePlayer() moves the player. But, it also moves whatever is "attached" to them. An object becomes
    /// attached to them when they grab it. Both the other object and the player have an attachedObject
    /// variable on them. It also sets the other object to its default bodyType if it is not being grabbed.
    /// This prevents a glitch where the objects would start sliding when the player let go.
    /// </summary>
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

    /// <summary>
    /// CheckIfGrabbing() checks if the player is grabbing, and updates the animator, isGrabbingHandle and
    /// shouldUpdateAnimation, both of which are used in MoveAndAnimate() to control the animations of the
    /// player.
    /// </summary>
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

    /// <summary>
    /// If the player enters a handle trigger, all of the necessary fields are set to reflect this.
    /// </summary>
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

    /// <summary>
    /// Likewise, if the player exits a handle trigger, the necessary fields are set to reflec this as well.
    /// </summary>
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
