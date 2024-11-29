using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileController : MonoBehaviour
{
    public float moveSpeed = 400f;
    private Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.z > AsteroidManager.Instnace.asteroidSpawnDistace)
        {
            Destroy(gameObject);
        }
        rb.velocity = new Vector3(0,0,moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Asteroid"))
        {
            other.GetComponent<AsteroidController>().DestroyAsteroid();
            Destroy(gameObject);
        }
      
    }
}
