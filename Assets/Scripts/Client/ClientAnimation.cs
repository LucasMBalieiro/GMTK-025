using System;
using System.Collections;
using UnityEngine;

public class ClientAnimation : MonoBehaviour
{
    public event Action OnAnimationEnd;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _moveSpeed;
    private Transform _deskTransform;
    private ClientData _clientData;
    private bool _isFacingUp;

    public void Initialize(ClientData clientData, Transform deskTransform, bool isFacingUp)
    {
        _clientData = clientData;
        _deskTransform = deskTransform;
        _isFacingUp = isFacingUp;
        
        StartCoroutine(GoToDeskPosition());
    }

    private IEnumerator GoToDeskPosition()
    {
        _spriteRenderer.sprite = _clientData.walkUp[0]; //ta aqui so de teste
        
        while (Vector3.Distance(transform.position, _deskTransform.position) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position,_deskTransform.position,_moveSpeed * Time.deltaTime);
            
            //Esperar ter algum sprite pra fazer as animaçoes
            
            yield return null;
        }
        
        transform.position = _deskTransform.position;

        _spriteRenderer.sprite = _isFacingUp ? _clientData.sittingUp : _clientData.sittingDown;
        
        OnAnimationEnd?.Invoke();
    }
}
