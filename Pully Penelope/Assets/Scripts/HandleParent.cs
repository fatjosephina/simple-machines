using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Is attached to the parent of a handle, and is used to set what object they are attached to.
/// </summary>
public class HandleParent : MonoBehaviour
{
    public GameObject attachedObject { get; set; }
}
