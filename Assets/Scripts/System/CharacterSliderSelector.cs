using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSliderSelector : MonoBehaviour
{
    public Transform characterHolder;
    public float slideDistance = 3f;
    public float slideSpeed = 5f;
    public int totalCharacters = 3;

    public TMP_Text instructionText;
    public Button confirmButton;
    public Button startGameButton;

    private int currentIndex = 0;
    private Vector3 targetPos;
    private bool isSliding = false;

    private int selectedP1 = -1;
    private int selectedP2 = -1;
    private bool selectingPlayer1 = true;

    void Start()
    {
        UpdateTargetPosition();
        startGameButton.gameObject.SetActive(false);
        confirmButton.gameObject.SetActive(true);
        instructionText.text = "Player 1: Choose Your Character";
    }

    public void Next()
    {
        if (isSliding || currentIndex >= totalCharacters - 1) return;
        currentIndex++;
        UpdateTargetPosition();
    }

    public void Previous()
    {
        if (isSliding || currentIndex <= 0) return;
        currentIndex--;
        UpdateTargetPosition();
    }

    void UpdateTargetPosition()
    {
        targetPos = new Vector3(-currentIndex * slideDistance, 0, 0);
        StopAllCoroutines();
        StartCoroutine(SmoothSlide());
    }

    IEnumerator SmoothSlide()
    {
        isSliding = true;
        while (Vector3.Distance(characterHolder.localPosition, targetPos) > 0.01f)
        {
            characterHolder.localPosition = Vector3.Lerp(characterHolder.localPosition, targetPos, Time.deltaTime * slideSpeed);
            yield return null;
        }
        characterHolder.localPosition = targetPos;
        isSliding = false;
    }

    public void ConfirmSelection()
    {
        if (selectingPlayer1)
        {
            selectedP1 = currentIndex;
            selectingPlayer1 = false;
            currentIndex = 0;
            UpdateTargetPosition();
            instructionText.text = "Player 2: Choose Your Character";
        }
        else
        {
            selectedP2 = currentIndex;

            CharacterSelectionManager.selectedCharacterIndexP1 = selectedP1;
            CharacterSelectionManager.selectedCharacterIndexP2 = selectedP2;

            instructionText.text = "Ready to Fight!";
            confirmButton.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(true);
        }
    }

    public void StartBossFight()
    {
        SceneManager.LoadScene("TransitionScene");
    }
}
