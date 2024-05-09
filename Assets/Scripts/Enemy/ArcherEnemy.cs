using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    private Enemy _enemy;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shootingRate = 2f;
    private bool isShooting = false; 

    // Start is called before the first frame update
    void Start()
    {
        _enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_enemy.isFoundPlayer && !isShooting) // Check if isShooting is false
        {
            if (!_enemy.isFrozen)
            {
                StartCoroutine(ShootArrows());
            }
        }
    }

    IEnumerator ShootArrows()
    {
        isShooting = true; // Set isShooting to true at the start of the coroutine
        GameObject prefab = Instantiate(projectilePrefab);
        prefab.transform.position = transform.position;
        if (prefab.GetComponent<Projectile>() != null)
        {
            prefab.GetComponent<Projectile>().moveDirection = _enemy.isFacingRight ? Vector2.left : Vector2.right;
        }
        yield return new WaitForSeconds(shootingRate);
        isShooting = false; // Set isShooting to false after the coroutine has finished
    }
}