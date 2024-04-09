using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int health = 3;
    private int damage = 1;
    [SerializeField] private float moveSpeed = 10f;
    
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_player == null)
            _player = GameObject.Find("Player");
        
        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // follow player
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
            moveSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
    
    }
}
