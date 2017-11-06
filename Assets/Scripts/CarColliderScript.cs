using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CarColliderScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collidedWith)
    {
        if (collidedWith.gameObject.tag == "Land")
        {
            //Reset();
            if (GetComponent<CarController>().Agent.IsAlive) SimulationManagerScript.Instance.CarCrash();
            Die();
            GetComponent<CarController>().Agent.SetGenotypeFitness();
        }

        if (collidedWith.gameObject.tag == "Checkpoint")
        {
            if (GetComponent<CarController>().nextCheckpoint ==
                collidedWith.gameObject.GetComponent<CheckpointScript>().Index)
            {
                GetComponent<CarController>().Agent.CurrentGenFitness++;
                GetComponent<CarController>().nextCheckpoint++;
                GetComponent<CarController>().timeSinceLastCheckpoint = 0;
            }
        }
    }

    private void Die()
    {
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<CarController>().Agent.IsAlive = false;
        GetComponent<CarController>().nextCheckpoint = 0;
    }
}
