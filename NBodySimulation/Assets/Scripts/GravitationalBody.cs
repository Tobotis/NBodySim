using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class GravitationalBody : MonoBehaviour
{
    
    public float mass;
    public Vector3 velocity;
    public Rigidbody rb;
    private float radius;
    public bool draggingObject = false;
    private Simulator sim;
    private void Awake()
    { 
        rb = GetComponent<Rigidbody>();
        sim = GameObject.Find("NBodySimulator").GetComponent<Simulator>();
        sim.bodies.Add(this);
        rb.mass = mass;
    }
    // Update the velocity based on all other bodies in space
    public void UpdateVelocity(List<GravitationalBody> bodies, float time)
    {
        foreach(GravitationalBody body in bodies)
        {
            if(body != this)
            {
                // F = G * (m1 * m2)/r^2 * r/abs(r)
                float rSquared = (body.rb.position - rb.position).sqrMagnitude;
                Vector3 dir = (body.rb.position - rb.position).normalized;
                Vector3 acceleration = dir * Simulator.G * body.mass  / rSquared; ;
                velocity += acceleration * time;
            }
        }
    }
    // Update the position of the body according to the updated velocity
    public void UpdatePosition(float time)
    {
         rb.position = rb.position + velocity * time;
    }

    private void Update()
    {
        if (Simulator.paused && Simulator.focusedBody != null)
        {
            if (Simulator.focusedBody == this)
            {
                if (Input.GetKeyDown(KeyCode.G))
                {
                    if (draggingObject)
                    {
                        draggingObject = false;
                    }
                    else
                    {
                        draggingObject = true;
                    }
                }
                if (draggingObject)
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
                draggingObject = false;
            }
        }
        else
        {
            draggingObject = false;
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