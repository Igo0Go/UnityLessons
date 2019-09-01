using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleTPS : MonoBehaviour {

    public Transform cameraHandler;
    public Transform cam;
    public GameObject instanceObject;

    [Range(1,10)]
    public float speed;
    [Range(1,360)]
    public float camRotateSpeed;

    [Header("Настройки гравитации")]
    [Range(0,100)]
    public float grav;
    [Range(0,100)]
    public float jumpForce;


    private CharacterController characterController;
    private Vector3 dir;
    private float yAxis;
    private bool moveState;
    private GameObject currentObject;

	// Use this for initialization
	void Start () {
        moveState = false;
        characterController = GetComponent<CharacterController>();// GameObject.GetComponent<type>() если есть, вернёт компонент type, имеющийся на GameObject. Иначе null
        Cursor.lockState = CursorLockMode.Locked; // отображение курсора
        dir = transform.forward;
        CamRotate();
    }
	
	// Update is called once per frame
	void Update () {
        if(!moveState)
        {
            PlayerMove();
        }
        InstanceObject();
    }

    private void LateUpdate()
    {
        if(!moveState)
        {
            CamRotate();
        }
    }

    private void CamRotate()
    {
        float mx;
        mx = Input.GetAxis("Mouse X"); //отслеживание ввода: Mouse X (мышь, горизонталь) 
                                                  //(заранее заготовленная настройка ввода, возвращающая положительные значения, если мышь
        cameraHandler.position = transform.position;                                    //движется вправо, отрицательные, если влево, иначе 0

        if(mx != 0)
        {
            cameraHandler.Rotate(transform.up, mx * camRotateSpeed * Time.deltaTime);
        }
        cam.LookAt(cameraHandler);
    }
    private void PlayerMove()
    {
        float h, v;

        h = Input.GetAxis("Horizontal"); //можно посмотреть, поменять и создать новые Input в редакторе. Для этого используйте
                                            //Edit /ProjectSettings/Input
        v = Input.GetAxis("Vertical");      //там будут уже имеющиеся Axis. Посмотрите кнопки для Vertical и Horizontal

        Vector3 camForward = cam.forward;
        camForward.y = 0;

        if (h != 0 || v != 0)
        {
            dir = cam.right * h + camForward * v;
            if (dir != Vector3.zero)
            {
                transform.forward = dir;
            }
        }
        if (characterController.isGrounded)
        {
            yAxis = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                yAxis = jumpForce;
            }
        }
        yAxis -= grav * Time.deltaTime;
        dir = new Vector3(dir.x * speed * Time.deltaTime, yAxis * Time.deltaTime, dir.z * speed * Time.deltaTime);

        if (dir != Vector3.zero)
        {                                       //движение с помощью CharacterControler отличается от редактирования позиции тем, что
                                                //учитываются ступеньки и уклоны
            characterController.Move(dir); 
        }
    }

    private void InstanceObject()
    {
        moveState = Input.GetKey(KeyCode.P);  //удерживание P

        if (moveState)
        {
            if(currentObject == null)
            {
                currentObject = Instantiate(instanceObject, transform.position + Vector3.up * 1.5f, Quaternion.identity);
                                        //Instanstiate - создание нового объекта из префаба instanceObject
            }                      //в точке transform.position + Vector3.up * 1.5f и с вращением Quaternion.identity (identity - стандартное)
            float h, v;
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");

            Vector3 camForward = cam.forward;
            camForward.y = 0;

            if (h != 0 || v != 0)
            {
                dir = cam.right * h + camForward * v;
                if (dir != Vector3.zero)
                {
                    currentObject.transform.position += dir * Time.deltaTime;
                }
            }
        }
        else
        {
            if(currentObject != null)
            {
                Destroy(currentObject, 5); //Удаление currentObject через 5 секунд
                currentObject = null;
            }
        }

        if(Input.GetKeyUp(KeyCode.P))
        {
            LogMessage();
            Invoke("LogError", 5); //вызов метода LogError через 5 секунд
        }
    }

    private void LogMessage()
    {
        Debug.Log("Поставили морковку"); //вывод сообщения в консоль
    }
    private void LogError()
    {
        Debug.LogError("Мы потеряли морковку!"); //вывод в консоль сообщения об ошибке
    }
}
