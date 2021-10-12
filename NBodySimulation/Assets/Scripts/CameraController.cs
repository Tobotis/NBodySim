using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraController : MonoBehaviour
{
    public Button exitFocuseeButton;
    public Button deleteButton;
    public static int focusedIndex = -1;
    public float smoothSpeed = 0.05f;
    public static InputField massInput;
    public static InputField xInput;
    public static InputField yInput;
    public static InputField xPosInput;
    public static InputField yPosInput;
    public Text moveText;
    private Vector3 Origin;
    private Vector3 Difference;
    public static Vector3 ResetCamera;
    private bool drag = false;

    private void Awake()
    {
        massInput = GameObject.Find("MassInput").GetComponent<InputField>();
        xInput = GameObject.Find("VelocityInputX").GetComponent<InputField>();
        yInput = GameObject.Find("VelocityInputY").GetComponent<InputField>();
        xPosInput = GameObject.Find("PosInputX").GetComponent<InputField>();
        yPosInput = GameObject.Find("PosInputY").GetComponent<InputField>();
        ResetCamera = Camera.main.transform.position;
    }
    private void FixedUpdate()
    {
        if(focusedIndex != -1)
        {
            moveText.gameObject.SetActive(true);
            if (Simulator.bodies[focusedIndex].moving == false)
            {
                
                FollowFocused();
            }
            }
        else
        {
            moveText.gameObject.SetActive(false);
            DragMove();
        }
    }
   
    void Update()
    {
        if(focusedIndex != -1 && Simulator.paused)
        {
            deleteButton.gameObject.SetActive(true);
        }
        else{
            deleteButton.gameObject.SetActive(false);
        }
        if(focusedIndex == -1)
        {
            
            exitFocuseeButton.gameObject.SetActive(false);
            massInput.gameObject.SetActive(false); xInput.gameObject.SetActive(false); yInput.gameObject.SetActive(false);xPosInput.gameObject.SetActive(false); yPosInput.gameObject.SetActive(false);
        }
        else
        {
            
            exitFocuseeButton.gameObject.SetActive(true);
        }
        if (!Simulator.paused)
        {
            if(focusedIndex != -1)
            {
                xInput.text = Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.x.ToString();
                yInput.text = Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.z.ToString();
                yPosInput.text = Simulator.bodies[focusedIndex].transform.position.z.ToString();
                xPosInput.text = Simulator.bodies[focusedIndex].transform.position.x.ToString();
            }
            massInput.interactable = false; xInput.interactable = false; yInput.interactable = false;xPosInput.interactable = false; yPosInput.interactable = false;
        }
        else{
            massInput.interactable = true; xInput.interactable = true; yInput.interactable = true;xPosInput.interactable = true; yPosInput.interactable = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform != null)
                {
                    ChangeFocus(hit.transform.gameObject);
                }
            }
        }
        if(Input.GetAxis("Mouse ScrollWheel") > 0f && this.gameObject.GetComponent<Camera>().orthographicSize >1)
        {
            this.gameObject.GetComponent<Camera>().orthographicSize--;
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            this.gameObject.GetComponent<Camera>().orthographicSize++;
        }
    }
    public static void ChangeFocus(GameObject go)
    {

        if(focusedIndex != -1)
        {
            Simulator.bodies[focusedIndex].GetComponent<Renderer>().material.color = Color.white;
            Simulator.bodies[focusedIndex].GetComponent<TrailRenderer>().startColor = Color.white;
        }
        focusedIndex = Simulator.bodies.IndexOf(go.GetComponent<GravitationalBody>());
        Simulator.bodies[focusedIndex].GetComponent<Renderer>().material.color = Color.red;
        Simulator.bodies[focusedIndex].GetComponent<TrailRenderer>().startColor = Color.red;
        massInput.gameObject.SetActive(true);
        massInput.text = Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().mass.ToString();
        xInput.gameObject.SetActive(true);
        xInput.text = Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.x.ToString();
        yInput.gameObject.SetActive(true);
        yInput.text = Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.z.ToString();
        xPosInput.gameObject.SetActive(true);
        xPosInput.text = Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().transform.position.x.ToString();
        yPosInput.gameObject.SetActive(true);
        yPosInput.text = Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().transform.position.z.ToString();

    }
    public static void ExitFocus()
    {
        if (focusedIndex != -1)
        {
            Simulator.bodies[focusedIndex].GetComponent<Renderer>().material.color = Color.white;
            Simulator.bodies[focusedIndex].GetComponent<TrailRenderer>().startColor = Color.white;
        }
        focusedIndex = -1;

    }
    public void SetMassOfFocused()
    {
        if(focusedIndex != -1 && Simulator.paused)
        {
            decimal temp = (decimal)Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().mass;
            if (!decimal.TryParse(massInput.text, out temp))
            {
            }
            else
            {
                   Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().mass = (float)temp;
                
            }

            
        }
    }
    public void SetXVelocityOfFocused()
    {
        if(focusedIndex != -1 && Simulator.paused)
        {
            decimal temp1 = (decimal)Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.x;
            if (!decimal.TryParse(xInput.text, out temp1))
            {
            }
            else
            {
                    Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.x = (float)temp1;
                
            }
        }
    }public void SetYVelocityOfFocused()
    {
        if(focusedIndex != -1 && Simulator.paused)
        {
            decimal temp2 = (decimal)Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.z;
            if (!decimal.TryParse(yInput.text, out temp2))
            {
            }
            else
            {
                
                    Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().velocity.z = (float)temp2;
                
            }
        }
    }public static void SetYPositionOfFocused()
    {
        if(focusedIndex != -1 && Simulator.paused)
        {
            decimal temp2 = (decimal)Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().transform.position.z;
            if (!decimal.TryParse(yPosInput.text, out temp2))
            {
            }
            else
            {
                TrailRenderer tR = Simulator.bodies[focusedIndex].gameObject.GetComponent<TrailRenderer>();
                tR.enabled = false;

                Simulator.bodies[focusedIndex].transform.position = new Vector3(Simulator.bodies[focusedIndex].transform.position.x, Simulator.bodies[focusedIndex].transform.position.y, (float)temp2);
                
                tR.enabled = true;
            }
        }
    }public static void SetXPositionOfFocused()
    {
        if(focusedIndex != -1 && Simulator.paused)
        {
            decimal temp2 = (decimal)Simulator.bodies[focusedIndex].GetComponent<GravitationalBody>().transform.position.x;
            if (!decimal.TryParse(xPosInput.text, out temp2))
            {
            }
            else
            {

                TrailRenderer tR = Simulator.bodies[focusedIndex].gameObject.GetComponent<TrailRenderer>();
                tR.enabled = false;
                Simulator.bodies[focusedIndex].transform.position = new Vector3((float)temp2, Simulator.bodies[focusedIndex].transform.position.y, Simulator.bodies[focusedIndex].transform.position.z);

                tR.enabled = true;
            }
        }
    }
    void FollowFocused()
    {
        if(focusedIndex != -1)
        {

            Vector3 offset = new Vector3(0, 15, 0);
            Vector3 desPos = Simulator.bodies[focusedIndex].gameObject.transform.position + offset;
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
