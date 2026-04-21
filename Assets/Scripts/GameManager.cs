using UnityEngine;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Timer")]
    public float gameDuration = 120f; // 2 minutes
    private float timeRemaining;

    public bool IsGameOver { get; private set; }

    public event Action<float> OnTimeChanged;
    public event Action OnGameOver;
    public event Action OnGameWin;

    private int totalCoins = 5;
    private int collectedCoins;

    public event Action<int, int> OnCoinsUpdated;

    void Awake()
    {
        Instance = this;
    }


    private void OnEnable()
    {
        EventManager.ListenEvent<OnCoinCollected>(CollectCoin);
        EventManager.ListenEvent<OnPlayerFell>(PlayerFell);
    }

    void Start()
    {
        timeRemaining = gameDuration;

        // auto find coins in scene
        collectedCoins = 0;

        OnCoinsUpdated?.Invoke(collectedCoins, totalCoins);
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Q))
        {
            Application.Quit();
        }

        if (IsGameOver) return;

        HandleTimer();
    }

    void HandleTimer()
    {
        timeRemaining -= Time.deltaTime;
        OnTimeChanged?.Invoke(timeRemaining);

        if (timeRemaining <= 0f)
        {
            GameOver();
        }
    }

    public void CollectCoin(OnCoinCollected e)
    {  
        if (IsGameOver) return;

        collectedCoins++;
        OnCoinsUpdated?.Invoke(collectedCoins, totalCoins);

        if (collectedCoins >= totalCoins)
        {
            GameWin();
        }
    }

    public void PlayerFell(OnPlayerFell e)
    {
        if (IsGameOver) return;
        GameOver();
    }

    void GameOver()
    {
        IsGameOver = true;
        OnGameOver?.Invoke();
        Debug.Log("GAME OVER");
    }

    void GameWin()
    {
        IsGameOver = true;
        OnGameWin?.Invoke();
        Debug.Log("YOU WIN");
    }

     
    private void OnDisable()
    {
        EventManager.StopListening<OnCoinCollected>(CollectCoin);
        EventManager.StopListening<OnPlayerFell>(PlayerFell);
    }
}