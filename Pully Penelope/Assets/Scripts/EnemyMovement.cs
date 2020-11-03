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
                    gameObject.GetComponent<HandleParent>().attachedObject = target.gameObject;
                    target.gameObject.GetComponent<PlayerMovement>().enabled = false;
                    target.gameObject.GetComponent<HandleParent>().attachedObject = gameObject;
                    transform.position = Vector3.MoveTowards(transform.position, guillotine.position, moveSpeed * Time.deltaTime);
                    target.position = Vector3.MoveTowards(target.position, guillotine.position, moveSpeed * Time.deltaTime);
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, homePosition, moveSpeed * Time.deltaTime);
            }
        }
    }
}
