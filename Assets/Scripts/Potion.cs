using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int heal = 1;
    
    private GameObject _player;
    private PlayerMovement _playerMovement;
    [SerializeField] private bool isHealthPotion = true;
    
    // Start is called before the first frame update
    void Start()
    {   
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerMovement = _player.GetComponent<PlayerMovement>();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerMovement.Heal(isHealthPotion, heal);
            Destroy(gameObject);
        }
    }
}
