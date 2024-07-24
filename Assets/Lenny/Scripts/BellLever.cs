using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BellLever : MonoBehaviour
{
    [SerializeField]
    Bell bellScript;

    //bool isRinging = false;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("------------------- COLLISION --------------");
        if (other.tag.Equals("Lever"))
        {
            bellScript.RingBell();
            //isRinging = true;
        }
    }
}
