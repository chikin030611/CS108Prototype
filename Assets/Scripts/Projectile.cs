using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 moveDirection;
    public float moveSpeed = 10f;
    public float damage = 1f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float interval = 0.5f;
    
    private Rigidbody2D _rb;
    
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        InvokeRepeating(nameof(FlipSprite), 0, interval);
        Destroy(gameObject, lifeTime);
    }
    
    private void FixedUpdate()
    {
        transform.Translate( moveDirection * (moveSpeed * Time.deltaTime));
    }
    
    private void FlipSprite()
    {
        if (moveDirection.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDirection.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    
}