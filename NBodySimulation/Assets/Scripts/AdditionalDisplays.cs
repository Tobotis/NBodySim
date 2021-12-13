using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditionalDisplays : MonoBehaviour
{
    private Simulator simulator;
    private LineRenderer lr;
    void Awake()
    {

        simulator = GameObject.Find("NBodySimulator").GetComponent<Simulator>();
        lr = GetComponent<LineRenderer>();
    }

    void FixedUpdate()
    {
        lr.positionCount = simulator.bodies.Count;
        for (int i = 0; i < simulator.bodies.Count; i++)
        {
            lr.SetPosition(i, simulator.bodies[i].transform.position);
        }
    }
}
