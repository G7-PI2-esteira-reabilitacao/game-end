using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] lives;
    public Text    coinText;
    public Text    scoreText;

    public void UpdateLives(int currentLives)
    {
        if (lives != null && lives.Length > 0)
        {
            for (int i = 0; i < lives.Length; i++)
            {
                if (lives[i] != null)
                    lives[i].color = currentLives > i ? Color.white : Color.black;
            }
        }
    }

    public void UpdateCoins(int coint)
    {
        if (coinText != null)
            coinText.text = coint.ToString();
    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }
}