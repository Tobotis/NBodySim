using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class GravitationalBody : MonoBehaviour
{
    public float radius;
    public float mass;
    public Vector3 startVelocity;
    public Vector3 velocity;
    public Rigidbody rb;
    public bool moving = false;
    private void Awake()
    {
        Simulator.bodies.Add(this);
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
        velocity = startVelocity;
    }
    public void UpdateVelocity(List<GravitationalBody> bodies, float time)
    {
        foreach(GravitationalBody body in bodies)
        {
            if(body != this)
            {
                float dist = (body.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (body.rb.position - rb.position).normalized;
                Vector3 force = forceDir * Simulator.G * body.mass * mass / dist;
                Vector3 acceleration =  force/ mass;
                velocity += acceleration * time;
            }
        }
    }
    public void UpdateVelocity(float time)
    {
        velocity += velocity * time;
    }
    public void UpdatePosition(float time)
    {
        

            rb.position = rb.position + velocity * time;
    }
    private void Update()
    {
        if (Simulator.paused && CameraController.focusedIndex != -1)
        {
            if (Simulator.bodies[CameraController.focusedIndex] == this)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (moving)
                    {

                        CameraController.xPosInput.text = transform.position.x.ToString();
                        CameraController.yPosInput.text = transform.position.z.ToString();
                        moving = false;
                    }
                    else
                    {
                        moving = true;
                    }
                }
                if (moving)
                {
                    float temp = Time.timeScale;
                    TrailRenderer tR = gameObject.GetComponent<TrailRenderer>();
                    tR.enabled = false;
                    Vector3 mousePos = Input.mousePosition;
                    Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                    worldPos.y = 0;
                    transform.position = worldPos;
                    tR.enabled = true;

                    Time.timeScale = temp;
                }
            }
            else
            {
                moving = false;
            }
        }
        else
        {
            moving = false;
        }
    }

}
public class BodyData
{
    public float mass;
    public Vector3 startVelocity;
    public Vector3 pos;

    public BodyData(float m, Vector3 startV, Vector3 p)
    {
        mass = m;
        startVelocity = startV;
        pos = p;
    }

}