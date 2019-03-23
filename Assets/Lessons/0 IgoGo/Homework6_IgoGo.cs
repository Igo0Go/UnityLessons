using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homework6_IgoGo : MonoBehaviour {

    [Range(0,1)]
    public float distance;

    private Rigidbody rb;
    private bool OnGround
    {
        get
        {
            if(Physics.Raycast(transform.position, -Vector3.up, distance))
            {
                return true;
            }
            return false;
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        if(Input.GetKeyDown(KeyCode.Space) && OnGround)
        {
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
        }
    }
}
