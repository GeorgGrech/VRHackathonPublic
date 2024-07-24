using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateLever : MonoBehaviour
{
    [SerializeField]
    GateScript gateScript;

    bool isClosed = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Lever") && isClosed)
        {
            gateScript.OpenGate();
            isClosed = false;
        }
    }
}
