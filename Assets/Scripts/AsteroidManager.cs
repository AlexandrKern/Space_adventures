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

    public int asteroidsToFinish = 3;
    public float speedIncrease = 5f;
    private int asteroidCaunter = 0;
    public InGameManager inGameManager;

    [HideInInspector] public float minX;
    [HideInInspector] public float maxX;
    [HideInInspector] public float minY;
    [HideInInspector] public float maxY;

    [HideInInspector] public List<GameObject> aliveAsteroids = new List<GameObject>();

    private void Start()
    {
        time = spawnTime;
        inGameManager.ChangeAsteroidKillCount(asteroidsToFinish);
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

    public void OnAsteroidKill(GameObject asteroid)
    {
        asteroidsToFinish--;
        aliveAsteroids.Remove(asteroid );
        if (asteroidsToFinish <= 0)
        {
            int thidLevelIndex = GameManager.Instnace.curentLevelIndex;
            int lastLevelComplited = Data.Instance.GetLevelCompleted();

            if (thidLevelIndex > lastLevelComplited)
            {
                Data.Instance.CompletedNextLevel();
            }
            inGameManager.OnLevelCompleteMenu();
        }
        else
        {
            inGameManager.ChangeAsteroidKillCount(asteroidsToFinish);
        }
    }

    private void SpawnNewAsteroid()
    {
        float newX = Random.Range(minX, maxX);
        float newY = Random.Range(minY, maxY);

        Vector3 spawnPos = new Vector3(newX, newY,asteroidSpawnDistace);

        GameObject go = Instantiate(asteroidPrefubs[Random.Range(0,asteroidPrefubs.Length)],spawnPos,Quaternion.identity);
        go.GetComponent<AsteroidController>().IncreaseSpeed(asteroidCaunter * speedIncrease);
        asteroidCaunter++;

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
