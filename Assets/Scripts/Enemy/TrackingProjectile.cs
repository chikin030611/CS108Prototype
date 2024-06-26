using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class TrackingProjectile : MonoBehaviour
{
    public float moveSpeed = 10f;
    [SerializeField] private float lifeTime = 60f;
    [SerializeField] private float interval = 0.5f;

    private Rigidbody2D _rb;
    private GameObject _player;
    private GameObject _childObject;
    private SpriteRenderer _spriteRenderer;
    
    private GameObject _trackingArrow;
    [SerializeField] private GameObject _line;
    private LineRenderer _lineRenderer;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");
        _childObject = transform.GetChild(0).gameObject;
        _spriteRenderer = _childObject.GetComponent<SpriteRenderer>();

        _line = Instantiate(_line);
        _lineRenderer = _line.GetComponent<LineRenderer>();
        
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        if (_player != null)
        {
            Vector2 direction = (_player.transform.position - transform.position).normalized;
            transform.Translate(direction * (moveSpeed * Time.deltaTime));

            // Calculate the angle in radians
            float angle = Mathf.Atan2(direction.y, direction.x);

            // Convert to degrees
            angle *= Mathf.Rad2Deg;

            // Rotate the sprite
            _childObject.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Update the positions of the LineRenderer
            _lineRenderer.SetPosition(0, transform.position);
            _lineRenderer.SetPosition(1, transform.position);
            if (Vector2.Distance(_player.transform.position, transform.position) < 22)
            {
                _lineRenderer.SetPosition(1, _player.transform.position);
            }
            
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerControls>().TakeDamage(1);
            Destroy(_line);
            Destroy(gameObject);
        } 
        if (other.gameObject.CompareTag("Shuriken") || other.gameObject.CompareTag("Sword") || other.gameObject.CompareTag("Ninjutsu"))
        {
            Destroy(_line);
            Destroy(gameObject);
        }
    }
}