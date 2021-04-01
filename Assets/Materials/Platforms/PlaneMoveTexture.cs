using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneMoveTexture : MonoBehaviour
{
    public PlayerController playerController;
    private Renderer renderer;
    public float ScrollY = 0.5f;

    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (playerController.isRun)
        {
            float offset = ScrollY * Time.time;
            renderer.material.mainTextureOffset = new Vector2(0, offset);
        }
    }
}
