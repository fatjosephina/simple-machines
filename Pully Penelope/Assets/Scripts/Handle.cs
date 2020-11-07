using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handle : MonoBehaviour
{
    public string Name { get; private set; }
    private Vector2 handleOrientation;
    public string handleAxis { get; private set; }

    private void Start()
    {
        Name = name;
        switch (Name)
        {
            case "DHandle":
                handleOrientation.x = 0;
                //positionChange.x = handleOrientation.x;
                handleOrientation.y = 1;
                handleAxis = "Vertical";
                break;
            case "UHandle":
                handleOrientation.x = 0;
                //positionChange.x = handleOrientation.x;
                handleOrientation.y = -1;
                handleAxis = "Vertical";
                break;
            case "RHandle":
                handleOrientation.x = -1;
                handleOrientation.y = 0;
                //positionChange.y = handleOrientation.y;
                handleAxis = "Horizontal";
                break;
            case "LHandle":
                handleOrientation.x = 1;
                handleOrientation.y = 0;
                //positionChange.y = handleOrientation.y;
                handleAxis = "Horizontal";
                break;
        }
    }
}
