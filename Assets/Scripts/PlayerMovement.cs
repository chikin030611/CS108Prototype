using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * TODO:
 * 1. Add health system √
 * 2. Add sword attack √
 *      i. Collision system 
 * 3. Add ninjutsu attack √
 *     i. Fire √
 *    ii. Ice √
 * 4. Add potion √
 * 5. Add enemy 
 *    i. Health system 
 *   ii. Attack system *****
 *       a. Collision Checking 
 * 6. Level design 
 *      i. Add finish line 
 *      ii. Add instructions (UI) 
 */

public class PlayerMovement : MonoBehaviour
{
    // Character Variables
    private int health;
    private int maxHealth = 3;
    private int ki;
    private int maxKi = 3;
    
    // Movement variables
    private float horizontal;
    private float speed = 8f;
    [SerializeField] private float jumpingPower = 6f;
    private int numOfJump = 0;
    public int maxNumOfJump = 2;
    private bool isFacingRight = true;
    
    // Sword Attack Variables
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private GameObject swordCollision;
    
    // Ninjutsu Attack Variables
    [SerializeField] private GameObject FireNinjutsu;
    [SerializeField] private GameObject IceNinjutsu;
    
    // Projectile Variables
    private bool shotCooldown;
    private float projectileFireRate = 1.5f;
    Vector2 shotDir = Vector2.right; // Sets the direction to fire bullet
    
    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private BoxCollider2D playerCollider;
    private CapsuleCollider2D groundCheck;
    private Animator anim;
    private LayerMask groundLayer;
    [SerializeField] private GameObject projectilePrefab;
    
    public int GetHealth() { return health; }
    public int GetMaxHealth() { return maxHealth; }
    public int GetKi() { return ki; }
    public int GetMaxKi() { return maxKi; }
    
    public void SetHealth(int health) { this.health = health; }
    public void SetMaxHealth(int maxHealth) { this.maxHealth = maxHealth; }
    public void SetKi(int ki) { this.ki = ki; }
    public void SetMaxKi(int maxKi) { this.maxKi = maxKi; }
    
    // Debug Method
    public Dictionary<String, String> DebugDictionary()
    {
        Dictionary<String, String> debug = new Dictionary<String, String>();
        debug.Add("Health", health.ToString());
        debug.Add("Max Health", maxHealth.ToString());
        debug.Add("Ki", ki.ToString());
        debug.Add("Max Ki", maxKi.ToString());
        debug.Add("Number of Jump", numOfJump.ToString());
        debug.Add("Is Facing Right", isFacingRight.ToString());
        debug.Add("Shot Cooldown", shotCooldown.ToString());
        debug.Add("Shot Direction", shotDir.ToString());
        debug.Add("Is Grounded", IsGrounded().ToString());
        debug.Add("Colliding with", CheckCollision()[0]);
        
        return debug;
    }
    
    // Checking what the player is colliding with
    private string[] CheckCollision()
    {
        // Get the playerCollider's bounds
        Bounds playerBounds = playerCollider.bounds;

        // Get all colliders that overlap with the playerCollider
        Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(playerBounds.center, playerBounds.size, 0f);

        // Create a list to store the names of the GameObjects
        List<string> gameObjectNames = new List<string>();

        // Iterate over the overlapping colliders
        foreach (Collider2D collider in overlappingColliders)
        {
            // If the collider is not the playerCollider itself
            if (collider != playerCollider)
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
        health = maxHealth;
        ki = maxKi;
        groundLayer = LayerMask.GetMask("Ground");
        
        // Get Components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<BoxCollider2D>();
        groundCheck = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();
    }
    
    private void FixedUpdate()
    {
        // Movement
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void Update()
    {
        // Input
        horizontal = Input.GetAxisRaw("Horizontal");
        anim.SetBool("isRunning", horizontal != 0);
        
        // Jump
        if (Input.GetButtonDown("Jump"))
        {
            numOfJump++;
            if (numOfJump < maxNumOfJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                anim.SetBool("isJumping", true);
            }
        }
        
        // Jump Power Control
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        
        // Reset Jumping Count
        if (IsGrounded())
        {
            numOfJump = 0;
            anim.SetBool("isJumping", false);
        } 
        else 
        {
            anim.SetBool("isJumping", true);
        }
        
        // Use Sword Attack
        if (Input.GetButtonDown("Sword"))
        {
            Instantiate(swordCollision, 
                transform.position + new Vector3(shotDir.x, shotDir.y, 0), 
                Quaternion.identity);        
        }
        
        // Fire Projectile
        if (Input.GetButtonDown("Shuriken") && !shotCooldown && ki > 0)
        {
            StartCoroutine(fireProjectile());
            ki--;
        }
        
        // Fire Ninjutsu
        if (Input.GetButtonDown("Fire") && ki > 0)
        {
            GameObject prefab = Instantiate(FireNinjutsu);
            prefab.transform.position = transform.position;
            prefab.GetComponent<Projectile>().moveDirection = shotDir;
            ki--;
        }
        
        // Ice Ninjutsu
        if (Input.GetButtonDown("Ice") && ki > 0 && IsGrounded())
        {
            StartCoroutine(IceNinjutsuPattern());

            ki--;
        }
        
        // Health System
        if (health <= 0)
        {
            Destroy(gameObject);
        }
        
        // Flip the character
        Flip(); 
    }
    
    // Fire Projectile
    IEnumerator fireProjectile()
    {
        shotCooldown = true;
        
        StartCoroutine(FireProjectilePattern(shotDir));
        StartCoroutine(FireProjectilePattern(shotDir + new Vector2(0, 0.45f)));
        StartCoroutine(FireProjectilePattern(shotDir + new Vector2(0, -0.45f)));
        
        yield return new WaitForSeconds(projectileFireRate);

        shotCooldown = false;
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
        float interval = isFacingRight? 2.2f : -2.2f;
        float x = shotDir.x + (isFacingRight? 0.7f : -0.7f);
        float y = shotDir.y + 0.5f;
        for (int i = 0; i < 3; i++)
        {
            GameObject prefab = Instantiate(IceNinjutsu,
                position + new Vector3(x + i * interval, y, 0),
                Quaternion.identity);
            prefab.GetComponent<IceNinjutsu>().direction = isFacingRight;
            yield return new WaitForSeconds(0.2f);
        }
    }

    // Check if the player is grounded
    private bool IsGrounded()
    {
        return groundCheck.IsTouchingLayers(groundLayer);
    }

    // Flip the character
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            shotDir = new Vector2(-shotDir.x, shotDir.y);
            sr.flipX = !sr.flipX;
        }
    }

    public void Heal(bool isHealthPotion, int heal)
    {
        if (isHealthPotion)
        {
            health += heal;
            if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
        else
        {
            ki += heal;
            if (ki > maxKi)
            {
                ki = maxKi;
            }
        }
    }
}