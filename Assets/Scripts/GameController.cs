using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    // Start is called before the first frame update
    void Start()
    {
        gameOverScreenUIScript = GameOverScreen.GetComponent<GameOverScreenUI>();
        
        numOfEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length;
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
        if (numOfEnemies <= 0 && !_objectiveComplete)
        {
            Instantiate(GoalDoor, new Vector3(38, -5, 0), Quaternion.identity);
            _objectiveComplete = true;
        }
        
        if (_gameWon || _gameLost)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            }
        }
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
    
    public int ReturnNumOfEnemies()
    {
        return numOfEnemies;
    }

    public void DecreaseNumOfEnemies()
    {
        numOfEnemies--;
    }
}
