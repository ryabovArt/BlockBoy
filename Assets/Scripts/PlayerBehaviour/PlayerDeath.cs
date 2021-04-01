using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerDeath : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject pauseButton;
    public GameObject swipePanel;

    public HighScore highScore;
    public CoinCollect coinCollect;

    private void Start()
    {
        PlayerController.instance.IfPlayerDeath += Death;
    }

    /// <summary>
    /// Смерть персонажа
    /// </summary>
    private void Death()
    {
        StartCoroutine(DeathActions());
    }

    /// <summary>
    /// Действия при смерти игрока
    /// </summary>
    /// <returns></returns>
    IEnumerator DeathActions()
    {
        PlayerController.instance.playerAnimator.SetTrigger("Death");
        PlayerController.instance.isRun = false;
        PlayerController.instance.rb.useGravity = true;
        PlayerController.instance.colliderInDeath.enabled = true;
        PlayerController.instance.colliderInDeath.isTrigger = false;
        PlayerController.instance.characterController.enabled = false;
        WorldController.instance.speed = 0f;

        SetPoints();

        yield return new WaitForSeconds(3f);

        SetActiveGameoverPanel();
    }

    /// <summary>
    /// Сохранение очков после столкновения
    /// </summary>
    private void SetPoints()
    {
        int temp = coinCollect.coinsCounter;
        highScore.temp += temp.ToString();
        highScore.Score();
    }

    /// <summary>
    /// Активируем панель с результатами и отключаем 
    /// кнопку паузы и свайп панель для того, чтобы сделать кликабельными кнопки
    /// </summary>
    private void SetActiveGameoverPanel()
    {
        gameOverPanel.SetActive(true);
        pauseButton.SetActive(false);
        swipePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        if (PlayerController.instance != null)
            PlayerController.instance.IfPlayerDeath -= Death;
    }
}
