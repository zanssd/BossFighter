using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject[] characterPrefabs;
    public Transform spawnPointP1;
    public Transform spawnPointP2;
    [SerializeField]
    private PlayerInputConfig player1Input,player2Input;

    void Start()
    {
        int index1 = CharacterSelectionManager.selectedCharacterIndexP1;
        int index2 = CharacterSelectionManager.selectedCharacterIndexP2;

        GameObject player1 = Instantiate(characterPrefabs[index1], spawnPointP1.position, Quaternion.identity);
        GameObject player2 = Instantiate(characterPrefabs[index2], spawnPointP2.position, Quaternion.identity);

        PlayerController player1Controller = player1.GetComponent<PlayerController>();
        player1Controller.inputConfig = player1Input;
        player1Controller.playerID = 1;
        PlayerController player2Controller = player2.GetComponent<PlayerController>();
        player2Controller.inputConfig = player2Input;
        player2Controller.playerID = 2;
    }
}

