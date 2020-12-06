using System.Collections;
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
    private float cooldownTime = 2f;
    public bool playerInRange = false;
    private State state;
    private Animator animator;
    private Vector3 lastPosition;
    public bool isBeingGrabbed = false;

    private AudioSource attachSound;
    private bool hasPlayedAttachSound = false;

    [Tooltip("The sound to be played when the player is in range.")]
    [SerializeField]
    private AudioSource playerSpottedSound;
    private bool hasPlayedPlayerSpottedSound = false;

    [Tooltip("The sound to be played when the enemy is vulnerable.")]
    [SerializeField]
    private AudioSource vulnerableSound;

    private enum State
    {
        Standard,
        Grabbing,
        BeingGrabbed,
        Cooldown
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
        attachSound = GameObject.Find("AttachSound").GetComponent<AudioSource>();
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
        else if (state == State.Cooldown)
        {
            BehaveCooldown();
        }
    }

    /// <summary>
    /// Sets the enemy's orientation
    /// </summary>
    private void SetOrientation()
    {
        if (state != State.BeingGrabbed)
        {
            if (transform.position != lastPosition)
            {
                float differenceX = transform.position.x - lastPosition.x;
                float differenceY = transform.position.y - lastPosition.y;
                if (differenceX >= differenceY)
                {
                    if (differenceX > 0)
                    {
                        differenceX = Mathf.Ceil(differenceX);
                    }
                    else
                    {
                        differenceX = Mathf.Floor(differenceX);
                    }
                    animator.SetFloat("orientationY", 0);
                    animator.SetFloat("orientationX", differenceX);
                }
                else
                {
                    if (differenceY > 0)
                    {
                        differenceY = Mathf.Ceil(differenceY);
                    }
                    else
                    {
                        differenceY = Mathf.Floor(differenceY);
                    }
                    animator.SetFloat("orientationX", 0);
                    animator.SetFloat("orientationY", differenceY);
                }
            }
            else
            {
                if (transform.position == homePosition)
                {
                    animator.SetFloat("orientationX", 0);
                    animator.SetFloat("orientationY", -1);
                }
            }
        }
        else
        {
            float orientationX = GetComponent<HandleParent>().attachedObject.GetComponent<Animator>().GetFloat("orientationX");
            float orientationY = GetComponent<HandleParent>().attachedObject.GetComponent<Animator>().GetFloat("orientationY");
            animator.SetFloat("orientationX", orientationX);
            animator.SetFloat("orientationY", orientationY);
        }
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
                    if (!hasPlayedPlayerSpottedSound)
                    {
                        playerSpottedSound.Play();
                        hasPlayedPlayerSpottedSound = true;
                    }
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
                    hasPlayedPlayerSpottedSound = false;
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
            if (!hasPlayedAttachSound)
            {
                attachSound.Play();
                hasPlayedAttachSound = true;
            }
            if (!isOnCooldown)
            {
                StartCoroutine(KillPlayerCoroutine());
            }
            else
            {
                hasPlayedAttachSound = false;
                state = State.Cooldown;
            }
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !playerInputCoroutineStarted)
            {
                playerInputValue = (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")) / 2;
                StartCoroutine(CheckIfSpammingButtonCoroutine());
            }
        }
        else
        {
            hasPlayedAttachSound = false;
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
            if (!vulnerableSound.isPlaying)
            {
                vulnerableSound.Play();
            }
        }
        else
        {
            vulnerableSound.Stop();
            isBeingGrabbed = false;
            isOnCooldown = true;
            state = State.Cooldown;
        }
    }

    /// <summary>
    /// If the player is on cooldown, calls the InitiateCooldown() coroutine.
    /// </summary>
    private void BehaveCooldown()
    {
        if (!isBeingGrabbed)
        {
            if (!isOnCooldown)
            {
                state = State.Standard;
            }
            else
            {
                animator.SetBool("isGrabbing", false);
                animator.SetBool("isBeingGrabbed", false);
                StartCoroutine(InitiateCooldown());
            }
        }
        else
        {
            state = State.BeingGrabbed;
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
        playerInputCoroutineStarted = false;
        yield return null;
    }

    /// <summary>
    /// Waits for several seconds then sets the isOnCooldown to false.
    /// </summary>
    private IEnumerator InitiateCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
    }

    private void OnDestroy()
    {
        state = State.BeingGrabbed;
        if (vulnerableSound != null)
        {
            vulnerableSound.Stop();
        }
        if (GameObject.FindWithTag("Player") != null)
        {
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().enabled = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Physics2D.IgnoreCollision(collision.collider, gameObject.GetComponent<Collider2D>());
        }
    }
}
