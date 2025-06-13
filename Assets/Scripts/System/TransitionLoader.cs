using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionLoader : MonoBehaviour
{
    public Slider loadingBar;
    public float loadDuration = 45f;

    private float timer = 0f;

    void Start()
    {
        loadingBar.value = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        loadingBar.value = Mathf.Clamp01(timer / loadDuration);

        if (timer >= loadDuration)
        {
            SceneManager.LoadScene("BossFight");
        }
    }
}
