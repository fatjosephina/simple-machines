using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
        homePosition = transform.position;
    }

    private void Update()
    {
        CheckDistance();
    }

    private void CheckDistance()
    {
        if (target != null)
        {
            if (Vector3.Distance(target.position, transform.position) <= chaseRadius)
            {
                if (Vector3.Distance(target.position, transform.position) > attackRadius)
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
                    if (!isOnCooldown)
                    {
                        StartCoroutine(KillPlayerCo());
                    }
                    if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && !playerInputCoroutineStarted)
                    {
                        playerInputValue = (Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")) / 2;
                        StartCoroutine(CheckIfSpammingButtonCo());
                        Debug.Log(playerInputValue);
                    }
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
            }
        }
    }

    private IEnumerator KillPlayerCo()
    {
        gameObject.GetComponent<HandleParent>().attachedObject = target.gameObject;
        target.gameObject.GetComponent<PlayerMovement>().enabled = false;
        target.gameObject.GetComponent<HandleParent>().attachedObject = gameObject;
        transform.position = Vector3.MoveTowards(transform.position, guillotine.position, moveSpeed * Time.deltaTime);
        target.position = Vector3.MoveTowards(target.position, guillotine.position, moveSpeed * Time.deltaTime);
        target.gameObject.GetComponent<PlayerMovement>().enabled = true;
        yield return null;
    }

    private IEnumerator CheckIfSpammingButtonCo()
    {
        playerInputCoroutineStarted = true;
        yield return new WaitForSeconds(waitTime);
        if ((Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")) / 2 != playerInputValue)
        {
            /*yield return new WaitForSeconds(waitTime);
            if ((Input.GetAxisRaw("Horizontal") + Input.GetAxisRaw("Vertical")) / 2 != playerInputValue)
            {
                if (target != null)
                {*/
            target.gameObject.GetComponent<PlayerMovement>().enabled = true;
            isOnCooldown = true;
                /*}
            }*/
        }
        yield return new WaitForSeconds(cooldownTime);
        isOnCooldown = false;
        playerInputCoroutineStarted = false;
        yield return null;
    }
}
