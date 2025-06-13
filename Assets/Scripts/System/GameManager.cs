using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private List<PlayerController> players = new List<PlayerController>();
    private int deadPlayerCount = 0;
    public GameObject boss;
    public int bestScore = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void RegisterPlayer(PlayerController player)
    {
        if (!players.Contains(player))
            players.Add(player);
    }

    public void PlayerDied(PlayerController player)
    {
        deadPlayerCount++;
        if (deadPlayerCount >= players.Count)
        {
            EndGame(false);
        }
    }

    public void BossDefeated()
    {
        EndGame(true);
    }

    void EndGame(bool win)
    {
        Time.timeScale = 0f;
        SaveManager.SaveEndGame(win);
        StartCoroutine(LoadEndGameScene());
    }


    IEnumerator LoadEndGameScene()
    {
        yield return new WaitForSecondsRealtime(0.1f);
        Time.timeScale = 1f;
        SceneManager.LoadScene("EndGame");
    }
}
