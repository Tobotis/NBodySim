using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using System.Reflection;
public class Simulator : MonoBehaviour
{
    // Gravitational constant
    public const float G = 1f;
    // List of all bodies in system
    public List<GravitationalBody> bodies = new List<GravitationalBody>();
    // Paused
    public static bool paused = true;
    // New Run (After Reset or start)
    private bool startingState = true;
    // Prefab of a gravitational body
    public GameObject bodyPrefab;
    // Configuration for reloading a state
    private List<BodyData> configuration = new List<BodyData>();
    // Index of the focused body
    public static GravitationalBody focusedBody = null;
    private void Awake()
    {
        SaveConfig();
    }

    public void OnReset()
    {
        startingState = true;
        Time.timeScale = 1;
        ExitFocus();
        Camera.main.transform.position = CameraController.resetCamera;
        LoadConfig(configuration);
    }

    public void OnPaused()
    {
        if (startingState)
        {

            SaveConfig();
            startingState = false;

        }
    }

    void FixedUpdate()
    {
        if (!paused)
        {
            foreach (GravitationalBody body in bodies)
            {
                body.UpdateVelocity(bodies, Time.fixedDeltaTime);
            }
            foreach (GravitationalBody body in bodies)
            {
                body.UpdatePosition(Time.fixedDeltaTime);
            }
        }
        
    }
    public void LoadConfig(List<BodyData> conf)
    {
        paused = true;
        ExitFocus();
        foreach (GravitationalBody body in bodies)
        {
            Destroy(body.gameObject);
        }
        bodies.Clear();
        foreach (BodyData body in conf)
        {
            GameObject clone = Instantiate(bodyPrefab, body.pos, new Quaternion(0,0,0,0));
            clone.GetComponent<GravitationalBody>().mass = body.mass;
            clone.GetComponent<GravitationalBody>().velocity = body.startVelocity;
        }
    }

    public void SaveConfig()
    {
        configuration.Clear();
        bodies.ForEach((item) =>
        {
            configuration.Add(new BodyData(item.mass, item.velocity, item.rb.position));
        });
    }
    public void NextFocused()
    {
        if(bodies.IndexOf(focusedBody) == bodies.Count - 1) {
            if(bodies.Count > 0)
            {
                ChangeFocus(bodies[0].gameObject);
            }
        
        }
        else
        {
            ChangeFocus(bodies[bodies.IndexOf(focusedBody) + 1].gameObject);
        }
    }
    public void PrevFocused()
    {
        if (bodies.IndexOf(focusedBody) <= 0)
        {
            if (bodies.Count > 0)
            {
                ChangeFocus(bodies[bodies.Count - 1].gameObject);
            }

        }
        else
        {
            ChangeFocus(bodies[bodies.IndexOf(focusedBody) - 1].gameObject);
        }
    }
    public void DeleteObj()
    {
        GravitationalBody remove = focusedBody;
        ExitFocus();
        bodies.Remove(remove);
        Destroy(remove.gameObject);
    }
    public void ChangeSpeed(float timeScale)
    {
        if(timeScale >= 100)
        {
            foreach (GravitationalBody body in bodies)
            {
                    body.gameObject.GetComponent<TrailRenderer>().enabled = false;
            }
        }
        else
        {
            foreach (GravitationalBody body in bodies)
            {
                body.gameObject.GetComponent<TrailRenderer>().enabled = true;

            }
        }
        Time.timeScale= (float)timeScale;
    }

    public void ClearTrails()
    {
        foreach (GravitationalBody body in bodies)
        {
            body.gameObject.GetComponent<TrailRenderer>().Clear();
        }
    }

    public void SetTrailLength(String val)
    {
        int temp;
        if (int.TryParse(val, out temp))
        {
            foreach (GravitationalBody body in bodies)
            {
                body.gameObject.GetComponent<TrailRenderer>().time = (float)temp;
            }
        }
    }
    public void AddPlanet()
    {
        GameObject go = Instantiate(bodyPrefab);
        ChangeFocus(go);
    }

    public static void ChangeFocus(GameObject go)
    {
        
        if (focusedBody != null)
        {
            focusedBody.GetComponent<Renderer>().material.color = Color.white;
            focusedBody.GetComponent<TrailRenderer>().startColor = Color.white;
        }
        focusedBody = null;
        UIController.OnFocused(go.GetComponent<GravitationalBody>());
        focusedBody = go.GetComponent<GravitationalBody>();
        focusedBody.GetComponent<Renderer>().material.color = Color.red;
        focusedBody.GetComponent<TrailRenderer>().startColor = Color.red;
        
    }
    public void SetMassOfFocused(String val)
    {
        if (focusedBody !=null && paused)
        {
            decimal temp = (decimal)focusedBody.GetComponent<GravitationalBody>().mass;
            if (!decimal.TryParse(val, out temp))
            {
            }
            else
            {
                focusedBody.GetComponent<GravitationalBody>().mass = (float)temp;
            }
        }
    }
    public void SetXVelocityOfFocused(String val)
    {
        if (focusedBody != null && paused)
        {
            decimal temp1 = (decimal)focusedBody.GetComponent<GravitationalBody>().velocity.x;
            if (!decimal.TryParse(val, out temp1))
            {
            }
            else
            {
                focusedBody.GetComponent<GravitationalBody>().velocity.x = (float)temp1;

            }
        }
    }
    public void SetYVelocityOfFocused(String val)
    {
        if (focusedBody != null && paused)
        {
            decimal temp2 = (decimal)focusedBody.GetComponent<GravitationalBody>().velocity.z;
            if (!decimal.TryParse(val, out temp2))
            {
            }
            else
            {

                focusedBody.GetComponent<GravitationalBody>().velocity.z = (float)temp2;

            }
        }
    }
    public void SetYPositionOfFocused(String val)
    {
        if (focusedBody != null && paused)
        {

            decimal temp2 = (decimal)focusedBody.GetComponent<GravitationalBody>().transform.position.z;
            if (!decimal.TryParse(val, out temp2))
            {
            }
            else
            {
                TrailRenderer tR = focusedBody.gameObject.GetComponent<TrailRenderer>();
                tR.enabled = false;

                focusedBody.transform.position = new Vector3(focusedBody.transform.position.x, focusedBody.transform.position.y, (float)temp2);

                tR.enabled = true;
            }
        }
    }
    public void SetXPositionOfFocused(String val)
    {
        
        if (focusedBody != null && paused )
        {
            decimal temp2 = (decimal)focusedBody.GetComponent<GravitationalBody>().transform.position.x;
            if (!decimal.TryParse(val, out temp2))
            {
            }
            else
            {

                TrailRenderer tR = focusedBody.gameObject.GetComponent<TrailRenderer>();
                tR.enabled = false;
                focusedBody.transform.position = new Vector3((float)temp2, focusedBody.transform.position.y, focusedBody.transform.position.z);

                tR.enabled = true;
            }
        }
    }
    public void ExitFocus()
    {
        UIController.OnFocused(null);
        if (focusedBody != null)
        {
            focusedBody.GetComponent<Renderer>().material.color = Color.white;
            focusedBody.GetComponent<TrailRenderer>().startColor = Color.white;
        }
        focusedBody =null;

    }
}
