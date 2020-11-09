using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    private string handleName;
    public Vector2 HandleOrientation { get; private set; }
    public string HandleAxis { get; private set; }

    private void Start()
    {
        handleName = name;
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
}
