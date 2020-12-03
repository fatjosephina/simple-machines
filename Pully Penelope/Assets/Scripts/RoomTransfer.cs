using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the room transfer mechanic.
/// </summary>
public class RoomTransfer : MonoBehaviour
{
    [Tooltip("The change of the camera in between rooms.")]
    [SerializeField]
    private Vector3 cameraChange = new Vector3(0, 8, 0);

    [Tooltip("The change of the player in between rooms.")]
    [SerializeField]
    private Vector3 playerChange = new Vector3(0, 2, 0);

    private Transform cameraTransform;
    private float cameraMovementDuration = 0.5f;

    private void Start()
    {
        cameraTransform = Camera.main.transform;
    }

    private void LateUpdate()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Box"))
        {
            collision.gameObject.transform.position += playerChange;
            if (collision.gameObject.GetComponent<HandleParent>().attachedObject != null)
            {
                collision.gameObject.GetComponent<HandleParent>().attachedObject.transform.position += playerChange;
            }
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.GetComponent<HandleParent>().attachedObject.CompareTag("Player"))
            {
                //cameraTransform.position = Vector3.Lerp(cameraTransform.position, cameraTransform.position + cameraChange, cameraSmoothing);
                //cameraTransform.position += cameraChange;
                //shouldCameraLerp = true;
                StartCoroutine(ChangeCameraPositionCoroutine());
            }
        }
    }

    /// <summary>
    /// Changes the camera position smoothly
    /// </summary>
    private IEnumerator ChangeCameraPositionCoroutine()
    {
        float interpolationValue = 0f;
        Vector3 currentPosition = cameraTransform.position;
        while (interpolationValue < 1.0f)
        {
            interpolationValue += Time.deltaTime * (Time.timeScale / cameraMovementDuration);
            cameraTransform.position = Vector3.Lerp(currentPosition, currentPosition + cameraChange, interpolationValue);
            yield return null;
        }
    }
}
