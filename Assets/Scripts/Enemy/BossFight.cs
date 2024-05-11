using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO:
// 1. Add victory screen
// 2. Add potions spawning
// 3. Fix enemy spawning

// level recommendation: 3-5
public class BossFight : MonoBehaviour
{
    private bool _enemyIsFrozen;
    private float _playerFreezeTime;
    private float _attackInterval = 3f;
    
    public GameObject arrow;
    public GameObject trackingArrow;
    public List<GameObject> enemiesList;
    // public GameObject shuriken;

    private Projectile _projectileScript;
    private Enemy _enemyScript;
    private GameObject _player;
    
    void Start()
    {
        _enemyScript = GetComponent<Enemy>();
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerFreezeTime = _player.GetComponent<PlayerControls>().GetIceFreezeTime();
        }
        StartCoroutine(attack());
    }

    void Update()
    {
        if (_enemyScript.IsDestroyed())
        {
            // Destroy all enemies
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject enemy in enemies)
            {
                Destroy(enemy);
            }
        }
    }

    IEnumerator attack()
    {
        while (true)
        {
            _enemyIsFrozen = _enemyScript.isFrozen;
            _attackInterval = _enemyIsFrozen? _attackInterval + _playerFreezeTime : 3f;
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            
            int randomAttack = Random.Range(0,3);
            // int randomAttack = 1;
            
            switch (randomAttack)
            {
                case 0:
                    spawnArrows();
                    break;
                case 1:
                    spawnTrackingArrows();
                    break;
                case 2:
                    if (enemyCount >= 4) break;
                    spawnEnemy();
                    break;
                case 3:
                    spawnShuriken();
                    break;
            }
            yield return new WaitForSeconds(_attackInterval);
        }
    }
        

    private void spawnArrows()
    {
        bool dir = Random.Range(0, 2) == 0;
        
        // > 0 high
        // -3 to 0 middle
        // < -3 low
        int level;
        if (_player.transform.position.y < -3)
        {
            level = 2;
        } 
        else if (_player.transform.position.y > 0)
        {
            level = 0;
        }
        else
        {
            level = 1;
        }
        
        int x = dir? -33 : 33;
        int[][] y = new int[3][];
        y[0] = new int[] {5, 2};
        y[1] = new int[] {-1, -3};
        y[2] = new int[] {-5, -6};
        
        _projectileScript = arrow.GetComponent<Projectile>();
        for (int i = y[level][0]; i >= y[level][1]; i--)
        {
            _projectileScript.moveDirection = x < 0? Vector2.right : Vector2.left;
            _projectileScript.lifeTime = 10f;
            _projectileScript.moveSpeed = 7.3f;
            Instantiate(arrow, new Vector3(x, i, 0), Quaternion.identity);
        }
    }
    
    private void spawnTrackingArrows()
    {
        Instantiate(trackingArrow, new Vector3(-33, 0, 0), Quaternion.Inverse(Quaternion.identity));
        Instantiate(trackingArrow, new Vector3(33, 0, 0), Quaternion.Inverse(Quaternion.identity));
    }
    
    private void spawnEnemy()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject enemyPrefab = enemiesList[Random.Range(0, enemiesList.Count)];
            GameObject enemyInstance = Instantiate(enemyPrefab, new Vector3(Random.Range(-20, 20), Random.Range(-5, 5), 0), Quaternion.identity);
            Enemy enemyScript = enemyInstance.GetComponent<Enemy>();
            enemyScript.SetMaxHealth(1);
            enemyScript.isFoundPlayer = true;
            enemyScript.exp = 0;
        }
    }
    
    private void spawnShuriken()
    {
        
    }

}
