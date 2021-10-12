using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Simulator : MonoBehaviour
{

    public static float G = 1f;
    public static List<GravitationalBody> bodies = new List<GravitationalBody>();
    public static bool paused = true;
    private bool start = true;
    public GameObject planetPrefab;
    public Button addButton;
    public Text pausedText;
    public InputField trailLengthInput;
    private List<BodyData> configuration = new List<BodyData>();
    private void Awake()
    {
        trailLengthInput.text = 1000.ToString();
        paused = true;
        bodies.ForEach((item) =>
        {
            configuration.Add(new BodyData(item.mass, item.velocity, item.rb.position));
        });
    }
    private void Update()
    {

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
    public void PauseResumeGame()
    {
        paused = !paused;
        if (start)
        {
            configuration = new List<BodyData>();
            bodies.ForEach((item) =>
            {
                configuration.Add(new BodyData(item.mass, item.velocity, item.rb.position));
            });
        }
        start = false;
        if (paused)
        {
            pausedText.text = "Starten";
            addButton.gameObject.SetActive(true);
        }
        else
        {
            pausedText.text = "Stoppen";
            addButton.gameObject.SetActive(false);
        }


    }
    public void ResetSimulation()
    {

        Time.timeScale = 1;
        CameraController.ExitFocus();
        Camera.main.transform.position = CameraController.ResetCamera;
        pausedText.text = "Starten";
        addButton.gameObject.SetActive(true);
        LoadConfig(configuration);

    }
    public void LoadEuler()
    {
        ResetSimulation();
        LoadConfig(new List<BodyData>(){new BodyData(m: 1, new Vector3(0.347111f,0, 0.532728f), p: new Vector3(-1,0,0)),
            new BodyData(m: 1, new Vector3(0.347111f, 0, 0.532728f), p: new Vector3(1, 0, 0)),
            new BodyData(m: 1, new Vector3(-0.694222f,0,-1.065456f), p: new Vector3(0,0,0)) });
    }
    public void LoadConfig(List<BodyData> conf)
    {

        start = true;
        paused = true;
        CameraController.ExitFocus();
        foreach (GravitationalBody body in bodies)
        {
            Destroy(body.gameObject);
        }
        bodies.Clear();
        foreach (BodyData body in conf)
        {
            GameObject clone = Instantiate(planetPrefab, body.pos, new Quaternion(0,0,0,0));
            clone.GetComponent<GravitationalBody>().mass = body.mass;
            clone.GetComponent<GravitationalBody>().velocity = body.startVelocity;
        }
    }
    public void NextFocused()
    {
        if(CameraController.focusedIndex == bodies.Count - 1) {
            if(bodies.Count > 0)
            {
                CameraController.ChangeFocus(bodies[0].gameObject);
            }
        
        }
        else
        {
            CameraController.ChangeFocus(bodies[CameraController.focusedIndex + 1].gameObject);
        }
    }
    public void BackFocused()
    {
        if (CameraController.focusedIndex <= 0)
        {
            if (bodies.Count > 0)
            {
                CameraController.ChangeFocus(bodies[bodies.Count - 1].gameObject);
            }

        }
        else
        {
            CameraController.ChangeFocus(bodies[CameraController.focusedIndex - 1].gameObject);
        }
    }
    public void DeleteGravObj()
    {
        GravitationalBody remove = bodies[CameraController.focusedIndex];
        CameraController.ExitFocus();
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

    public void AddPlanet()
    {
        GameObject go = Instantiate(planetPrefab);
        CameraController.ChangeFocus(go);
    }
    public void ClearTrails()
    {
        foreach (GravitationalBody body in bodies)
        {
            body.gameObject.GetComponent<TrailRenderer>().Clear();
        }
    }
    public void SetTrailLength()
    {
        foreach (GravitationalBody body in bodies)
        {
            int temp = 0;
            if(int.TryParse(trailLengthInput.text,out temp)) {
                body.gameObject.GetComponent<TrailRenderer>().time = (float)temp;
            }
        }
    }
}
