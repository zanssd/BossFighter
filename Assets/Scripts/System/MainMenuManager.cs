using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenuManager : MonoBehaviour
{

    [Header("Settings")]
    public Toggle musicToggle;
    public Toggle sfxToggle;

    [Header("Stats")]
    public TMP_Text scoreText, scorePlayerText;

    private void Start()
    {
        //LoadSettings();
        LoadStats();
    }


    public void ToggleMusic(bool isOn)
    {
        AudioManager.Instance.SetMusicEnabled(isOn);
        SaveSettings();
    }

    public void ToggleSFX(bool isOn)
    {
        AudioManager.Instance.SetSFXEnabled(isOn);
        SaveSettings();
    }

    void LoadStats()
    {
        var data = SaveManager.LoadEndGame();
        if (data != null)
        {
            scoreText.text = $"Player Wins: {data.playerWins} \n Boss Wins: {data.bossWins}";
            scorePlayerText.text = $"Player 1: {data.totalScoreP1} \nPlayer 2: {data.totalScoreP2}";
        }
        else
        {
            scoreText.text = "No game data yet.";
            scorePlayerText.text = "No game data yet.";
        }
    }


    void LoadSettings()
    {
        bool musicOn = SettingsData.Load().musicOn;
        bool sfxOn = SettingsData.Load().sfxOn;

        musicToggle.isOn = musicOn;
        sfxToggle.isOn = sfxOn;

        AudioManager.Instance.SetMusicEnabled(musicOn);
        AudioManager.Instance.SetSFXEnabled(sfxOn);
    }

    void SaveSettings()
    {
        SettingsData.Save(musicToggle.isOn, sfxToggle.isOn);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
