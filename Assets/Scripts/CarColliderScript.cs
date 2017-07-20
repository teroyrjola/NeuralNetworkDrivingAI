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
            Reset();
        }
    }

    void Reset()
    {
        transform.position = new Vector3(576, -399);
        transform.rotation = new Quaternion();
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}
