using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    private GameObject _player;
    private PlayerControls _playerControls;
    private Vector3 _playerPosition;
    private Transform _transform;
    
    [SerializeField] private GameObject projectilePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerControls = _player.GetComponent<PlayerControls>();
            _playerPosition = _player.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _transform.position = transform.position;
        Vector2 direction = _playerPosition - transform.position;
        GetComponent<Projectile>().moveDirection = direction;
    }
}
