using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTetrisBlockInMenu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstruction"))
        {
            StartCoroutine(Destroyer(other));
        }
    }

    /// <summary>
    /// Уничтожаем упавшую фигурку тетрис
    /// </summary>
    /// <param name="col"> фигурка тетрис </param>
    /// <returns> каждые 10 сек </returns>
    IEnumerator Destroyer(Collider col)
    {
        yield return new WaitForSeconds(10f);
        if (col != null)
            Destroy(col.gameObject);
    }
}
