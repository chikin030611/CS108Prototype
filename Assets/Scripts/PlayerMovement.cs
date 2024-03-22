using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 10f;
    private int numOfJump = 0;
    public int maxNumOfJump = 2;
    private bool isFacingRight = true;
    private bool shotCooldown;
    private float projectileFireRate = 1.5f;
    
    Vector2 shotDir = Vector2.right; // Sets the direction to fire bullet
    
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private CapsuleCollider2D groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject projectilePrefab;
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        groundCheck = GetComponent<CapsuleCollider2D>();
    }
    
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") )
        {
            numOfJump++;
            if (numOfJump < maxNumOfJump)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
                Debug.Log("numOfJump: " + numOfJump + "\n isGrounded: " + IsGrounded());
            }
        }
        
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Fire1"))
        {
            StartCoroutine(fireProjectile());
        }
        
        if (IsGrounded())
        {
            numOfJump = 0;
        }
        
        Flip();
    }
    
    IEnumerator fireProjectile()
    {
        shotCooldown = true;

        StartCoroutine(FireProjectilePattern(shotDir));
        StartCoroutine(FireProjectilePattern(shotDir + new Vector2(0, 0.45f)));
        StartCoroutine(FireProjectilePattern(shotDir + new Vector2(0, -0.45f)));
        
        yield return new WaitForSeconds(projectileFireRate);

        shotCooldown = false;
    }
    
    IEnumerator FireProjectilePattern(Vector2 direction)
    {
        for (int j = 0; j < 3; j++)
        {
            GameObject prefab = Instantiate(projectilePrefab);
            prefab.transform.position = transform.position;
            prefab.GetComponent<Projectile>().moveDirection = direction;
            yield return new WaitForSeconds(0.2f); // Wait for 0.2 seconds between each shot
        }
    }

    private bool IsGrounded()
    {
        return groundCheck.IsTouchingLayers(groundLayer);
    }

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