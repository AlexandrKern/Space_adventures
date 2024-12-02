using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public float moveSpeed;

    private Rigidbody rb;

    private Vector3 randomRotation;

    private float removePositionZ;

    public Material targetMaterial;

    private Material baseMaterial;
    private Renderer[] renderers;

    public ParticleSystem explosion;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        randomRotation = new Vector3(Random.Range(0f,100f), Random.Range(0f, 100f), Random.Range(0f, 100f));

        removePositionZ = Camera.main.transform.position.z;

        renderers = GetComponents<Renderer>();
        baseMaterial = renderers[0].material;
    }

    private void Update()
    {
        if(transform.position.z < removePositionZ)
        {
            AsteroidManager.Instnace.aliveAsteroids.Remove(gameObject);
            Destroy(gameObject);
        }

        Vector3 movePosition = new Vector3(0,0, -moveSpeed * Time.deltaTime);
        rb.velocity = movePosition;

        transform.Rotate(randomRotation * Time.deltaTime);
    }

    public void ResetMaterial()
    {
        if(renderers == null)
            return;

        foreach(Renderer renderer in renderers)
        {
            renderer.material = baseMaterial;
        }

    }

    public void SetTargetMaterial()
    {
        if (renderers == null)
            return;

        foreach (Renderer renderer in renderers)
        {
            renderer.material = targetMaterial;
        }
    }

    public void DestroyAsteroid()
    {
        Data.Instance.AddGold();
        AsteroidManager.Instnace.OnAsteroidKill(gameObject);
        Instantiate(explosion,transform.position,Quaternion.identity);
        AudioManager.Instance.PlaySFX("Explosion");
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().OnAsteroidImpact();
            DestroyAsteroid();
        }
    }

    public void IncreaseSpeed(float speedIncrease)
    {
        moveSpeed += speedIncrease;
    }
}
