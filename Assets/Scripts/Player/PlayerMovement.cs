using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private RotationController rotationController;
    private Camera _camera;
    private Rigidbody2D _rb;
    private Vector2 _input;
    
    private int _arrowKeysHeld = 0;
    private Vector3 _lastArrowDirection = Vector3.zero;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    private void Update()
    {
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _input.Normalize();

        MouseShooting();
        KeyBoardShooting();
    }

    private void FixedUpdate()
    {
        _rb.velocity = _input * movementSpeed;
    }

    //Ficou meio merda o feeling de qnd vc atira e anda ao msm tempo, fiz o de teclado dps
    private void MouseShooting()
    {
        if (Input.GetMouseButtonDown(0))
        {
            rotationController.SetSpinning(true);
        }

        if (Input.GetMouseButtonUp(0))
        {
            rotationController.SetSpinning(false);
            
            Vector3 mouseWorldPosition = _camera.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0f; 
            
            rotationController.Shoot(mouseWorldPosition);
        }
    }
    
    private void KeyBoardShooting()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))    { _arrowKeysHeld++; _lastArrowDirection = Vector3.up; }
        if (Input.GetKeyDown(KeyCode.DownArrow))  { _arrowKeysHeld++; _lastArrowDirection = Vector3.down; }
        if (Input.GetKeyDown(KeyCode.LeftArrow))  { _arrowKeysHeld++; _lastArrowDirection = Vector3.left; }
        if (Input.GetKeyDown(KeyCode.RightArrow)) { _arrowKeysHeld++; _lastArrowDirection = Vector3.right; }

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            rotationController.SetSpinning(true);
        }

        // Infelizmente esse é o jeito mais facil...
        if (Input.GetKeyUp(KeyCode.UpArrow))    _arrowKeysHeld--;
        if (Input.GetKeyUp(KeyCode.DownArrow))  _arrowKeysHeld--;
        if (Input.GetKeyUp(KeyCode.LeftArrow))  _arrowKeysHeld--;
        if (Input.GetKeyUp(KeyCode.RightArrow)) _arrowKeysHeld--;

        if (_arrowKeysHeld == 0 && (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow)))
        {
            rotationController.SetSpinning(false);
            if (_lastArrowDirection != Vector3.zero)
            {
                Vector3 targetPosition = transform.position + _lastArrowDirection * 10f; 
                rotationController.Shoot(targetPosition);
            }
        }
    }
}
