using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField] private int heal = 5;
    
    private GameObject _player;
    private PlayerControls _playerControls;
    [SerializeField] private bool isHealthPotion = true;
    
    // Start is called before the first frame update
    void Start()
    {   
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerControls = _player.GetComponent<PlayerControls>();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerControls.Heal(isHealthPotion, heal);
            Destroy(gameObject);
        }
    }
}
