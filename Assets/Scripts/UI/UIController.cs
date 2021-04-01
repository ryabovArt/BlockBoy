using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gameOverPanel;

    public GameObject buttonPause;
    public GameObject settingsPanel;
    public GameObject swipePanel;

    public Toggle toggleMusicOnOff;
    public Toggle toggleSFXOnOff;
    public Slider sliderChangeVolume;

    public AudioMixerGroup mixer;

    public AudioMixerSnapshot normal;
    public AudioMixerSnapshot menu;

    private void Start()
    {
        SetUIElementsValue();
        //PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// Проверяем наличие сохраненных настроес
    /// </summary>
    private void SetUIElementsValue()
    {
        if (PlayerPrefs.HasKey("MusicOnOff"))
        {
            toggleMusicOnOff.isOn = PlayerPrefs.GetInt("MusicOnOff") == 1;
            toggleSFXOnOff.isOn = PlayerPrefs.GetInt("SFXOnOff") == 1;
            sliderChangeVolume.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        }
        else
        {
            toggleMusicOnOff.isOn = true;
            toggleSFXOnOff.isOn = true;
            sliderChangeVolume.value = 1f;
        }
    }

    /// <summary>
    /// Загружаем уровень
    /// </summary>
    public void LoadGame()
    {
        StartCoroutine(LoadGameCorutine());
    }
    IEnumerator LoadGameCorutine()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Game");
        gameOverPanel.SetActive(false);
    }

    /// <summary>
    /// Активируем панель настроек(пауза)
    /// </summary>
    public void SettingsOn()
    {
        buttonPause.SetActive(false);
        pausePanel.SetActive(true);
        swipePanel.SetActive(false);
        menu.TransitionTo(0);
        Time.timeScale = 0;
    }

    /// <summary>
    /// Деактивируем панель настроек(пауза)
    /// </summary>
    public void SettingsOff()
    {
        buttonPause.SetActive(true);
        pausePanel.SetActive(false);
        swipePanel.SetActive(true);
        normal.TransitionTo(0);
        Time.timeScale = 1;
    }

    /// <summary>
    /// Выход в меню
    /// </summary>
    public void GoToMenu()
    {
        Time.timeScale = 1;
        StartCoroutine(GoToMenuCorutine());
    }
    IEnumerator GoToMenuCorutine()
    {
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Menu");
    }

    /// <summary>
    /// Включение и отключение музыки
    /// </summary>
    /// <param name="enabled"> параметр вкл\выкл </param>
    public void MusicOnOff(bool enabled)
    {
        if (enabled)
            mixer.audioMixer.SetFloat("MusicOnOff", 0);
        else
            mixer.audioMixer.SetFloat("MusicOnOff", -80);

        PlayerPrefs.SetInt("MusicOnOff", enabled ? 1 : 0);
    }

    /// <summary>
    /// Включение и отключение звуков
    /// </summary>
    /// <param name="enabled"> параметр вкл\выкл </param>
    public void SFXOnOff(bool enabled)
    {
        if (enabled)
            mixer.audioMixer.SetFloat("SFXOnOff", 0);
        else
            mixer.audioMixer.SetFloat("SFXOnOff", -80);

        PlayerPrefs.SetInt("SFXOnOff", enabled ? 1 : 0);
    }

    /// <summary>
    /// Уровень громкости
    /// </summary>
    /// <param name="volume"> параметр уровня громкости </param>
    public void ChangeVolume(float volume)
    {
        mixer.audioMixer.SetFloat("MasterVolume", Mathf.Lerp(-50, 0, volume));
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
