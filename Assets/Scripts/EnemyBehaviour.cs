using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{

    GameManager _gameManager;
    GameObject _player;

    float _enemyHealth = 100f;
    float _enemyMoveSpeed = 2f;
    Vector2 _enemyMoveDirection;
    Quaternion _targetRotation;
    bool _disableEnemy = false;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        _player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if(!_gameManager._gameOver && !_disableEnemy) 
        {
            MoveEnemy();
            RotateEnemy();
        }
    }

    void MoveEnemy() 
    {
        gameObject.transform.position = Vector2.MoveTowards(gameObject.transform.position, _player.transform.position,
         _enemyMoveSpeed * Time.deltaTime);
    }

    void RotateEnemy() 
    {
        _enemyMoveDirection = _player.transform.position - gameObject.transform.position;
        _enemyMoveDirection.Normalize();

        _targetRotation = Quaternion.LookRotation(Vector3.forward, _enemyMoveDirection);

        //If enemy is already facing player don't rotate
        if (gameObject.transform.rotation != _targetRotation) 
        {
            gameObject.transform.rotation = Quaternion.RotateTowards(gameObject.transform.rotation, _targetRotation, 200 * Time.deltaTime);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bullet") 
        {
            StartCoroutine(Damaged());
            _enemyHealth -= 40f;

            if(_enemyHealth <= 0f)
            {
                Destroy(gameObject);
            }

            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.tag == "Player") 
        {
            _gameManager._gameOver = true;
            collision.gameObject.SetActive(false);
        }
    }

    IEnumerator Damaged() 
    {
        _disableEnemy = true;
        yield return new WaitForSeconds(0.5f);
        _disableEnemy = false;
    }

}
