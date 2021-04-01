using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinCollect : MonoBehaviour
{
    [SerializeField] private float speed;

    public Transform target;
    public Transform init;
    public GameObject coinPrefab;
    public Animator coinAnimator;

    public TMP_Text coinCounterTxt;
    internal int coinsCounter;

    /// <summary>
    /// Определение параметров для движения подобранной монеты от игрока к счетчику монет
    /// </summary>
    public void StartCoinMove()
    {
        Vector3 initPos = new Vector3(init.position.x, init.position.y, init.position.z);
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, target.position.z);
        GameObject coin = Instantiate(coinPrefab, transform);
        StartCoroutine(MoveCoin(coin.transform, initPos, targetPos));
    }

    /// <summary>
    /// Движение подобранной монеты от игрока к счетчику монет и
    /// прирост счетчика
    /// </summary>
    /// <param name="obj"> объект монеты </param>
    /// <param name="startPos"> начальная позиция монеты </param>
    /// <param name="endPos"> конечная позиция монеты </param>
    /// <returns></returns>
    IEnumerator MoveCoin(Transform obj, Vector3 startPos, Vector3 endPos)
    {
        float time = 0;

        while (time < 1)
        {
            time += speed * Time.deltaTime;
            endPos = new Vector3(target.position.x, target.position.y, target.position.z);
            obj.position = Vector3.Lerp(startPos, endPos, time);
            yield return new WaitForEndOfFrame();
        }
        coinAnimator.SetTrigger("GetCoin");
        coinsCounter++;
        coinCounterTxt.text = coinsCounter.ToString();
        
        Destroy(obj.gameObject);
    }
}
