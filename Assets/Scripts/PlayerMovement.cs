using System;
using System.Collections;
using UnityEngine;

/*
 * TODO:
 * 1. Add health system
 * 2. Add sword attack
 *      i. Collision system
 * 3. Add ninjutsu attack
 * 4. Add potion
 * 5. Add enemy
 * 6. Level design
 *      i. Add finish line
 *      ii. Add instructions
 */

public class PlayerMovement : MonoBehaviour
{
    // Character Variables
    public int health;
    public int maxHealth = 3;
    public int ki;
    public int maxKi = 3;
    
    // Movement variables
    private float horizontal;
    private float speed = 8f;
    [SerializeField] private float jumpingPower = 6f;
    private int numOfJump = 0;
    public int maxNumOfJump = 2;
    private bool isFacingRight = true;
    
    // Projectile Variables
    private bool shotCooldown;
    private float projectileFireRate = 1.5f;
    Vector2 shotDir = Vector2.right; // Sets the direction to fire bullet
    
    // Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CapsuleCollider2D groundCheck;
    private Animator anim;
    private LayerMask groundLayer;
    [SerializeField] private GameObject projectilePrefab;
    
    void Start()
    {
        // Initialize Variables
        health = maxHealth;
        ki = maxKi;
        groundLayer = LayerMask.GetMask("Ground");
        
        // Get Components
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
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
        
        // Fire Projectile
        if (Input.GetButtonDown("Fire1") && !shotCooldown && ki > 0)
        {
            StartCoroutine(fireProjectile());
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
}