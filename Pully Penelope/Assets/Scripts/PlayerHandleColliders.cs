using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandleColliders : MonoBehaviour
{
    public void EnableUHandle()
    {
        EnableHandle("UHandle");
    }
    public void EnableDHandle()
    {
        EnableHandle("DHandle");
    }
    public void EnableRHandle()
    {
        EnableHandle("RHandle");
    }
    public void EnableLHandle()
    {
        EnableHandle("LHandle");
    }

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
