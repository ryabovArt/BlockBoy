using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBlocksSpawner : MonoBehaviour
{
    private Vector3 spawnPos;
    public GameObject[] tetrisBlock;

    void Start()
    {
        spawnPos.y = 20f;
        Repeat();
    }

    private void Repeat()
    {
        StartCoroutine(SpawnerCorutine());
    }

    /// <summary>
    /// Рандомный спавн фигурок тетрис на заднем плане меню
    /// </summary>
    /// <returns> каждые 0.2с. </returns>
    IEnumerator SpawnerCorutine()
    {
        yield return new WaitForSeconds(0.2f);
        int index = Random.Range(0, tetrisBlock.Length);
        float xPos = Random.Range(-70f, 70f);
        float zPos = Random.Range(0, 130f);
        spawnPos = new Vector3(xPos, spawnPos.y, zPos);
        Instantiate(tetrisBlock[index], spawnPos, Quaternion.identity);
        Repeat();
    }
}
