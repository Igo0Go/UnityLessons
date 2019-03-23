using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SampleNavMesh : MonoBehaviour {

    public NavMeshAgent agent;
    private Camera cam;

    private Vector3 offset;
	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        offset = transform.position - agent.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        MyMouse();
	}

    private void LateUpdate()
    {
        HoldPos();
    }

    private void MyMouse()
    {
        if(Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 200))
            {
                Vector3 pos = hit.point + hit.normal * 1;
                agent.destination = pos;
            }
        }
    }

    private void HoldPos()
    {
        transform.position = agent.transform.position + offset;
    }
}
