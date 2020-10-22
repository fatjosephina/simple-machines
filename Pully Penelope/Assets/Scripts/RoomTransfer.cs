using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{
    [SerializeField]
    private Vector3 cameraChange = new Vector3(0, 8, 0);

    [SerializeField]
    private Vector3 playerChange = new Vector3(0, 2, 0);

    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraTransform.position += cameraChange;
            collision.gameObject.transform.position += playerChange;
        }
    }
}
