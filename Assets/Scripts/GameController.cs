using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private int numOfEnemies;
    private GameObject player;
    private PlayerControls playerControls;
    [SerializeField] private GameObject GoalDoor;
    [SerializeField] private GameObject GameOverScreen;
    private GameOverScreenUI gameOverScreenUIScript;
    
    private bool _objectiveComplete = false;
    private bool _gameWon = false;
    private bool _gameLost = false;
    
    public struct GameData
    {
        public int _health { get; set; }
        public int _maxHealth { get; set; }
        public int _ki { get; set; }
        public int _maxKi { get; set; }
        public int _level { get; set; }
        public int _exp { get; set; }
        public float _fireDamage { get; set; }
        public float _iceFreezeTime { get; set; }
        public float _shurikenFireRate { get; set; }
    }
    
    public static GameData gameData = new GameData();
    
    public static GameController Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    [RuntimeInitializeOnLoadMethod]
    public static void Initialize()
    {
        gameData._maxHealth = 5;
        gameData._health = 5;
        gameData._maxKi = 5;
        gameData._ki = 5;
        gameData._level = 1;
        gameData._exp = 4;
        gameData._fireDamage = 1f;
        gameData._iceFreezeTime = 2f;
        gameData._shurikenFireRate = 0.5f;
    }
    
    // Start is called before the first frame update
    void Start()
    { 
        gameOverScreenUIScript = GameOverScreen.GetComponent<GameOverScreenUI>();
        
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerControls = player.GetComponent<PlayerControls>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControls.GetHealth() <= 0 && !_gameLost)
        {
            // Freeze the game
            gameOverScreenUIScript._gameOverText = "You Died!";
            Instantiate(GameOverScreen, new Vector3(0, 0, 0), Quaternion.identity);
            Time.timeScale = 0;
            _gameLost = true;
        }
        
        if (_gameWon || _gameLost)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;

                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
    
    public void TransferToNextLevel()
    {
        StartCoroutine(GetPlayerDataToGameData());
        
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            WinGame();
            return;
        }
        Time.timeScale = 1;
        SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex + 1);
    }
    
    public IEnumerator GetPlayerDataToGameData()
    {
        gameData._maxHealth = playerControls.GetMaxHealth();
        gameData._health = playerControls.GetHealth();
        gameData._maxKi = playerControls.GetMaxKi();
        gameData._ki = playerControls.GetKi();
        gameData._level = playerControls.GetLevel();
        gameData._exp = playerControls.GetExp();
        gameData._fireDamage = playerControls.GetFireDamage();
        gameData._iceFreezeTime = playerControls.GetIceFreezeTime();
        yield return true;
    }
    
    public bool GetObjectiveComplete()
    {
        return _objectiveComplete;
    }
    
    public bool GetGameWon()
    {
        return _gameWon;
    }
    
    public bool GetGameLost()
    {
        return _gameLost;
    }
    
    public void WinGame()
    {
        gameOverScreenUIScript._gameOverText = "You Win!";
        Instantiate(GameOverScreen, new Vector3(0, 0, 0), Quaternion.identity);
        Time.timeScale = 0;
        _gameWon = true;
    }
}
