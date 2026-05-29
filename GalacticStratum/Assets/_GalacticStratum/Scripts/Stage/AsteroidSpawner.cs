using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private CinemachineCamera mainCamera;

    [Header("Spawn Settings")]
    [SerializeField] private SpriteRenderer map;
    [SerializeField] private int numberOfAsteroids;
    [SerializeField] private int maxAttemptsToSpawn;
    [SerializeField] private float minDistanceBetweenNonClusteredAsteroids;
    [SerializeField] private float secureDistanceForPlayerSpawn;

    [Header("Asteroid Cluster Settings")]
    [SerializeField] private float clusterRadius;
    [SerializeField] private int clusterAmount;
    private Vector2[] clusterCenters;

    private struct SpawnedAsteroid
    {
        public GameObject AsteroidObject;
        public Vector2 Position;
        public AsteroidData.AsteroidType size;
    }
    private List<SpawnedAsteroid> spawnedAsteroids = new();

    private readonly float[,] minimumDistanceMatrix = new float[4, 4]
    {
        //Pq,  Md,   Gr,   Mss
        { 3f, 5.6f, 9.6f, 17.6f }, //Pequeno
        { 5.6f, 10.3f, 14.6f, 24f }, //Médio
        { 9.6f, 14.6f, 20.3f, 30.1f }, //Grande
        { 17.6f, 24f, 30.1f, 40.5f } //Massivo
    };

    private void Start()
    {
        clusterCenters = new Vector2[clusterAmount];

        GenerateAsteroidField();
        TryToSpawnPlayer();
    }

    private void GenerateAsteroidField()
    {
        spawnedAsteroids.Clear();

        // Corrigido: Preencher clusterCenters corretamente
        for (int i = 0; i < clusterCenters.Length; i++)
        {
            float randomClusterX = Random.Range(map.bounds.min.x, map.bounds.max.x);
            float randomClusterY = Random.Range(map.bounds.min.y, map.bounds.max.y);
            clusterCenters[i] = new Vector2(randomClusterX, randomClusterY);
        }

        for (int i = 0; i < numberOfAsteroids; i++)
        {
            TryToSpawnAsteroid();
        }
    }

    private void TryToSpawnAsteroid()
    {
        AsteroidData data = new();

        for (int attempt = 0; attempt < maxAttemptsToSpawn; attempt++)
        {
            float randomX = Random.Range(map.bounds.min.x, map.bounds.max.x);
            float randomY = Random.Range(map.bounds.min.y, map.bounds.max.y);
            Vector2 spawnPosition = new(randomX, randomY);

            if (IsValidPosition(spawnPosition, data))
            {
                SpawnAsteroid(spawnPosition, data);
                return;
            }
        }
    }

    private bool IsValidPosition(Vector2 position, AsteroidData data)
    {
        if (!map.bounds.Contains(position))
            return false;

        bool isInCluster = false;
        foreach (var cluster in clusterCenters)
        {
            if (Vector2.Distance(position, cluster) <= clusterRadius)
                isInCluster = true;
        }

        foreach (SpawnedAsteroid spawned in spawnedAsteroids)
        {
            bool spawnedIsInCluster = false;
            foreach (var cluster in clusterCenters)
            {
                if (Vector2.Distance(spawned.Position, cluster) <= clusterRadius)
                    spawnedIsInCluster = true;
            }

            int matrixRow = (int)data.Size.Type;
            int matrixCol = (int)spawned.size;

            float requiredDistance = minimumDistanceMatrix[matrixRow, matrixCol];

            if (!isInCluster || !spawnedIsInCluster)
                requiredDistance += minDistanceBetweenNonClusteredAsteroids;

            if (Vector2.Distance(position, spawned.Position) < requiredDistance)
            {
                return false;
            }
        }

        return true;
    }

    private void SpawnAsteroid(Vector2 position, AsteroidData asteroidData)
    {
        GameObject asteroid = Instantiate(asteroidPrefab);
        SpriteRenderer spriteRenderer = asteroid.GetComponent<SpriteRenderer>();

        asteroid.transform.position = position;
        asteroid.GetComponent<Asteroid>().data = asteroidData;
        asteroid.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 361));
        spriteRenderer.flipX = Random.value >= 0.5f;
        spriteRenderer.flipY = Random.value >= 0.5f;
        asteroid.transform.parent = transform;
        asteroid.name = $"{asteroidData.Size.Type}Asteroid_{spawnedAsteroids.Count + 1}";

        spawnedAsteroids.Add(new SpawnedAsteroid { AsteroidObject = asteroid, Position = position, size = asteroidData.Size.Type });
    }

    private void TryToSpawnPlayer()
    {
        Vector2 spawnPosition;
        GameObject player;

        for (int attempt = 0; attempt < maxAttemptsToSpawn; attempt++)
        {
            float randomX = Random.Range(map.bounds.min.x, map.bounds.max.x);
            float randomY = Random.Range(map.bounds.min.y, map.bounds.max.y);
            spawnPosition = new(randomX, randomY);

            if (IsValidPositionForPlayerSpawn(spawnPosition))
            {
                player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
                player.name = "Player";
                player.GetComponent<Player>().mainCamera = mainCamera;
                mainCamera.Follow = player.transform;
                return;
            }
        }

        spawnPosition = map.bounds.center;

        foreach (var asteroid in spawnedAsteroids)
        {
            if (Vector2.Distance(asteroid.Position, spawnPosition) <= secureDistanceForPlayerSpawn)
            {
                Destroy(asteroid.AsteroidObject);
            }
        }

        player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        player.name = "Player";
        player.GetComponent<Player>().mainCamera = mainCamera;
        mainCamera.Follow = player.transform;
        Debug.LogWarning("It was not possible to find a safe spot to player spawn. Spawning in Center and destroying nearby Asteroids.");
    }

    private bool IsValidPositionForPlayerSpawn(Vector2 position)
    {
        if (!map.bounds.Contains(position))
            return false;

        foreach (SpawnedAsteroid spawned in spawnedAsteroids)
        {
            if (Vector2.Distance(position, spawned.Position) < secureDistanceForPlayerSpawn)
                return false;
        }

        return true;
    }
}
