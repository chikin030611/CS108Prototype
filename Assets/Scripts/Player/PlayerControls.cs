using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    // Character Variables
    private int _health;
    private int _maxHealth = 5;
    private int _ki;
    private int _maxKi = 5;
    private Vector3 _respawnPoint;
    private int _level = 1;
    private int _exp = 4;
    private int _expToLevelUp = 5;
    private float _fireDamage = 1f;
    private float _iceFreezeTime = 2f;
    
    // Movement variables
    private float _horizontal;
    private float _speed = 8f;
    [SerializeField] private float jumpingPower = 6f;
    private int _numOfJump = 0;
    public int maxNumOfJump = 2;
    private bool _isRunning = false;
    private bool _isFacingRight = true;
    private bool _isAttacking = false;
    private bool _isKnockedBack = false;
    private bool _isNinjutsuCooledDown = false;
    private bool _isLeveledUp = false;
    
    // Sword Attack Variables
    [SerializeField] private GameObject swordCollision;
    
    // Ninjutsu Attack Variables
    [SerializeField] private GameObject fireNinjutsu;
    [SerializeField] private GameObject iceNinjutsu;
    
    // Projectile Variables
    private bool _isShurikenCoolDown;
    private float _shurikenFireRate = 1.5f;
    private float _shurikenCoolDownTime = 1.5f;
    private float _ninjutsuCoolDownTime = 2f;
    private Vector2 _shotDir = Vector2.right; // Sets the direction to fire bullet
    
    // Components
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private BoxCollider2D _playerCollider;
    private BoxCollider2D _damageCollider;
    private CapsuleCollider2D _groundCheck;
    private Animator _anim;
    private LayerMask _groundLayer;
    private BoxCollider2D _voidCollider;
    [SerializeField] private GameObject projectilePrefab;
    
    public int GetHealth() { return _health; }
    public int GetMaxHealth() { return _maxHealth; }
    public int GetKi() { return _ki; }
    public int GetMaxKi() { return _maxKi; }
    public int GetLevel() { return _level; }
    public int GetExp() { return _exp; }
    public int GetExpToLevelUp() { return _expToLevelUp; }
    public bool GetIsNinjutsuCoolDown() { return _isNinjutsuCooledDown; }
    public float GetNinjutsuCoolDownTime() { return _ninjutsuCoolDownTime; }
    public bool GetIsShurikenCoolDown() { return _isShurikenCoolDown; }
    public float GetShurikenCoolDownTime() { return _shurikenCoolDownTime; }
    
    // Debug Method
    public Dictionary<String, String> DebugDictionary()
    {
        Dictionary<String, String> debug = new Dictionary<String, String>();
        // debug.Add("Health", _health.ToString() + "/" + _maxHealth.ToString());
        // debug.Add("Ki", _ki.ToString() + "/" + _maxKi.ToString());
        debug.Add("Shuriken Cooldown", _isShurikenCoolDown.ToString());
        // debug.Add("Speed", _speed.ToString());
        // debug.Add("Number of Jump", _numOfJump.ToString());
        // debug.Add("Is Facing Right", _isFacingRight.ToString());
        // debug.Add("Shot Cooldown", _shotCooldown.ToString());
        // debug.Add("Shot Direction", _shotDir.ToString());
        // debug.Add("Is Grounded", IsGrounded().ToString());
        // debug.Add("Is Running", _isRunning.ToString());
        // debug.Add("Is Attacking", _isAttacking.ToString());
        // debug.Add("Colliding with", CheckCollision()[0]);
        
        return debug;
    }
    
    // Checking what the player is colliding with
    private string[] CheckCollision()
    {
        // Get the playerCollider's bounds
        Bounds playerBounds = _playerCollider.bounds;

        // Get all colliders that overlap with the playerCollider
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(playerBounds.center, playerBounds.size, 0f);

        // Create a list to store the names of the GameObjects
        List<string> gameObjectNames = new List<string>();

        // Iterate over the overlapping colliders
        foreach (Collider2D collider in overlappingColliders)
        {
            // If the collider is not the playerCollider itself
            if (collider != _playerCollider)
            {
                // Add the name of the GameObject to the list
                gameObjectNames.Add(collider.gameObject.name);
            }
        }

        // Convert the list to an array and return it
        return gameObjectNames.ToArray();
    }
    
    void Start()
    {
        // Initialize Variables
        _health = _maxHealth;
        _ki = _maxKi;
        _groundLayer = LayerMask.GetMask("Ground");
        _respawnPoint = new Vector3(-26, -4, 0);
        
        // Get Components
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerCollider = GetComponent<BoxCollider2D>();
        _damageCollider = transform.GetChild(0).GetComponent<BoxCollider2D>();
        _groundCheck = GetComponent<CapsuleCollider2D>();
        _anim = GetComponent<Animator>();
        // _voidCollider = GameObject.Find("Void Collider").GetComponent<BoxCollider2D>();
    }
    
    private void FixedUpdate()
    {
        // Movement
        if (!_isKnockedBack)
            _rigidbody.velocity = new Vector2(_horizontal * _speed, _rigidbody.velocity.y);
    }

    void Update()
    {
        // Input
        _horizontal = Input.GetAxisRaw("Horizontal");
        _isRunning = _horizontal != 0;
        _anim.SetBool("isRunning", _horizontal != 0);
        
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            _numOfJump++;
            if (_numOfJump < maxNumOfJump)
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpingPower);
                _anim.SetBool("isJumping", true);
            }
        }
        
        // Jump Power Control
        if (Input.GetButtonUp("Jump") && _rigidbody.velocity.y > 0f)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * 0.5f);
        }
        
        // Reset Jumping Count
        if (IsGrounded())
        {
            _numOfJump = 0;
            _anim.SetBool("isJumping", false);
        } 
        else 
        {
            _anim.SetBool("isJumping", true);
        }
        
        // Use Sword Attack
        if (Input.GetButtonDown("Sword"))
        {
            _isAttacking = true;
            _anim.SetTrigger("Attack");
            if (_isRunning)
            {
                StartCoroutine(RunAttack());
            }
            else
            {
                Instantiate(swordCollision,
                    transform.position + new Vector3(_shotDir.x, _shotDir.y, 0),
                    Quaternion.identity);
                StartCoroutine(Wait(0.8f));
            }
        }
        else
        {
            _isAttacking = false;
        }
             
        // Shoot Shuriken
        if (Input.GetButtonDown("Shuriken") && !_isShurikenCoolDown)
        {
            ShootShuriken();
        }

        // Ninjutsu
        if (!_isNinjutsuCooledDown && (Input.GetButtonDown("Fire") || Input.GetButtonDown("Ice")) && _ki > 0)
        {
            UseNinjutsu();
        }

        // Health System
        if (_health <= 0)
        {
            // TODO: Die
        }
        
        // Level System
        if (_exp >= _expToLevelUp)
        {
            
            _exp = 0;
            _level++;
            _maxHealth += 1;
            _health = _maxHealth;
            _maxKi += 1;
            _ki = _maxKi;
            _shurikenFireRate -= .2f;
            _fireDamage += 0.5f;
            _iceFreezeTime += 1.5f;
            _isLeveledUp = true;
        }
        
        // Flip the character
        Flip(); 
    }
    


    // Wait for a certain amount of time
    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }
    
    // Running while attacking
    IEnumerator RunAttack()
    {
        _speed = 4f;
        Instantiate(swordCollision,
            transform.position + new Vector3(_shotDir.x, _shotDir.y, 0),
            Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        _speed = 8f;
    }
    
    // Use Ninjutsu
    private void UseNinjutsu()
    {
        // Fire Ninjutsu
        if (Input.GetButtonDown("Fire"))
        {
            GameObject prefab = Instantiate(fireNinjutsu);
            prefab.GetComponent<FireNinjutsu>().damage = _fireDamage;
            prefab.transform.position = transform.position;
            prefab.GetComponent<Projectile>().moveDirection = _shotDir;
            _ki -= prefab.GetComponent<FireNinjutsu>().GetKiCost();
        }

        // Ice Ninjutsu
        if (Input.GetButtonDown("Ice") && IsGrounded())
        {
            StartCoroutine(IceNinjutsuPattern());
            _ki -= iceNinjutsu.GetComponent<IceNinjutsu>().GetKiCost();
        }
            
        _isNinjutsuCooledDown = true;
        StartCoroutine(NinjutsuCoolDown());
    }
    
    // Ninjutsu Cooldown
    IEnumerator NinjutsuCoolDown()
    {
        float originalTime = _ninjutsuCoolDownTime;
        while (_ninjutsuCoolDownTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            _ninjutsuCoolDownTime -= 0.1f;
        }
        _isNinjutsuCooledDown = false;
        _ninjutsuCoolDownTime = originalTime;
    }
    
    // Shoot Shuriken
    private void ShootShuriken()
    {
        _isShurikenCoolDown = true;
        
        StartCoroutine(FireProjectilePattern(_shotDir));
        StartCoroutine(FireProjectilePattern(_shotDir + new Vector2(0, 0.45f)));
        StartCoroutine(FireProjectilePattern(_shotDir + new Vector2(0, -0.45f)));
        
        StartCoroutine(ShurikenCoolDown());
    }
    
    // Shuriken Cooldown
    IEnumerator ShurikenCoolDown()
    {
        float originalTime = _shurikenCoolDownTime;
        while (_shurikenCoolDownTime > 0)
        {
            yield return new WaitForSeconds(0.1f);
            _shurikenCoolDownTime -= 0.1f;
        }
        _isShurikenCoolDown = false;
        _shurikenCoolDownTime = originalTime;
    }

    
    // Fire Projectile Pattern
    IEnumerator FireProjectilePattern(Vector2 direction)
    {
        for (int j = 0; j < 2; j++)
        {
            GameObject prefab = Instantiate(projectilePrefab);
            prefab.transform.position = transform.position;
            prefab.GetComponent<Projectile>().moveDirection = direction;
            yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds between each shot
        }
    }
    
    // Ice Ninjutsu Pattern
    IEnumerator IceNinjutsuPattern()
    {
        Vector3 position = transform.position;
        bool dir = _isFacingRight;
        float interval = _isFacingRight? 2.2f : -2.2f;
        float x = _shotDir.x + (_isFacingRight? 0.7f : -0.7f);
        float y = _shotDir.y + 0.5f;
        for (int i = 0; i < 3; i++)
        {
            GameObject prefab = Instantiate(iceNinjutsu,
                position + new Vector3(x + i * interval, y, 0),
                Quaternion.identity);
            prefab.GetComponent<IceNinjutsu>().freezeTime = _iceFreezeTime;
            prefab.GetComponent<IceNinjutsu>().direction = dir;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Check if the player is grounded
    private bool IsGrounded()
    {
        return _groundCheck.IsTouchingLayers(_groundLayer);
    }

    // Flip the character
    private void Flip()
    {
        if (_isFacingRight && _horizontal < 0f || !_isFacingRight && _horizontal > 0f)
        {
            _isFacingRight = !_isFacingRight;
            _shotDir = new Vector2(-_shotDir.x, _shotDir.y);
            _spriteRenderer.flipX = !_spriteRenderer.flipX;
        }
    }

    // Heal the player
    public void Heal(bool isHealthPotion, int heal)
    {
        if (isHealthPotion)
        {
            _health += heal;
            if (_health > _maxHealth)
            {
                _health = _maxHealth;
            }
        }
        else
        {
            _ki += heal;
            if (_ki > _maxKi)
            {
                _ki = _maxKi;
            }
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Respawn"))
        {
            Respawn();
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            Knockback();
            TakeDamage(1);
        }
    }

    private void Knockback()
    {
        _isKnockedBack = true;
        _rigidbody.AddForce(new Vector2(_isFacingRight? -5f: 5f, 5f), ForceMode2D.Impulse);
        StartCoroutine(StopKnockback());
    }
    
    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        _isKnockedBack = false;
    }

    private void Respawn()
    {
        transform.position = _respawnPoint;
        _health--;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        Invincible();
    }

    private void Invincible()
    {
        _playerCollider.enabled = false;
        _spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        StartCoroutine(Flash());
    }

    IEnumerator Flash()
    {
        for (int i = 0; i < 10; i++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
        _playerCollider.enabled = true;
        _spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    public void AddExp()
    {
        _exp++;
    }

    public bool GetIsLeveledUp()
    {
        return _isLeveledUp;
    }
}