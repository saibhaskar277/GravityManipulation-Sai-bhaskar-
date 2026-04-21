using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TMP_Text timerText;
    public TMP_Text coinText;
    public Button restatButton;

    void Start()
    {
        GameManager.Instance.OnTimeChanged += UpdateTimer;
        GameManager.Instance.OnCoinsUpdated += UpdateCoins;
        GameManager.Instance.OnGameOver += ShowGameOver;
        GameManager.Instance.OnGameWin += ShowWin;
        restatButton.onClick.AddListener(() => SceneManager.LoadScene(0));
    }

    

    void OnDestroy()
    {
        GameManager.Instance.OnTimeChanged -= UpdateTimer;
        GameManager.Instance.OnCoinsUpdated -= UpdateCoins;
        GameManager.Instance.OnGameOver -= ShowGameOver;
        GameManager.Instance.OnGameWin -= ShowWin;
    }

    void UpdateTimer(float time)
    {
        time = Mathf.Max(0, time);

        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);

        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    void UpdateCoins(int collected, int total)
    {
        coinText.text = $"{collected} / {total}";
    }

    void ShowGameOver()
    {
        SceneManager.LoadScene(0);
        //restatButton.gameObject.SetActive(true);
    }

    void ShowWin()
    {
        SceneManager.LoadScene(0);
       // restatButton.gameObject.SetActive(true);
    }
}