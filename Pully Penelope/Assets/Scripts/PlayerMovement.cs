using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    private new Rigidbody2D rigidbody;
    private Vector3 positionChange;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        positionChange = Vector3.zero;
        positionChange.x = Input.GetAxisRaw("Horizontal");
        positionChange.y = Input.GetAxisRaw("Vertical");

        /*if (positionChange != Vector3.zero)
        {
            MovePlayer();
        }*/
    }

    private void FixedUpdate()
    {
        if (positionChange != Vector3.zero)
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        rigidbody.MovePosition(transform.position + positionChange.normalized * moveSpeed * Time.deltaTime);
    }
}
