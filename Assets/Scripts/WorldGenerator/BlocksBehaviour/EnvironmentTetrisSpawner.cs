using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentTetrisSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] tetrisObject;
    [SerializeField] private int spawnChanse;

    private Rigidbody[] rb;

    void Start()
    {
        FallingTetrisFigures();
    }

    /// <summary>
    /// Падающие фигурки тетрис
    /// </summary>
    private void FallingTetrisFigures()
    {
        rb = new Rigidbody[tetrisObject.Length];

        for (int i = 0; i < tetrisObject.Length; i++)
        {
            tetrisObject[i].SetActive(Random.Range(0, 10) <= spawnChanse);

            if (tetrisObject[i].activeSelf)
            {
                rb[i] = tetrisObject[i].GetComponent<Rigidbody>();
                rb[i].drag = Random.Range(0, 3f);
                if (tetrisObject[i].transform.localPosition.x > 0)
                    tetrisObject[i].transform.localPosition = new Vector3(Random.Range(15f, 60f), Random.Range(10f, 60f), 0);
                else if (tetrisObject[i].transform.localPosition.x < 0)
                    tetrisObject[i].transform.localPosition = new Vector3(Random.Range(-60f, -15f), Random.Range(10f, 60f), 0);
            }
        }
    }
}
