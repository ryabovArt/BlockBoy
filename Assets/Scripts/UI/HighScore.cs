using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI[] scoreText;

    internal List<int> bestResult = new List<int>(3) { 0, 0, 0 };

    internal int currentScore;
    internal string temp = string.Empty;

    private void Start()
    {
        //PlayerPrefs.DeleteKey("SaveScore");
        //bestResult.Clear();
        CheckPlayerPrefs();
    }

    /// <summary>
    /// Проверка наличия сохраненных данных
    /// </summary>
    private void CheckPlayerPrefs()
    {
        if (PlayerPrefs.HasKey("SaveScore"))
            return;
        else
            PlayerPrefs.SetString("SaveScore", "0 1 2");
    }

    /// <summary>
    /// Сохраняем очки и сортируем по убыванию
    /// </summary>
    public void Score()
    {
        temp += $" {PlayerPrefs.GetString("SaveScore")}";
        PlayerPrefs.SetString("SaveScore", temp);

        string[] numbers = temp.Split(new char[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < numbers.Length; i++)
        {
            int a = int.Parse(numbers[i]);
            bestResult.Add(a);
        }

        bestResult.Sort();
        bestResult.Reverse();

        int index = 0;

        while (index < bestResult.Count - 1)
        {
            if (bestResult[index] == bestResult[index + 1])
            {
                bestResult.RemoveAt(index);
            }
            else
                index++;
        }

        for (int i = 0; i < scoreText.Length; i++)
        {
            scoreText[i].text = bestResult[i].ToString();
            Debug.Log(bestResult[i]);
        }
    }
}
