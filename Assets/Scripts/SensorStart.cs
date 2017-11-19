using UnityEngine;

namespace Assets.Scripts
{
    public class SensorStart : MonoBehaviour
    {
        public float MaxDistance;
        public LayerMask layer;
        public Transform SensorEnd;
        public bool active;
        public float Output { get; set; }

        void FixedUpdate()
        {
            if (active) { 
            Vector2 direction = SensorEnd.transform.position - this.transform.position;
            direction.Normalize();
            
            RaycastHit2D sensorHit = Physics2D.Raycast(this.transform.position, direction, MaxDistance, layer);

            if (sensorHit.collider == null)
                sensorHit.distance = MaxDistance;

            this.Output = sensorHit.distance;

            SensorEnd.transform.position = (Vector2)this.transform.position + direction * sensorHit.distance;
            }
        }


    }
}