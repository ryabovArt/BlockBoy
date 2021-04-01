using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects : MonoBehaviour
{
    public static Effects instance = null;

    [SerializeField] private ParticleSystem[] effects;

    public GameObject coin2D;

    public CoinCollect coinCollect;

    public AudioSource[] sounds;

    private bool isRightLeg = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance == this) Destroy(gameObject);
    }

    private void Start()
    {
        Invoke("BackgroundMusicPlay", PlayerController.instance.startTime);
    }

    internal void BackgroundMusicPlay()
    {
        sounds[3].Play();
    }

    /// <summary>
    /// Эффект пыли при беге
    /// </summary>
    public void MoveEffects()
    {
        if (isRightLeg)
        {
            effects[0].Play();
            isRightLeg = false;
        }
        else
        {
            effects[1].Play();
            isRightLeg = true;
        }
        sounds[0].pitch = Random.Range(0.9f, 1.1f);
        sounds[0].Play();
    }

    /// <summary>
    /// Эффект пыли при прыжке
    /// </summary>
    public void JumpDust()
    {
        effects[2].Play();
    }

    /// <summary>
    /// Сбор монет
    /// </summary>
    /// <param name="col"></param>
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Coin"))
        {
            StartCoroutine(SetActiveCoinSprite());
            effects[3].Emit(50);
            sounds[1].Play();
            if (!PlayerController.instance.characterController.isGrounded)
                sounds[1].pitch += 0.01f;
            else
                sounds[1].pitch = 1f;
            coinCollect.StartCoinMove();
            Destroy(col.gameObject);
        }
    }

    /// <summary>
    /// Включение и отключения объекта Coin при сборе монет
    /// </summary>
    /// <returns></returns>
    private IEnumerator SetActiveCoinSprite()
    {
        coin2D.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        coin2D.SetActive(false);
    }

    /// <summary>
    /// Эффект удара о препятствие
    /// </summary>
    public void Knock()
    {
        effects[6].Emit(50);
    }

    /// <summary>
    /// Эффект удара о препятствие (звездочки)
    /// </summary>
    public void DeathEffect()
    {
        effects[4].Play();
        effects[5].Play();
    }

    /// <summary>
    /// Эффект дыма при полете
    /// </summary>
    public void FlySmoke()
    {
        effects[7].Play();
        effects[8].Play();
    }
    public void StopFlySmoke()
    {
        effects[7].Stop();
        effects[8].Stop();
    }

    /// <summary>
    /// Звук остановки после полета
    /// </summary>
    public void StopFlySound()
    {
        sounds[11].Play();
    }
}
