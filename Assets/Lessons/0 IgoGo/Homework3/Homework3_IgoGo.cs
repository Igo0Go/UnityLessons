using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homework3_IgoGo : MonoBehaviour {

    [Range(1, 10)]
    public float speed;
    [Range(1, 360)]
    public float camRotateSpeed;
    [Range(0, 90)]
    public float maxYAngel;
    [Range(-90, 0)]
    public float minYAngel;


    [Space(10)]
    [Header("Насройки гравитации")]
    [Range(0, 100)]
    public float grav;
    [Range(-100, 100)]
    public float jumpForce;

    [Space(20)]
    public GameObject[] spawnObjects;

    private CharacterController characterController;
    private Vector3 dir;
    private float yAxis;
    private float currentYAxisAngel;
    private float currentXAxisAngel;

    private bool withObject;
    private int spawnNumber;
    private GameObject currentObject;

    // Use this for initialization
    void Start () {
        Cursor.lockState = CursorLockMode.Locked;
        currentYAxisAngel = transform.eulerAngles.y;
        currentXAxisAngel = transform.eulerAngles.x;
        characterController = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        PlayerMove();
        InstanceMethod();
	}
    private void LateUpdate()
    {
        PlayerRotate();
    }

    private void PlayerMove()
    {
        float h, v;

        h = Input.GetAxis("Horizontal"); 
        v = Input.GetAxis("Vertical");      

        Vector3 camForward = transform.forward;
        camForward.y = 0;

        if (h != 0 || v != 0)
        {
            dir = transform.right * h + camForward * v;
        }
        if (characterController.isGrounded)
        {
            yAxis = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(jumpForce < 0 )
                {
                    Debug.LogError("Ты чё, больной?! Ты ж вниз прыгаешь...");
                }
                yAxis = jumpForce;
            }
        }
        yAxis -= grav * Time.deltaTime;
        dir = new Vector3(dir.x * speed * Time.deltaTime, yAxis * Time.deltaTime, dir.z * speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {                                      
            characterController.Move(dir);
        }
    }
    private void PlayerRotate()
    {
        float mx, my;
        mx = Input.GetAxis("Mouse X");
        my = Input.GetAxis("Mouse Y");

        if(mx!=0 || my != 0)
        {
            currentYAxisAngel += mx * camRotateSpeed * Time.deltaTime;
            currentXAxisAngel -= my * camRotateSpeed * Time.deltaTime;
            currentXAxisAngel = Mathf.Clamp(currentXAxisAngel, minYAngel, maxYAngel);
            transform.rotation = Quaternion.Euler(currentXAxisAngel, currentYAxisAngel, 0);
        }
    }

    private void InstanceMethod()
    {
        if(Input.GetButtonDown("IgoGo_Axis"))
        {
            withObject = true;
            InstanceNew();
        }
        if(Input.GetButtonUp("IgoGo_Axis"))
        {
            withObject = false;
            currentObject.transform.parent = null;
            currentObject = null;
        }

        if(withObject)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                spawnNumber++;
                if (spawnNumber > spawnObjects.Length - 1)
                {
                    spawnNumber = 0;
                }
                InstanceNew();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                spawnNumber--;
                if (spawnNumber < 0)
                {
                    spawnNumber = spawnObjects.Length - 1;
                }
                InstanceNew();
            }
        }
        
    }

    private void InstanceNew()
    {
        if(currentObject != null)
        {
            Destroy(currentObject);
        }
        currentObject = Instantiate(spawnObjects[spawnNumber], transform);
        currentObject.transform.position = transform.position + transform.forward * 1.5f;
    }
}
