using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes care of all handle objects.
/// </summary>
public class Handle : MonoBehaviour
{
    private Vector2 HandleOrientation { get; set; }
    private string HandleAxis { get; set; }

    private void Start()
    {
        SetHandleData();
    }

    /// <summary>
    /// Sets the handle's properties and fields, such as HandleOrientation and HandleAxis.
    /// </summary>
    private void SetHandleData()
    {
        float handleX = HandleOrientation.x;
        float handleY = HandleOrientation.y;
        switch (name)
        {
            case "DHandle":
                handleX = 0;
                handleY = 1;
                HandleAxis = "Vertical";
                break;
            case "UHandle":
                handleX = 0;
                handleY = -1;
                HandleAxis = "Vertical";
                break;
            case "RHandle":
                handleX = -1;
                handleY = 0;
                HandleAxis = "Horizontal";
                break;
            case "LHandle":
                handleX = 1;
                handleY = 0;
                HandleAxis = "Horizontal";
                break;
        }
        HandleOrientation = new Vector2(handleX, handleY);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!gameObject.CompareTag("PlayerHandle"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.GetComponent<HandleParent>().attachedObject == null || collision.gameObject.GetComponent<HandleParent>().attachedObject == transform.parent.gameObject)
                {
                    collision.gameObject.GetComponent<PlayerMovement>().HandleEntry(name, HandleAxis, HandleOrientation, gameObject.transform.parent.gameObject);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!gameObject.CompareTag("PlayerHandle"))
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerMovement>().HandleExit();
            }
        }
    }
}
