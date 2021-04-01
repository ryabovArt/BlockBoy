using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public GameObject[] obstaclePlatforms;
    public GameObject[] upperPlatforms;
    public GameObject[] blocks;
    public GameObject[] environment;
    public Transform platformContainer;

    private Transform lastPlatform = null;

    private int index;

    private bool isEnvironment = false;
    internal bool isObstacle = true;
    internal bool isUpperPlatform = false;

    void Start()
    {
        Init();
    }

    /// <summary>
    /// Создание платформ на старте игры
    /// </summary>
    public void Init()
    {
        for (int j = 0; j < 1; j++)
        {
            CreateFirstPlatform();
        }
        for (int i = 0; i < 14; i++)
        {
            CreatePlatforms();
        }
    }

    /// <summary>
    /// Генерация первой платформы
    /// </summary>
    private void CreateFirstPlatform()
    {
        InstantiatePlatform(blocks[0]);
    }

    /// <summary>
    /// Выбор платформы
    /// </summary>
    public void CreatePlatforms()
    {
        GameObject temp;

        if (!isEnvironment)
        {
            if (!isUpperPlatform && isObstacle)
            {
                int indexObstaclePlatforms = Random.Range(0, obstaclePlatforms.Length);
                temp = obstaclePlatforms[indexObstaclePlatforms];
            }
            else
            {
                temp = upperPlatforms[index];
                index++;

                if (index >= 5)
                {
                    isObstacle = true;
                    index = 0;
                    isUpperPlatform = false;
                }
            }

            isEnvironment = true;
        }
        else
        {
            temp = environment[0];
            isEnvironment = false;
        }

        InstantiatePlatform(temp);
    }

    /// <summary>
    /// Поиск координат выбранной платформы
    /// </summary>
    /// <param name="platform"> платформа </param>
    private void InstantiatePlatform(GameObject platform)
    {
        Vector3 pos = (lastPlatform == null) ?
               platformContainer.position : lastPlatform.GetComponent<PlatformController>().endPoint.position;

        GameObject res = Instantiate(platform, pos, Quaternion.identity, platformContainer);
        lastPlatform = res.transform;
    }
}
