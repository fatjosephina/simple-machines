using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles enemy movement and related behavior.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 homePosition;

    [SerializeField]
    private float chaseRadius;

    [SerializeField]
    private float attackRadius;

    [SerializeField]
    private Transform guillotine;

    private float playerInputValue;
    private float waitTime = 0.1f;
    private bool playerInputCoroutineStarted = false;
    private bool isOnCooldown = false;
    private float cooldownTime = 1f;
    private bool playerInRange = false;
    private State state;
    private Animator animator;
    private Vector3 lastPosition;

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
        lastPosition = transform.position;
    }

    private void Update()
    {
        DefineState();
        SetOrientation();
    }

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

    private void SetOrientation()
    {
        animator.SetFloat("orientationX", transform.position.x - lastPosition.x);
        animator.SetFloat("orientationY", transform.position.y - lastPosition.y);
        lastPosition = transform.position;
    }

    /// <summary>
    /// Checks the distance between the player, and either tries to catch them or return to home base accordingly.
    /// </summary>
    private void BehaveStandard()
    {
        animator.SetBool("isGrabbing", false);
        animator.SetBool("isBeingGrabbed", false);
        if (target != null)
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
            {
                if (/*Vector3.Distance(target.position, transform.position) > attackRadius*/!playerInRange)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
                }
                else
                {
                    /*gameObject.GetComponent<HandleParent>().attachedObject = target.gameObject;
                    target.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    target.gameObject.GetComponent<HandleParent>().attachedObject = gameObject;
                    transform.position = Vector3.MoveTowards(transform.position, guillotine.position, moveSpeed * Time.deltaTime);
                    target.position = Vector3.MoveTowards(target.position, guillotine.position, moveSpeed * Time.deltaTime);*/
                    /*if (!isOnCooldown)
                    {
                        StartCoroutine(KillPlayerCoroutine());
                    }
                    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !playerInputCoroutineStarted)
                    {
                        playerInputValue = (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")) / 2;
                        StartCoroutine(CheckIfSpammingButtonCoroutine());
                    }*/
                    state = State.Grabbing;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    private void BehaveGrabbing()
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

    private void BehaveBeingGrabbed()
    {

    }

    /// <summary>
    /// Attaches itself to the player and attempts to kill them.
    /// </summary>
    private IEnumerator KillPlayerCoroutine()
    {
        target.gameObject.GetComponentInChildren<MovementPressKey>().shouldBeFast = true;
        gameObject.GetComponent<HandleParent>().attachedObject = target.gameObject;
        target.gameObject.GetComponent<PlayerMovement>().enabled = false;
        target.gameObject.GetComponent<HandleParent>().attachedObject = gameObject;
        transform.position = Vector3.MoveTowards(transform.position, guillotine.position, moveSpeed * Time.deltaTime);
        target.position = Vector3.MoveTowards(target.position, guillotine.position, moveSpeed * Time.deltaTime);
        //target.gameObject.GetComponent<PlayerMovement>().enabled = true;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHandle"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("PlayerHandle"))
        {
            playerInRange = false;
        }
    }
}
