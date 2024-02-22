using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;

    Rigidbody2D _rb;
    Camera _mainCamera;

    float _moveVertical;
    float _moveHorizontal;
    float _moveSpeed= 5f;
    float _speedLimiter= 0.7f;

    Vector2 _moveVelocity;

    Vector2 _mousePosition;
    Vector2 _offSet;

    [SerializeField] GameObject _bullet;
    [SerializeField] GameObject _bulletSpawn;

    bool _isShooting = false;
    float _bulletSpeed = 15f;



    // Start is called before the first frame update
    void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        _moveHorizontal = Input.GetAxisRaw("Horizontal");
        _moveVertical = Input.GetAxisRaw("Vertical");

        _moveVelocity = new Vector2(_moveHorizontal, _moveVertical) * _moveSpeed;

        if (Input.GetMouseButtonDown(0)) 
        {
            _isShooting = true;
        }
    }


    private void FixedUpdate() 
    {
        MovePlayer();
        RotatePlayer();    

        if (_isShooting) 
        {
            StartCoroutine(Fire());
        }
    }


    void MovePlayer() 
    {
        //Moving
        if (_moveHorizontal != 0 || _moveVertical != 0) 
        {
            if (_moveHorizontal != 0 && _moveVertical != 0) {
                _moveVelocity *= _speedLimiter;
            }
            _rb.velocity = _moveVelocity;
        }
        //Standing still
        else 
        {
            _moveVelocity = new Vector2(0f, 0f);
            _rb.velocity = _moveVelocity;
        }
    }


    void RotatePlayer() 
    {
        //Checking mouse position & player position on screen
        _mousePosition = Input.mousePosition;
        Vector3 screenPoint = _mainCamera.WorldToScreenPoint(gameObject.transform.localPosition);
        
        //Comparing player positon onscreen to mouse position
        _offSet = new Vector2(_mousePosition.x - screenPoint.x, _mousePosition.y -screenPoint.y).normalized;

        //Rotatation angle
        float angle = Mathf.Atan2(_offSet.y, _offSet.x) * Mathf.Rad2Deg;
        gameObject.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);
    }

    IEnumerator Fire() 
    {
        _isShooting = false;
         GameObject bullet = Instantiate(_bullet, _bulletSpawn.transform.position, Quaternion.identity);
        //Shoots the bullet
        bullet.GetComponent<Rigidbody2D>().velocity = _offSet * _bulletSpeed;
        yield return new WaitForSeconds(3f);
        Destroy(bullet);
    }

}
