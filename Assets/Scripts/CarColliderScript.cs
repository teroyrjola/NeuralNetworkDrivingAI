using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Assets.Scripts;
using UnityEngine;

public class CarColliderScript : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collidedWith)
    {
        if (collidedWith.gameObject.tag == "Land")
        {
            CarController controller = GetComponent<CarController>();
            //Reset();
            if (controller.Agent.IsAlive)
            {
                controller.Agent.SetGenotypeFitness();
                Die();
                SimulationManagerScript.Instance.CarCrash();
            }
        }

        if (collidedWith.gameObject.tag == "Checkpoint")
        {
            CarController controller = GetComponent<CarController>();
            CheckpointScript checkpoint = collidedWith.GetComponent<CheckpointScript>();

            if (controller.nextCheckpoint == collidedWith.gameObject.GetComponent<CheckpointScript>().Index)
            {
                controller.Agent.CurrentGenFitness++;

                if (checkpoint.TakeExtraRewardIfAnyLeft())
                {
                    controller.Agent.CurrentGenFitness++;
                }
                controller.nextCheckpoint++;
                controller.timeSinceLastCheckpoint = 0;
            }
        }
    }

    private void Die()
    {
        GetComponent<Rigidbody2D>().angularVelocity = 0;
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        GetComponent<CarController>().Agent.IsAlive = false;
        GetComponent<CarController>().nextCheckpoint = 0;
        GetComponent<CarController>().HideSensors();
    }
}
