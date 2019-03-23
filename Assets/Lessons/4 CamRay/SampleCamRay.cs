using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SampleCamRay : MonoBehaviour {

    public LayerMask secondMask;
    public LayerMask specMask;
    [Range(0,200)]
    public float shootDistance;
    public LineRenderer lineRenderer;
    public Transform rayStartPoint;

    private Camera cam;
    private LayerMask defaultMask;
    private bool Visor
    {
        get
        {
            return _visor;
        }
        set
        {
            _visor = value;
            if(_visor)
            {
                cam.cullingMask = secondMask;
            }
            else
            {
                cam.cullingMask = defaultMask;
            }
        }
    }
    private bool _visor;

    void Start () {
        cam = GetComponent<Camera>();
        defaultMask = cam.cullingMask;
	}
	
	void Update () {
        ChangeMask();
        MyRay();
	}

    private void ChangeMask()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            Visor = !Visor;
        }
    }
    private void MyRay()
    {
        if(Input.GetMouseButton(0))
        {
            RaycastHit hit;
            if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, shootDistance, specMask))
            {
                lineRenderer.SetPosition(0, rayStartPoint.position);
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.enabled = true;
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }
        else
        {
            lineRenderer.enabled = false;
        }
    }
}
