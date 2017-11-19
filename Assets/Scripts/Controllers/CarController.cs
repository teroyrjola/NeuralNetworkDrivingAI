﻿using Assets.Scripts;
using Assets.Scripts.AI;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool userControl;
    public float acceleration;
    public float steering;
    public float max_turningSpeed;
    public int nextCheckpoint;
    public bool testiarvo;
    public int ID;
    public float maxTimeBetweenCheckpoints = 3;
    public float timeSinceLastCheckpoint = 0;

    public double[] controllerInput;

    internal Sensor[] sensors;
    private Rigidbody2D rb;

    public Agent Agent { get; set; }

    public double[] Movement { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sensors = GetComponentsInChildren<Sensor>();
        foreach (var sensor in sensors)
        {
            sensor.active = true;
        }
        Movement = new double[4];
        Agent = new Agent();
    }

    void Update()
    {
        if (Agent.IsAlive)
        {
            timeSinceLastCheckpoint += Time.deltaTime;

            if (timeSinceLastCheckpoint > maxTimeBetweenCheckpoints)
            {
                GetComponent<Rigidbody2D>().angularVelocity = 0;
                GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
                GetComponent<CarController>().Agent.SetGenotypeFitness();
                GetComponent<CarController>().Agent.IsAlive = false;
                GetComponent<CarController>().nextCheckpoint = 0;
                GetComponent<CarController>().HideSensors();
                SimulationManagerScript.Instance.CarCrash();
            }
        }
    }

    void FixedUpdate()
    {
        if (Agent.IsAlive)
        {
            float vertical;
            float horizontal;
            if (!userControl)
            {
                double[] sensorInput = new double[sensors.Length];
                for (int i = 0; i < sensors.Length; i++)
                {
                    sensorInput[i] = sensors[i].Output;
                }

                Agent.ANN.SetInputLayer(sensorInput);
                Movement = Agent.ANN.ProcessInputs();

                horizontal = (float)(Movement[0]);
                vertical = (float)(Movement[1]);

                if (horizontal > 1) horizontal = 1;
                else if (horizontal < -1) horizontal = -1;

                if (vertical > 1) vertical = 1;
                else if (vertical < -1) vertical = -1;
            }
            else
            {
                horizontal = -Input.GetAxis("Horizontal");
                vertical = Input.GetAxis("Vertical");
            }

            Vector2 speed = transform.up * (vertical * acceleration);
            rb.AddForce(speed);

            float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
            if (direction >= 0.0f)
            {
                rb.rotation += horizontal * steering * (rb.velocity.magnitude / 50.0f);
                //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
            }
            else
            {
                rb.rotation -= horizontal * steering * (rb.velocity.magnitude / 50.0f);
                //rb.AddTorque((-h * steering) * (rb.velocity.magnitude / 10.0f));
            }

            Vector2 forward = new Vector2(0.0f, 0.5f);
            float steeringRightAngle;
            if (rb.angularVelocity > 0)
            {
                steeringRightAngle = -90;
            }
            else
            {
                steeringRightAngle = 90;
            }

            Vector2 rightAngleFromForward = Quaternion.AngleAxis(steeringRightAngle, Vector3.forward) * forward;

            float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

            Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);

            rb.AddForce(rb.GetRelativeVector(relativeForce));
        }
    }

    public void ResetSensors()
    {
            sensors[0].sensor.gameObject.transform.localPosition = new Vector3(0, 5.75f);
            sensors[1].sensor.gameObject.transform.localPosition = new Vector3(2.25f, 5f);
            sensors[2].sensor.gameObject.transform.localPosition = new Vector3(3.5f, 3.5f);
            sensors[3].sensor.gameObject.transform.localPosition = new Vector3(-2.25f, 5f);
            sensors[4].sensor.gameObject.transform.localPosition = new Vector3(-3.5f, 3.5f);
    }

    public void HideSensors()
    {
        foreach (var sensor in sensors)
        {
            sensor.active = false;
            sensor.sensor.gameObject.SetActive(false);
        }
    }

    public void ShowSensors()
    {
        foreach (var sensor in sensors)
        {
            sensor.active = true;
            sensor.sensor.gameObject.SetActive(true);
        }
    }
}