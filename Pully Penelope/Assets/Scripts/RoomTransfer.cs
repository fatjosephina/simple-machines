using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the room transfer mechanic.
/// </summary>
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
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            collision.gameObject.transform.position += playerChange;
            //Debug.Log("Success");
            if (collision.gameObject.GetComponent<HandleParent>().attachedObject != null)
            {
                collision.gameObject.GetComponent<HandleParent>().attachedObject.transform.position += playerChange;
            }
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<HandleParent>().attachedObject.CompareTag("Player"))
            {
                cameraTransform.position += cameraChange;
            }
        }
    }
}
