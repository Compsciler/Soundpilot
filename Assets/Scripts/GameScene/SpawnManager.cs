using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] float firstSpawnTime;
    private float nextSpawnTime;
    [SerializeField] float spawnRate;

    [Serializable]
    private struct SpawnGameObject
    {
        [SerializeField] internal GameObject spawnPrefab;
        [SerializeField] internal float spawnPosMinY;
        [SerializeField] internal float spawnPosMaxY;
        [SerializeField] internal float relativeSpawnProbability;
    }
    [SerializeField] SpawnGameObject[] spawnGameObjects;

    [SerializeField] Transform obstacleParentTF;

    private float[] relativeSpawnProbabilities;
    private float relativeSpawnProbabilitySum = 0;

    [SerializeField] PlayerController playerController;
    private Transform playerTF;
    private float playerStartPosX;

    void Start()
    {
        playerTF = playerController.transform;
        playerStartPosX = playerTF.position.x;

        nextSpawnTime = firstSpawnTime;

        relativeSpawnProbabilities = new float[spawnGameObjects.Length];
        for (int i = 0; i < spawnGameObjects.Length; i++)
        {
            float relativeSpawnProbability = spawnGameObjects[i].relativeSpawnProbability;
            relativeSpawnProbabilities[i] = relativeSpawnProbability;
            relativeSpawnProbabilitySum += relativeSpawnProbability;
        }
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad >= nextSpawnTime)
        {
            SpawnObstacle();
            nextSpawnTime = Time.timeSinceLevelLoad + spawnRate;
        }
    }

    public void SpawnObstacle()
    {
        int obstacleIndex = Sample(relativeSpawnProbabilities);
        SpawnGameObject spawnGameObjectObstacle = spawnGameObjects[obstacleIndex];
        GameObject obstaclePrefab = spawnGameObjectObstacle.spawnPrefab;
        float obstaclePosX = obstaclePrefab.transform.position.x + (playerTF.position.x - playerStartPosX);  // TODO: Make viewport point instead?
        float obstaclePosY = UnityEngine.Random.Range(spawnGameObjectObstacle.spawnPosMinY, spawnGameObjectObstacle.spawnPosMaxY);
        Vector3 obstaclePos = new Vector3(obstaclePosX, obstaclePosY, obstaclePrefab.transform.position.z);
        Instantiate(obstaclePrefab, obstaclePos, obstaclePrefab.transform.rotation, obstacleParentTF);
    }

    // Choose an integer at random, according to the supplied distribution
    public static int Sample(float[] distro)
    {
        float total = distro.Sum();
        return Sample(distro, total);
    }
    public static int Sample(float[] distro, float total)
    {
        float randVal = total * UnityEngine.Random.Range(0f, 1f);
        for (int i = 0; i < distro.Length; i++)
        {
            if (randVal < distro[i])
            {
                return i;
            }
            randVal -= distro[i];
        }
        return distro.Length - 1;
    }
}
