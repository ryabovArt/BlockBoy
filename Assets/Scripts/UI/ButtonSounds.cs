using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSounds : MonoBehaviour
{
    public AudioSource[] buttonSounds;

    /// <summary>
    /// Звук при наведении курсора на кнопку
    /// </summary>
    public void Highligted()
    {
        buttonSounds[0].Play();
    }

    /// <summary>
    /// Звук при клике по кнопке
    /// </summary>
    public void Click()
    {
        buttonSounds[1].Play();
    }

    /// <summary>
    /// Фоновая музыка в меню
    /// </summary>
    public void MenuBackground()
    {
        buttonSounds[2].Play();
    }
}
