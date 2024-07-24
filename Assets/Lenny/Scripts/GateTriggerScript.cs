using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateTriggerScript : MonoBehaviour
{
    [SerializeField]
    GateScript gateScript;

    bool isOpen = true;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && isOpen)
        {
            gateScript.CloseGate();
            isOpen = false;
        }
    }
}
