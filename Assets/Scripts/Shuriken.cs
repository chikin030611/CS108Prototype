using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Shuriken : MonoBehaviour
{
    private float moveSpeed;
    public float damage = 1f;
    [SerializeField] private float rotationSpeed = 540;
    
    private CircleCollider2D shurikenCollider;
    private SpriteRenderer sr;
    private Projectile projectile;
    
    // Start is called before the first frame update
    void Start()
    {
        shurikenCollider = GetComponent<CircleCollider2D>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        projectile = GetComponent<Projectile>();
        
        moveSpeed = projectile.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        sr.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            projectile.moveSpeed = 0;
            rotationSpeed = 0;
            sr.transform.Rotate(0, 0, 0);
        }
    }
    
}
