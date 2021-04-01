using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    public Transform endPoint;

    void Start()
    {
        WorldController.instance.OnPlatformMovement += TryDelAndAddPlatform;
    }

    /// <summary>
    /// Добавление и удаление блоков
    /// </summary>
    private void TryDelAndAddPlatform()
    {
        if(transform.position.z < WorldController.instance.minZ)
        {
            WorldController.instance.worldBuilder.CreatePlatforms();
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (WorldController.instance != null)
        {
            WorldController.instance.OnPlatformMovement -= TryDelAndAddPlatform;
        }
    }
}
