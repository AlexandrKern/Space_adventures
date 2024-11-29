using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidManager : MonoBehaviour
{
    #region Singleton
    public static AsteroidManager Instnace;

    private void Awake()
    {
        if (Instnace == null)
        {
            Instnace = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    #endregion

    public GameObject[] asteroidPrefubs;
    public float asteroidSpawnDistace;

    public float spawnTime;

    private float time;

    [HideInInspector] public float minX;
    [HideInInspector] public float maxX;
    [HideInInspector] public float minY;
    [HideInInspector] public float maxY;

    public List<GameObject> aliveAsteroids = new List<GameObject>();

    private void Start()
    {
        time = spawnTime;
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= spawnTime)
        {
            SpawnNewAsteroid();
            time = 0;
        }
    }

    private void SpawnNewAsteroid()
    {
        float newX = Random.Range(minX, maxX);
        float newY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(newX, newY,asteroidSpawnDistace);

        GameObject go = Instantiate(asteroidPrefubs[Random.Range(0,asteroidPrefubs.Length)],spawnPos,Quaternion.identity);
        aliveAsteroids.Add(go);
    }

    public void UpdateAsteroid(List<GameObject> targetAsteroids)
    {
        foreach (GameObject asteroid in aliveAsteroids)
        {
            if (targetAsteroids.Contains(asteroid))
            {
                asteroid.GetComponent<AsteroidController>().SetTargetMaterial();
            }
            else
            {
                asteroid.GetComponent<AsteroidController>().ResetMaterial();
            }
        }
    }
}
