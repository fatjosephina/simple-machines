using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArrowWindow : MonoBehaviour
{
    [SerializeField]
    private Camera uiCamera;

    private Vector3 targetPosition;
    private RectTransform arrowRectTransform;
    private GameObject player;
    private float offset = 90f;
    private float modValue = 360f;

    private void Awake()
    {
        targetPosition = GameObject.FindGameObjectWithTag("Goal").transform.position;
        arrowRectTransform = transform.Find("Arrow").GetComponent<RectTransform>();
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        SetArrowData();
    }

    /// <summary>
    /// If the player exists, sets the arrow data
    /// </summary>
    private void SetArrowData()
    {
        if (player != null)
        {
            SetArrowPositionAndRotation();
            CheckIfOffScreen();
        }
    }

    /// <summary>
    /// Sets the position and rotation of the arrow
    /// </summary>
    private void SetArrowPositionAndRotation()
    {
        Vector3 toPosition = targetPosition;
        Vector3 fromPosition = player.transform.position;
        fromPosition.z = 0f;
        Vector3 direction = (toPosition - fromPosition).normalized;
        float angle = (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) % modValue;
        arrowRectTransform.localEulerAngles = new Vector3(0, 0, angle + offset);
    }

    /// <summary>
    /// Checks if the arrow would be off screen and corrects it
    /// </summary>
    private void CheckIfOffScreen()
    {
        float borderSize = 100f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(targetPosition);
        bool isOffScreen = targetPositionScreenPoint.x <= borderSize || targetPositionScreenPoint.x >= Screen.width - borderSize
            || targetPositionScreenPoint.y <= borderSize || targetPositionScreenPoint.y >= Screen.height - borderSize;

        if (isOffScreen)
        {
            Vector3 cappedTargetScreenPosition = targetPositionScreenPoint;
            if (cappedTargetScreenPosition.x <= borderSize)
            {
                cappedTargetScreenPosition.x = borderSize;
            }
            if (cappedTargetScreenPosition.x >= Screen.width - borderSize)
            {
                cappedTargetScreenPosition.x = Screen.width - borderSize;
            }
            if (cappedTargetScreenPosition.y <= borderSize)
            {
                cappedTargetScreenPosition.y = borderSize;
            }
            if (cappedTargetScreenPosition.y >= Screen.height - borderSize)
            {
                cappedTargetScreenPosition.y = Screen.height - borderSize;
            }

            Vector3 arrowWorldPosition = uiCamera.ScreenToWorldPoint(cappedTargetScreenPosition);
            arrowRectTransform.position = arrowWorldPosition;
            arrowRectTransform.localPosition = new Vector3(arrowRectTransform.localPosition.x, arrowRectTransform.localPosition.y, 0f);
        }
        else
        {
            Vector3 arrowWorldPosition = uiCamera.ScreenToWorldPoint(targetPositionScreenPoint);
            arrowRectTransform.position = arrowWorldPosition;
            arrowRectTransform.localPosition = new Vector3(arrowRectTransform.localPosition.x, arrowRectTransform.localPosition.y, 0f);
        }
    }
}
