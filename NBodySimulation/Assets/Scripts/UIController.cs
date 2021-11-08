using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static event Action OnReset;

    private Simulator simulator;

    private static Button addButton;
    private static Button startStopButton;
    private static Button resetButton;
    private static Button clearTrailsButton;
    private static Button loadEulerButton;
    private static Button deleteButton;
    private static Button exitFocusButton;
    private static Button nextButton;
    private static Button previousButton;
    private static Text pausedText;
    private static InputField trailLengthInput;

    private static InputField massInput;
    private static InputField xVInput;
    private static InputField yVInput;
    private static InputField xPInput;
    private static InputField yPInput;


    void Awake()
    {

        simulator = gameObject.GetComponent<Simulator>();

        addButton = GameObject.Find("AddButton").GetComponent<Button>();
        resetButton = GameObject.Find("ResetButton").GetComponent<Button>();
        startStopButton = GameObject.Find("StartStopButton").GetComponent<Button>();
        clearTrailsButton = GameObject.Find("ClearTrailsButton").GetComponent<Button>();
        loadEulerButton = GameObject.Find("LoadEulerButton").GetComponent<Button>();
        deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
        exitFocusButton = GameObject.Find("ExitFocusButton").GetComponent<Button>();
        nextButton = GameObject.Find("NextButton").GetComponent<Button>();
        previousButton = GameObject.Find("PreviousButton").GetComponent<Button>();
        trailLengthInput = GameObject.Find("TrailLengthInputField").GetComponent<InputField>();
        massInput = GameObject.Find("MassInputField").GetComponent<InputField>();
        xVInput = GameObject.Find("VelocityXInputField").GetComponent<InputField>();
        yVInput = GameObject.Find("VelocityYInputField").GetComponent<InputField>();
        xPInput = GameObject.Find("PosXInputField").GetComponent<InputField>();
        yPInput = GameObject.Find("PosYInputField").GetComponent<InputField>();

        addButton.onClick.AddListener(simulator.AddPlanet);
        resetButton.onClick.AddListener(ResetGame);
        startStopButton.onClick.AddListener(StartStopGame);
        clearTrailsButton.onClick.AddListener(simulator.ClearTrails);
        loadEulerButton.onClick.AddListener(LoadEuler);
        deleteButton.onClick.AddListener(simulator.DeleteObj);
        deleteButton.gameObject.SetActive(false);
        exitFocusButton.onClick.AddListener(simulator.ExitFocus);
        exitFocusButton.gameObject.SetActive(false);
        nextButton.onClick.AddListener(simulator.NextFocused);
        previousButton.onClick.AddListener(simulator.PrevFocused);
        previousButton.onClick.AddListener(simulator.PrevFocused);
        trailLengthInput.onValueChanged.AddListener(simulator.SetTrailLength);
        trailLengthInput.text = 100.ToString();

        massInput.onValueChanged.AddListener(simulator.SetMassOfFocused);
        massInput.gameObject.SetActive(false);

        xVInput.onValueChanged.AddListener(simulator.SetXVelocityOfFocused);
        xVInput.gameObject.SetActive(false);

        yVInput.onValueChanged.AddListener(simulator.SetYVelocityOfFocused);
        yVInput.gameObject.SetActive(false);

        xPInput.onValueChanged.AddListener(simulator.SetXPositionOfFocused);
        xPInput.gameObject.SetActive(false);

        yPInput.onValueChanged.AddListener(simulator.SetYPositionOfFocused);
        yPInput.gameObject.SetActive(false);

        pausedText = startStopButton.GetComponentInChildren<Text>();

    }

    public static void OnFocused(GravitationalBody obj)
    {
        if(obj != null)
        {
            deleteButton.gameObject.SetActive(true);
            exitFocusButton.gameObject.SetActive(true);
            massInput.gameObject.SetActive(true);
            massInput.text = obj.mass.ToString();
            xVInput.gameObject.SetActive(true);
            xVInput.text = obj.velocity.x.ToString();
            yVInput.gameObject.SetActive(true);
            yVInput.text = obj.velocity.z.ToString();
            xPInput.gameObject.SetActive(true);
            xPInput.text = obj.rb.position.x.ToString();
            yPInput.gameObject.SetActive(true);
            yPInput.text = obj.rb.position.z.ToString();

        }
        else
        {
            deleteButton.gameObject.SetActive(false);
            exitFocusButton.gameObject.SetActive(false);
            massInput.gameObject.SetActive(false);
            xVInput.gameObject.SetActive(false);
            yVInput.gameObject.SetActive(false);
            xPInput.gameObject.SetActive(false);
            yPInput.gameObject.SetActive(false);
            
        }
    }

    private void Update()
    {
        if (Simulator.focusedBody != null && !Simulator.paused)
        {
            xVInput.text = Simulator.focusedBody.GetComponent<GravitationalBody>().velocity.x.ToString();
            yVInput.text = Simulator.focusedBody.GetComponent<GravitationalBody>().velocity.z.ToString();
            xPInput.text = Simulator.focusedBody.transform.position.x.ToString();
            yPInput.text = Simulator.focusedBody.transform.position.z.ToString();
        }
    }
    private void StartStopGame()
    {
        pausedText.text = Simulator.paused ? "Stop" : "Start";
        Simulator.paused = !Simulator.paused;
        addButton.gameObject.SetActive(Simulator.paused);
        simulator.OnPaused();
    }

    private void ResetGame()
    {
        trailLengthInput.text = 100.ToString();
        pausedText.text = "Start";
        Simulator.paused = true;
        addButton.gameObject.SetActive(true);
        simulator.OnPaused();
        simulator.OnReset();
    }

    private void LoadEuler()
    {
        ResetGame();
        simulator.LoadConfig(new List<BodyData>(){new BodyData(m: 1, new Vector3(0.347111f,0, 0.532728f), p: new Vector3(-1,0,0)),
            new BodyData(m: 1, new Vector3(0.347111f, 0, 0.532728f), p: new Vector3(1, 0, 0)),
            new BodyData(m: 1, new Vector3(-0.694222f,0,-1.065456f), p: new Vector3(0,0,0)) });
    }

}
