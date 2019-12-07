using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Boundary
{
     public float xMin, xMax, zMin, zMax;
}

public class PlayerController : MonoBehaviour
{
     public float speed;
     public float tilt;
     public Boundary boundary;

     public GameObject shot;
     public Transform shotSpawn;
     public float fireRate;

     private float nextFire;
     private Rigidbody rb;
     private AudioSource audioSource;

     private float timer = 0.0f;
     private float pickupTime = 3.0f;

     private void Start()
     {
          rb = GetComponent<Rigidbody>();
          audioSource = GetComponent<AudioSource>();
     }

     void Update()
     {
       if (Input.GetButton("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
            audioSource.Play();

            if (Time.time > timer)
            {
              timer = Time.time + pickupTime;
              speed = 10;
              fireRate = 0.25f;
            }
        }
     }

     void FixedUpdate()
     {
          float moveHorizontal = Input.GetAxis("Horizontal");
          float moveVertical = Input.GetAxis("Vertical");

          Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
          rb.velocity = movement * speed;

          rb.position = new Vector3
          (
               Mathf.Clamp(rb.position.x, boundary.xMin, boundary.xMax),
               0.0f,
               Mathf.Clamp(rb.position.z, boundary.zMin, boundary.zMax)
          );

          rb.rotation = Quaternion.Euler(0.0f, 0.0f, rb.velocity.x * -tilt);
     }

     void OnTriggerEnter (Collider other)
     {
       if (other.gameObject.CompareTag("PickUp")) {

       //... then set the other object we just collided with to inactive.
       other.gameObject.SetActive(false);

       speed = 20;
       fireRate = 0.05f;
      }
     }
}
