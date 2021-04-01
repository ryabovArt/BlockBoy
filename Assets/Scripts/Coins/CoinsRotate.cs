using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0); // вращение монет
    }
}
