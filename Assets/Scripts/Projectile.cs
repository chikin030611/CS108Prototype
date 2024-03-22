using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Vector2 moveDirection;
    public float moveSpeed = 10f;
    public float damage = 1f;
    [SerializeField] private float lifeTime = 2f;
    [SerializeField] private float interval = 0.5f;
    
    private Rigidbody2D rb;
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
    
    private void FixedUpdate()
    {
        transform.Translate( moveDirection * (moveSpeed * Time.deltaTime));
    }
    
}