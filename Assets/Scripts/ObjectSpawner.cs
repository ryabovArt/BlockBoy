using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] obstacles;
    [SerializeField] private GameObject[] coins;

    public static int obstacleSpawnChance = 3;
    [SerializeField] private int coinSpawnChance;

    void Start()
    {
        SpawnObject();
    }

    /// <summary>
    /// Спавн объектов
    /// </summary>
    private void SpawnObject()
    {
        for (int i = 0; i < obstacles.Length; i++)
        {
            obstacles[i].SetActive(Random.Range(0, 10) <= obstacleSpawnChance);
        }
        for (int j = 0; j < coins.Length; j++)
        {
            coins[j].SetActive(Random.Range(0, 10) <= coinSpawnChance);
        }
    }
}
