using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationType
{
    Teleport,
    Rotate,
    QuaternionType,
}
public enum PositionType
{
    global,
    local
}

public delegate void SimpleHandler(); //делегат, используемый для активации разных методов в зависимости от выбранных настроек

public class SampleTransform : MonoBehaviour {

    [Header("Позиция")]
    public PositionType positionType;
    public Vector3 samplePosition;

    [Header("Вращение")]
    public RotationType rotationType;
    public Vector3 sampleRotation;
    [Range(0,10)] public float rotationSpeed;
    public float thresholdValue;

    [Space(20)]
    public bool startRotate;

    private Vector3 result;
    private SimpleHandler positionAction;
    private SimpleHandler rotationAction;

    private PositionType PositionType
    {
        get
        {
            return _posType;
        }
        set
        {
            _posType = value;
            positionAction = null;
            switch (_posType)
            {
                case PositionType.global:
                    positionAction = SetGlobalPos;
                    break;
                case PositionType.local:
                    positionAction = SetLocalPos;
                    break;
            }
        }
    }
    private PositionType _posType;

    private RotationType RotationType
    {
        get
        {
            return _rotType;
        }
        set
        {
            _rotType = value;
            rotationAction = null;
            switch (rotationType)
            {
                case RotationType.Teleport:
                    rotationAction = RotateTeleport;
                    break;
                case RotationType.Rotate:
                    rotationAction = RotateMethod;
                    break;
                case RotationType.QuaternionType:
                    rotationAction = QuaternionRotate;
                    break;
            }
        }
    }
    private RotationType _rotType;

    // Use this for initialization
    void Start () {
        UpdateTypes();
        result = Vector3.zero;
    }
	
	// Update is called once per frame
	void Update () {

        UpdateTypes();
        positionAction();
        if(startRotate)
        {
            rotationAction();
        }
	}

    private void UpdateTypes()
    {
        PositionType = positionType;
        RotationType = rotationType;
    }


    public void SetGlobalPos()
    {
        transform.position = samplePosition;
    }
    public void SetLocalPos()
    {
        transform.localPosition = samplePosition;
    }

    public void RotateTeleport()
    {
        //Euler - представление Quaternuon  в виде Эйлеровых углов [0;360]
        Quaternion localRot = Quaternion.Euler(sampleRotation);
        transform.localRotation = localRot;
        startRotate = false;
    }
    public void RotateMethod()
    {
        Vector3 multiplicator = Vector3.one;

        Quaternion xRot, yRot, zRot;

        xRot = Quaternion.Euler(sampleRotation.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);
        yRot = Quaternion.Euler(transform.localRotation.eulerAngles.x, sampleRotation.y, transform.localRotation.eulerAngles.z);
        zRot = Quaternion.Euler(sampleRotation.x, transform.localRotation.eulerAngles.y, transform.localRotation.eulerAngles.z);



        float xAngel, yAngel, zAngel;

        //Angel - угол между двумя вращениями, представленными в Quaternion
        xAngel = Quaternion.Angle(transform.localRotation, xRot);
        yAngel = Quaternion.Angle(transform.localRotation, yRot);
        zAngel = Quaternion.Angle(transform.localRotation, zRot);
        
        multiplicator.x = transform.localRotation.eulerAngles.x > sampleRotation.x ? -1 : 1;
        multiplicator.y = transform.localRotation.eulerAngles.y > sampleRotation.y ? -1 : 1;
        multiplicator.z = transform.localRotation.eulerAngles.z > sampleRotation.z ? -1 : 1;

        if (result == Vector3.one)
        {
            RotateTeleport();
            startRotate = false;
            result = Vector3.zero;
        }
        else
        {
            if (xAngel > thresholdValue)
            {                                                                   //время с предыдущего кадра
                transform.Rotate(transform.right, multiplicator.x * rotationSpeed * Time.deltaTime); //Rotate - вращение вокруг оси axis на angel градусов
            }
            else
            {
                result.x = 1;
            }

            if (yAngel > thresholdValue)
            {
                transform.Rotate(transform.up, multiplicator.y * rotationSpeed * Time.deltaTime);
            }
            else
            {
                result.y = 1;
            }

            if (zAngel> thresholdValue)
            {
                transform.Rotate(transform.forward, multiplicator.z * rotationSpeed * Time.deltaTime);
            }
            else
            {
                result.z = 1;
            }
        }
    }
    public void QuaternionRotate()
    {
        Quaternion targetRot = Quaternion.Euler(sampleRotation);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRot, rotationSpeed * Time.deltaTime); //Slerp - плавная линейная интерполяция значений поворота
        if(Quaternion.Angle(transform.localRotation,targetRot) <= thresholdValue)
        {
            startRotate = false;
        }
    }
}
