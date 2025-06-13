using UnityEngine;
using System.IO;

public static class SaveManager
{
    private static string path = Application.persistentDataPath + "/endgame.json";

    [System.Serializable]
    public class EndGameStats
    {
        public int playerWins = 0;
        public int bossWins = 0;

        public int totalScoreP1 = 0;
        public int totalScoreP2 = 0;
    }

    public static void SaveEndGame(bool win)
    {
        EndGameStats data = LoadEndGame() ?? new EndGameStats();

        if (win)
            data.playerWins++;
        else
            data.bossWins++;

        File.WriteAllText(path, JsonUtility.ToJson(data, true));
    }

    public static EndGameStats LoadEndGame()
    {
        if (!File.Exists(path)) return null;

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<EndGameStats>(json);
    }

    public static void AddPlayerScore(int playerIndex, int score)
    {
        var data = LoadEndGame();
        if (data == null) data = new EndGameStats();

        if (playerIndex == 1) data.totalScoreP1 += score;
        else if (playerIndex == 2) data.totalScoreP2 += score;

        SaveEndGame(data);
    }

    private static void SaveEndGame(EndGameStats data)
    {
        File.WriteAllText(path, JsonUtility.ToJson(data, true));
    }

}
