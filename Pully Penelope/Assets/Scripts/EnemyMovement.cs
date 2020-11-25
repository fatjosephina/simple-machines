using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles enemy movement and related behavior.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Tooltip("The speed of movement.")]
    [SerializeField]
    private float moveSpeed;

    [Tooltip("The target of the enemy.")]
    [SerializeField]
    private Transform target;

    [Tooltip("The home position.")]
    [SerializeField]
    private Vector3 homePosition;

    [Tooltip("The radius at which the enemy will detect the player.")]
    [SerializeField]
    private float chaseRadius;

    [Tooltip("The radius of attack.")]
    [SerializeField]
    private float attackRadius;

    [Tooltip("The transform of the guillotine.")]
    [SerializeField]
    private Transform guillotine;

    private float playerInputValue;
    private float waitTime = 0.1f;
    private bool playerInputCoroutineStarted = false;
    private bool isOnCooldown = false;
    private float cooldownTime = 1f;
    public bool playerInRange = false;
    private State state;
    private Animator animator;
    private Vector3 lastPosition;
    public bool isBeingGrabbed = false;

    private enum State
    {
        Standard,
        Grabbing,
        BeingGrabbed
    }

    private void Start()
    {
        state = State.Standard;
        target = GameObject.FindWithTag("Player").transform;
        homePosition = transform.position;
        animator = GetComponent<Animator>();
        animator.SetFloat("orientationX", 0);
        animator.SetFloat("orientationY", -1);
        lastPosition = transform.position;
    }

    private void Update()
    {
        DefineState();
        SetOrientation();
    }

    /// <summary>
    /// Calls the respective method for the correct state.
    /// </summary>
    private void DefineState()
    {
        if (state == State.Standard)
        {
            BehaveStandard();
        }
        else if (state == State.Grabbing)
        {
            BehaveGrabbing();
        }
        else if (state == State.BeingGrabbed)
        {
            BehaveBeingGrabbed();
        }
    }

    /// <summary>
    /// Sets the enemy's orientation
    /// </summary>
    private void SetOrientation()
    {
        float differenceX = transform.position.x - lastPosition.x;
        animator.SetFloat("orientationX", differenceX);
        float differenceY = transform.position.y - lastPosition.y;
        //if (differenceX == 0 && differenceY == 0) { differenceY = -1; }
        animator.SetFloat("orientationY", differenceY);
        lastPosition = transform.position;
    }

    /// <summary>
    /// Checks the distance between the player, and either tries to catch them or return to home base accordingly.
    /// </summary>
    private void BehaveStandard()
    {
        if (!isBeingGrabbed)
        {
            animator.SetBool("isGrabbing", false);
            animator.SetBool("isBeingGrabbed", false);
            if (target != null)
            {
                if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
                {
                    if (!playerInRange)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                    }
                    else
                    {
                        state = State.Grabbing;
                    }
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
                }
            }
        }
        else
        {
            state = State.BeingGrabbed;
        }
    }

    /// <summary>
    /// Tries to kill the player or let them go.
    /// </summary>
    private void BehaveGrabbing()
    {
        if (!isBeingGrabbed)
        {
            animator.SetBool("isGrabbing", true);
            animator.SetBool("isBeingGrabbed", false);
            if (!isOnCooldown)
            {
                StartCoroutine(KillPlayerCoroutine());
            }
            else
            {
                state = State.Standard;
            }
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !playerInputCoroutineStarted)
            {
                playerInputValue = (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")) / 2;
                StartCoroutine(CheckIfSpammingButtonCoroutine());
            }
        }
        else
        {
            state = State.BeingGrabbed;
        }
    }

    /// <summary>
    /// A vulnerable state where the enemy cannot do much.
    /// </summary>
    private void BehaveBeingGrabbed()
    {
        if (GetComponent<HandleParent>().attachedObject != null)
        {
            animator.SetBool("isGrabbing", false);
            animator.SetBool("isBeingGrabbed", true);
        }
        else
        {
            isBeingGrabbed = false;
            state = State.Standard;
        }
    }

    /// <summary>
    /// Attaches itself to the player and attempts to kill them.
    /// </summary>
    private IEnumerator KillPlayerCoroutine()
    {
        if (target != null)
        {
            target.gameObject.GetComponentInChildren<MovementPressKey>().shouldBeFast = true;
            gameObject.GetComponent<HandleParent>().attachedObject = target.gameObject;
            target.gameObject.GetComponent<PlayerMovement>().enabled = false;
            target.gameObject.GetComponent<HandleParent>().attachedObject = gameObject;
            transform.position = Vector3.MoveTowards(transform.position, guillotine.position, moveSpeed * Time.deltaTime);
            target.position = Vector3.MoveTowards(target.position, guillotine.position, moveSpeed * Time.deltaTime);
        }
        yield return null;
    }

    /// <summary>
    /// Checks if the player is spamming buttons, and if they are, they will be released.
    /// </summary>
    private IEnumerator CheckIfSpammingButtonCoroutine()
    {
        playerInputCoroutineStarted = true;
        yield return new WaitForSeconds(waitTime);
        if ((Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")) / 2 != playerInputValue)
        {
            if (target != null)
            {
                gameObject.GetComponent<HandleParent>().attachedObject = null;
                target.gameObject.GetComponent<PlayerMovement>().enabled = true;
                target.gameObject.GetComponent<HandleParent>().attachedObject = null;
            }
            isOnCooldown = true;
        }
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
        playerInputCoroutineStarted = false;
        yield return null;
    }
}
