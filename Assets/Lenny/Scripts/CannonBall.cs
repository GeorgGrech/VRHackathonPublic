using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBall : MonoBehaviour
{

    float thrust = 20f;
    private Rigidbody rb;

    [SerializeField] private bool isGrabable = false;
    [SerializeField] private GameObject effect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Vector3 dir = Quaternion.AngleAxis(transform.rotation.y, Vector3.right) * Vector3.forward;
        rb.velocity = dir * thrust;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isGrabable)
        {
            StartCoroutine(ExplodeAndDestroy());
        }
    }

    IEnumerator ExplodeAndDestroy()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        Instantiate(effect, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }
}
