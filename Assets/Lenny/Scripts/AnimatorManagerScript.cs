using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorManagerScript : MonoBehaviour
{

    [Header("Name of the Animation to play")]
    [SerializeField]
    string animationOnStart;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        anim.SetTrigger(animationOnStart);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
