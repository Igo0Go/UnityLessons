using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerReactor : MonoBehaviour {

    public Image aim;
    public Slider slider;
    public Vector3 safePos;

    private Homework3_IgoGo player;
    private float standartGrav;
    private Transform cam;

    private void Start()
    {
        safePos = transform.position;
        player = GetComponent<Homework3_IgoGo>();
        standartGrav = player.grav;
        slider.value = 0;
    }

    private void Update()
    {
        MyRay();
    }

    private void MyRay()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 3f))
        {
            if(hit.collider.tag.Equals("Finish"))
            {
                aim.color = Color.red;
                if (Input.GetMouseButtonDown(0))
                {
                    Destroy(hit.collider.gameObject);
                }
            }
            else
            {
                aim.color = Color.white;
            }
        }
        else
        {
            aim.color = Color.white;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Water"))
        {
            player.grav = 0.5f;
            return;
        }

        if (other.tag.Equals("DeadZone"))
        {
            transform.position = safePos;
            return;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag.Equals("Water"))
        {
            slider.value += 10 * Time.deltaTime;
            return;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Water"))
        {
            player.grav = standartGrav;
        }
    }
}
