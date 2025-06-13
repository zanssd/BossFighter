using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject heartPrefab; // prefab Image (sprite heart)
    public Transform player1HeartsParent;
    public Transform player2HeartsParent;

    private Dictionary<PlayerController, List<Image>> playerHearts = new Dictionary<PlayerController, List<Image>>();

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterPlayer(PlayerController player, int maxHearts)
    {
        Transform parent = player.inputConfig.name.Contains("1") ? player1HeartsParent : player2HeartsParent;
        List<Image> hearts = new List<Image>();

        for (int i = 0; i < maxHearts; i++)
        {
            GameObject heart = Instantiate(heartPrefab, parent);
            hearts.Add(heart.GetComponent<Image>());
        }

        playerHearts[player] = hearts;
    }

    public void UpdateHealth(PlayerController player, int currentHealth)
    {
        if (!playerHearts.ContainsKey(player)) return;

        List<Image> hearts = playerHearts[player];
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].enabled = i < currentHealth;
        }
    }
}
