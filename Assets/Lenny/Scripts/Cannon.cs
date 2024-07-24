using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private GameObject ball;

    [SerializeField] private GameObject barrel;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ShootCannonBall();
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            barrel.transform.Rotate(5, 0, 0);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            barrel.transform.Rotate(-5, 0, 0);
        }
    }

    void ShootCannonBall()
    {
        Instantiate(ball, transform.position, Quaternion.identity);
    }
}