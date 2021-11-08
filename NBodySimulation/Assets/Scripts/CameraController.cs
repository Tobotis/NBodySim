using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    public float smoothSpeed = 0.05f;
    private Vector3 Origin;
    private Vector3 Difference;
    public static Vector3 resetCamera;
    private bool drag = false;
    private Simulator sim;

    private void Awake()
    {
        resetCamera = Camera.main.transform.position;
        sim = GameObject.Find("NBodySimulator").GetComponent<Simulator>();
    }
    private void FixedUpdate()
    {
        if(Simulator.focusedBody != null)
        {
            if (Simulator.focusedBody.draggingObject == false)
            {
                FollowFocused();
            }
        }
        else
        {
            DragMove();
        }
    }
   
    void Update()
    {

        if(Input.GetAxis("Mouse ScrollWheel") > 0f && this.gameObject.GetComponent<Camera>().orthographicSize >1)
        {
            this.gameObject.GetComponent<Camera>().orthographicSize--;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            this.gameObject.GetComponent<Camera>().orthographicSize++;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    Simulator.ChangeFocus(hit.transform.gameObject);
                }
            }
        }
    }
    
    
    
    void FollowFocused()
    {
        if(Simulator.focusedBody != null)
        {

            Vector3 offset = new Vector3(0, 15, 0);
            Vector3 desPos = Simulator.focusedBody.gameObject.transform.position + offset;
            Vector3 smoothPos = Vector3.Lerp(transform.position, desPos, smoothSpeed);
            transform.position = smoothPos;
        }
    }
    void DragMove()
    {
        if (Input.GetMouseButton(0))
        {
            Difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - Camera.main.transform.position;
            if (!drag)
            {
                drag = true;
                Origin = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
        else
        {
            drag = false;
        }
        if (drag)
        {
            Camera.main.transform.position = Origin - Difference;
        }
    }
}
