using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    public float speed = 2.5f;
    public PlayerController playerController;
    public WorldBuilder worldBuilder;
    public float minZ = 0;

    public delegate void TryToDelAndAddPlatform();
    public event TryToDelAndAddPlatform OnPlatformMovement;

    public static WorldController instance;

    private void Awake()
    {
       if (WorldController.instance != null)
       {
            Destroy(gameObject);
            return;
       }
       WorldController.instance = this;
       //DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        WorldController.instance = null;
    }
     
    void Start()
    {
        StartCoroutine(OnPlatformMovementCorutine());
        StartCoroutine(Difficulty());
    }

    void Update()
    {
        if (playerController.isRun)
            transform.position -= Vector3.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// Вызов события удаления и добавления блоков
    /// </summary>
    /// <returns></returns>
    IEnumerator OnPlatformMovementCorutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.5f);
            OnPlatformMovement?.Invoke();
        }
    }

    /// <summary>
    /// Сложность игры
    /// </summary>
    /// <returns></returns>
    IEnumerator Difficulty()
    {
        while(true)
        {
            yield return new WaitForSeconds(60f);
            speed += 1f;
            ObjectSpawner.obstacleSpawnChance += 1;
        }
    }
}
