using UnityEngine;

namespace Assets.Scripts
{
    public class Sensor : MonoBehaviour
    {
        public float max_distance;
        public LayerMask layer;
        public Transform sensor;
        public bool active;
        public float Output { get; set; }

        void FixedUpdate()
        {
            if (active) { 
            Vector2 direction = sensor.transform.position - this.transform.position;
            direction.Normalize();
            
            RaycastHit2D sensorHit = Physics2D.Raycast(this.transform.position, direction, max_distance, layer);

            if (sensorHit.collider == null)
                sensorHit.distance = max_distance;

            this.Output = sensorHit.distance;

            sensor.transform.position = (Vector2)this.transform.position + direction * sensorHit.distance;
            }
        }


    }
}