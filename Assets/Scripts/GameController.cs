using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private int numOfEnemies;
    private GameObject player;
    private PlayerControls playerControls;
    [SerializeField] private GameObject GoalDoor;
    private bool _gameWon = false;
    
    // Start is called before the first frame update
    void Start()
    {
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
        if (playerControls.GetHealth() <= 0)
        {
            // Freeze the game
            Time.timeScale = 0;
        }
        if (numOfEnemies <= 0 && !_gameWon)
        {
            Instantiate(GoalDoor, new Vector3(38, -5, 0), new Quaternion(0, 0, 90, 0));
            _gameWon = true;
        }
    }
    
    public void WinGame()
    {
        // Freeze the game
        Time.timeScale = 0;
    }
    
    public int ReturnNumOfEnemies()
    {
        return numOfEnemies;
    }

    public void UpdateNumOfEnemies(int value)
    {
        numOfEnemies = value;
    }
}
