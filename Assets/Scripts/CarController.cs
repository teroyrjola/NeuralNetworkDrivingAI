using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.AI;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public bool userControl;
    public float acceleration;
    public float steering;

    public double[] controllerInput;

    private Sensor[] sensors;
    private Rigidbody2D rb;

    public Agent agent { get; set; }

    public double[] Movement { get; set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sensors = GetComponentsInChildren<Sensor>();
        agent = new Agent();
        Movement = new double[4];
    }

    void FixedUpdate()
    {
        float v = 1;
        float h = 1;
        if (!userControl)
        {
            double[] sensorInput = new double[sensors.Length];
            for (int i = 0; i < sensors.Length; i++)
            {
                sensorInput[i] = sensors[i].Output;
            }

            agent.ANN.SetInputLayer(sensorInput);
            Movement = agent.ANN.ProcessInputs();

            h = (float)(Movement[0] - Movement[1]);
            v = (float)(Movement[2] - Movement[3]);
        }
        else
        {
            h = -Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
        }

        Vector2 speed = transform.up * (v * acceleration);
        rb.AddForce(speed);

        float direction = Vector2.Dot(rb.velocity, rb.GetRelativeVector(Vector2.up));
        if (direction >= 0.0f)
        {
            rb.rotation += h * steering * (rb.velocity.magnitude / 50.0f);
            //rb.AddTorque((h * steering) * (rb.velocity.magnitude / 10.0f));
        }
        else
        {
            rb.rotation -= h * steering * (rb.velocity.magnitude / 50.0f);
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
        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(rightAngleFromForward), Color.green);

        float driftForce = Vector2.Dot(rb.velocity, rb.GetRelativeVector(rightAngleFromForward.normalized));

        Vector2 relativeForce = (rightAngleFromForward.normalized * -1.0f) * (driftForce * 10.0f);


        Debug.DrawLine((Vector3)rb.position, (Vector3)rb.GetRelativePoint(relativeForce), Color.red);

        rb.AddForce(rb.GetRelativeVector(relativeForce));
    }
}