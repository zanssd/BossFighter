using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class EndGameManager : MonoBehaviour
{
    public TMP_Text resultText,scoreText, scorePlayerText;

    void Start()
    {
        Time.timeScale = 1f;

        var data = SaveManager.LoadEndGame();
        if (data != null)
        {
            resultText.text = data.playerWins > data.bossWins ? "You Win!" : "Game Over";
            scoreText.text = $"Player Wins: {data.playerWins} \n Boss Wins: {data.bossWins}";
            scorePlayerText.text = $"Player 1: {data.totalScoreP1} \nPlayer 2: {data.totalScoreP2}";

        }
    }


    public void RestartFight()
    {
        SceneManager.LoadScene("BossFight");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
