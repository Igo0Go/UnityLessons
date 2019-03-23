using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homework4_IgoGo : MonoBehaviour {

    [Range(0,200)]
    public float distance;
    [Range(0,100)]
    public float speed;

    public GameObject spawnObj;
    public LayerMask obstacleMask;

    private Camera cam;
    private GameObject teleportPoint;
    private bool move;
    private int angelMove;
    private Vector3 target;
    private float standartAngel;
    private float targetAngel;

    private float Distance
    {
        get
        {
            return Vector3.Distance(transform.position, target);
        }
    }

	// Use this for initialization
	void Start () {
        cam = GetComponent<Camera>();
        standartAngel = cam.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
        ChangeCamAngel();

        if (move)
        {
            MoveToTarget();
        }
        else
        {
            MyRay();
        }
	}

    private void MyRay()
    {
        if (Input.GetKey(KeyCode.F))
        {
            if(teleportPoint == null)
            {
                teleportPoint = Instantiate(spawnObj, transform);
            }
            teleportPoint.transform.position = transform.position + transform.forward * distance;
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, distance, ~obstacleMask))
            {
                teleportPoint.transform.position = hit.point + hit.normal * 1.5f;
            }
        }

        if (Input.GetKeyUp(KeyCode.F))
        {
            teleportPoint.transform.parent = null;
            target = teleportPoint.transform.position;
            targetAngel = 160;
            angelMove = 1;
            move = true;
        }
    }

    private void MoveToTarget()
    {
        if(Distance > 5)
        {
            transform.position = Vector3.Slerp(transform.position, target, speed * Time.deltaTime);
        }
        else
        {
            transform.position = target;
            Destroy(teleportPoint);
            targetAngel = standartAngel;
            move = false;
        }
    }

    private void ChangeCamAngel()
    {
        if(angelMove > 0)
        {
            int multiplicator = 1;
            if (targetAngel < cam.fieldOfView)
            {
                multiplicator = -1;
            }

            if (Mathf.Abs(cam.fieldOfView - targetAngel) > 1)
            {
                cam.fieldOfView += multiplicator * 10;
            }
            else
            {
                cam.fieldOfView = targetAngel;
                if(angelMove == 1)
                {
                    targetAngel = standartAngel;
                }
                else if(angelMove == 2)
                {
                    angelMove = 0;
                }
            }
        }
        
    }
}
