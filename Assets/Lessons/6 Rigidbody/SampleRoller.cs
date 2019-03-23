using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleRoller : MonoBehaviour {

    [Range(0,10)]
    public float moveForce;
    public Transform cam;
    public Transform camPoint;
    [Range(0,200)]
    public float rotSpeed;

    private Vector3 dirVector;
    private Rigidbody rb;
    private Vector3 safePos;

	// Use this for initialization
	void Start () {
        safePos = transform.position;
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        RollerMove();
	}

    private void LateUpdate()
    {
        CamPosition();
    }

    private void RollerMove()
    {
        float h, v;
        v = Input.GetAxis("Vertical");
        h = Input.GetAxis("Horizontal");

        if(v != 0 || h != 0)
        {
            Vector3 camForward = cam.forward;
            camForward.y = 0;

            dirVector = cam.right * h + camForward * v;
            rb.AddForce(dirVector * moveForce * Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
    private void CamPosition()
    {
        camPoint.position = transform.position;

        float rot = Input.GetAxis("RollerRotateAxis");

        if(rot != 0)
        {
            camPoint.Rotate(camPoint.up, - rot * rotSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("DeadZone"))
        {
            transform.position = safePos;
            return;
        }

        Homework2_IgoGo mover;
        if(MyGetComponent(other.gameObject, out mover))
        {
            mover.move = true;
        }
    }

    private bool MyGetComponent<T>(GameObject origin, out T component)
    {
        bool result = true;
        component = origin.GetComponent<T>();
        if(component == null)
        {
            result = false;
        }
        return result;
    }
}
