using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prediction : MonoBehaviour
{
    void Update()
    {
        if (Simulator.paused)
        {
            foreach(GravitationalBody body in Simulator.bodies)
            {
                
            }
        }
    }
}
