using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pawn : MonoBehaviour
{

    [HideInInspector]
    public Transform tf;
    public Rigidbody rb;
    public float animationCutoffVelocity = 0.001f;
    public Animator anim;
    private Vector3 lastFramePosition;

    private void Awake()
    {
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }

    // Start is called before the first frame update
        void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Animate()
    {
        float velocity = (tf.position - lastFramePosition).magnitude * Time.deltaTime;

        if ( velocity > animationCutoffVelocity)
        {
            anim.SetTrigger("Run");
        }
        else
        {
            anim.SetTrigger("Idle");
        }

        lastFramePosition = tf.position;
    }
    
}
