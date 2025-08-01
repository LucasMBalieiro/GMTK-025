using System.Collections;
using UnityEngine;


public class RotationController : MonoBehaviour
{
    
    public enum ProjectileState
    {
        Orbiting,
        Waiting,
        Launched,
        Returning
    }

    [SerializeField] private Transform orbitTarget;
    [SerializeField] private float orbitDistance;
    
    [Header("Orbit")]
    [SerializeField] private float startOrbitSpeed;
    [SerializeField] private float orbitMaxSpeed;
    [SerializeField] private float orbitAcceleration;
    [SerializeField] private float orbitSpeedTresholdToLaunch;
    
    [Header("Rotation")]
    [SerializeField] private float startRotationSpeed;
    
    [Header("Shoot")]
    [SerializeField] private float launchSpeed;
    [SerializeField] private float returnSpeed;
    [SerializeField] private float launchDistance;
    [SerializeField] private float launchAcceleration;
    [SerializeField] [Range(0.9f, 0.99f)] private float precision;
    
    
    private ProjectileState _currentState = ProjectileState.Orbiting;
    
    private float _orbitSpeed;
    private float _rotationSpeed;
    private bool _isSpinning = false;
    
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        transform.localPosition = new Vector3(orbitDistance, 0, 0);
        
        _orbitSpeed = startOrbitSpeed;
        _rotationSpeed = startRotationSpeed;
        
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_currentState == ProjectileState.Orbiting || _currentState == ProjectileState.Waiting)
        {
            HandleOrbitSpeedChange();
            RotateSelf();
            RotateAround();
        }
    }
    
    public void Shoot(Vector3 mouseWorldPosition)
    {
        if (_currentState == ProjectileState.Orbiting && _orbitSpeed > orbitSpeedTresholdToLaunch)
        {
            StartCoroutine(ShootSequence(mouseWorldPosition));
        }
    }
    
    private IEnumerator ShootSequence(Vector3 mouseWorldPosition)
    {
        _currentState = ProjectileState.Waiting;
        
        Vector3 desiredDirection = (mouseWorldPosition - orbitTarget.position);
        desiredDirection.Normalize();
        Vector3 currentDirection;
        
        // Verifica se a orbita ta alinhada
        while (true)
        {
            currentDirection = (transform.position - orbitTarget.position).normalized;
            
            if (Vector3.Dot(currentDirection, desiredDirection) > precision)
            {
                break;
            }

            yield return null;
        }
        
        _currentState = ProjectileState.Launched;
        
        Vector3 targetPosition = transform.position + desiredDirection * launchDistance; 
        
        // Atira
        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, launchSpeed * Time.deltaTime);
            yield return null;
        }
        
        _currentState = ProjectileState.Returning;
        
        // Retorna
        while (Vector3.Distance(transform.position, orbitTarget.position) > 1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, orbitTarget.position, returnSpeed * Time.deltaTime);
            yield return null;
        }
        
        transform.localPosition = new Vector3(currentDirection.x * orbitDistance, currentDirection.y * orbitDistance, currentDirection.z); 
        _currentState = ProjectileState.Orbiting;
        _orbitSpeed = startOrbitSpeed;
    }

    public void SetSpinning(bool isSpinning)
    {
        _isSpinning = isSpinning;
    }

    private void RotateSelf()
    {
        transform.Rotate(Vector3.forward, _rotationSpeed * Time.deltaTime);
    }

    private void RotateAround()
    {
        transform.RotateAround(orbitTarget.position, Vector3.back, _orbitSpeed * Time.deltaTime);
    }

    private void HandleOrbitSpeedChange()
    {
        float targetSpeed = _isSpinning ? orbitMaxSpeed : startOrbitSpeed;
        
        _orbitSpeed = Mathf.MoveTowards(_orbitSpeed, targetSpeed, orbitAcceleration * Time.deltaTime);

        if (_orbitSpeed > orbitSpeedTresholdToLaunch)
        {
            _spriteRenderer.color = new Color(255f, 0f, 0f, 255f);
        }
        else
        {
            _spriteRenderer.color = new Color(100f, 255f, 255f, 255f);
        }
    }

    public ProjectileState GetCurrentState()
    {
        return _currentState;
    }
}
