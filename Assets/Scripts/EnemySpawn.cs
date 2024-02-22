using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawn : MonoBehaviour
{

    [SerializeField] GameManager _gameManager;
    [SerializeField] GameObject _enemy;
    [SerializeField] GameObject [] _spawnPoints;

    float _spawnTimer = 2f;
    float _spawnRateIncrease = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnNextEnemy());
        StartCoroutine(SpawnRateIncrease());
    }

    IEnumerator SpawnNextEnemy() 
    {
        int nextSpawnLocation = Random.Range(0, _spawnPoints.Length);

        Instantiate(_enemy, _spawnPoints[nextSpawnLocation].transform.position, Quaternion.identity);
        yield return new WaitForSeconds(_spawnTimer);

        if (!_gameManager._gameOver) 
        {
            StartCoroutine(SpawnNextEnemy());
        }
    }

    IEnumerator SpawnRateIncrease() 
    {
        yield return new WaitForSeconds(_spawnRateIncrease);

        if (_spawnTimer >= 0.5f) 
        {
            _spawnTimer -= 0.2f;
        }
        StartCoroutine(SpawnRateIncrease());
    }

}
