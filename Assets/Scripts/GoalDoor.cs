using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    
    private GameObject _gameController;
    private GameController _gameControllerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.Find("GameController");
        if (_gameController != null)
        {
            _gameControllerScript = _gameController.GetComponent<GameController>();
        }
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _gameControllerScript.WinGame();
        }
    }
}