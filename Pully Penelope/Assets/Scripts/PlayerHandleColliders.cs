using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandleColliders : MonoBehaviour
{
    /// <summary>
    /// Enables the up handle
    /// </summary>
    public void EnableUHandle()
    {
        EnableHandle("UHandle");
    }
    /// <summary>
    /// Enables the down handle
    /// </summary>
    public void EnableDHandle()
    {
        EnableHandle("DHandle");
    }
    /// <summary>
    /// Enables the right handle
    /// </summary>
    public void EnableRHandle()
    {
        EnableHandle("RHandle");
    }
    /// <summary>
    /// Enables the left handle
    /// </summary>
    public void EnableLHandle()
    {
        EnableHandle("LHandle");
    }

    /// <summary>
    /// Takes whatever handle is entered as a parameter and enables it while disabling all others attached to the game object
    /// </summary>
    private void EnableHandle(string handleName)
    {
        foreach (Transform child in transform)
        {
            if (child.name != "PressKeyMovement")
            {
                if (child.name == handleName)
                {
                    child.GetComponent<BoxCollider2D>().enabled = true;
                }
                else
                {
                    child.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }
    }
}
