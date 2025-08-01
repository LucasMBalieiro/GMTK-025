using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportController : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform orbitTransform;
    [SerializeField] private float orbitSpeed;
    [SerializeField] private bool clockwiseDirection;
    [SerializeField] private float orbitDistance;
    
    private Vector3 _axis;
    
    void Start()
    {
        _axis = clockwiseDirection ? Vector3.back : Vector3.forward;
        transform.position = new Vector3(0f, orbitDistance, 0f);
    }
    
    void Update()
    {
        RotateAround();
    }
    
    //CODIGO REPETIDO AAAAAAAA
    private void RotateAround()
    {
        transform.RotateAround(playerTransform.position, _axis, orbitSpeed * Time.deltaTime);
    }

    public void Teleport()
    {
        playerTransform.position = transform.position;
    }
}
