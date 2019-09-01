using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum IgoGo_MoveType
{
    teleport,
    translate,
    slerp,
    steps
}

public class Homework2_IgoGo : MonoBehaviour {

    public IgoGo_MoveType IgoGo_Move;

    [Space(20)]
    public Vector3 targetPosition;
    [Range(0,10)]
    public float speed;
    [Range(0,1)]
    public float thresholdValue;
    private float Distance
    {
        get
        {
            return Vector3.Distance(transform.localPosition, targetPosition);
        }
    }

    private IgoGo_MoveType MoveType
    {
        get
        {
            return _moveType;
        }
        set
        {
            _moveType = value;
            moveAction = null;
            switch (_moveType)
            {
                case IgoGo_MoveType.teleport:
                    moveAction = TeleportMove;
                    break;
                case IgoGo_MoveType.translate:
                    moveAction = TranslateMove;
                    break;
                case IgoGo_MoveType.slerp:
                    moveAction = SlerpMove;
                    break;
                case IgoGo_MoveType.steps:
                    moveAction = StepsMove;
                    break;
            }
        }
    }
    public bool move;

    private IgoGo_MoveType _moveType;
    private SimpleHandler moveAction;



	// Use this for initialization
	void Start () {
        MoveType = IgoGo_Move;
	}
	
	// Update is called once per frame
	void Update () {
        MoveType = IgoGo_Move;

        if (move)
        {
            moveAction();
        }
	}

    private void TeleportMove()
    {
        transform.localPosition = targetPosition;
        move = false;
    }
    private void TranslateMove()
    {
        Vector3 direction = targetPosition - transform.localPosition;
        if(Distance > thresholdValue)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = targetPosition;
            move = false;
        }
    }
    private void SlerpMove()
    {
        if (Distance > thresholdValue)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = targetPosition;
            move = false;
        }
    }
    private void StepsMove()
    {
        Vector3 direction = targetPosition - transform.localPosition;
        if (Distance > thresholdValue)
        {
            transform.localPosition += direction.normalized * speed * Time.deltaTime;
        }
        else
        {
            transform.localPosition = targetPosition;
            move = false;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.TransformPoint(targetPosition));
        Gizmos.DrawCube(transform.transform.TransformPoint(targetPosition), transform.localScale);
    }

}
