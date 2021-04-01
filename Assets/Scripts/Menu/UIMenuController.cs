using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuController : MonoBehaviour
{
    public UIController uiController;

    /// <summary>
    /// Запуск уровня
    /// </summary>
    public void PlayGame()
    {
        StartCoroutine(PlayGameCorutine());
    }
    IEnumerator PlayGameCorutine()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
    }

    /// <summary>
    /// Вуключение меню настрорек
    /// </summary>
    public void Settings()
    {
        StartCoroutine(SettingsCorutine());
    }
    IEnumerator SettingsCorutine()
    {
        yield return new WaitForSeconds(0.4f);
        uiController.pausePanel.SetActive(false);
        uiController.settingsPanel.SetActive(true);
    }

    /// <summary>
    /// Выход из игры
    /// </summary>
    public void Quit()
    {
        StartCoroutine(QuitCorutine());
    }
    IEnumerator QuitCorutine()
    {
        yield return new WaitForSeconds(0.5f);
        Application.Quit();
    }
}
