using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform car;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
	    var carToFollow = SimulationManagerScript.Instance.cars[0];
	    foreach (var car in SimulationManagerScript.Instance.cars)
	    {
	        if (car.controller.nextCheckpoint > carToFollow.nextCheckpoint) carToFollow = car;
	    }

        transform.position = new Vector3(carToFollow.controller.GetComponent<Transform>().position.x,
		    carToFollow.controller.GetComponent<Transform>().position.y, -15);
	}
}
