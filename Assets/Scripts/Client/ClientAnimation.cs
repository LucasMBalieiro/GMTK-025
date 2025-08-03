using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ClientAnimation : MonoBehaviour
{
    public event Action OnAnimationEnd;
    
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _moveSpeed;
    
    [SerializeField] private Animator _animator;
    private readonly int moveX = Animator.StringToHash("moveX");
    private readonly int moveY = Animator.StringToHash("moveY");
    
    private Transform _deskTransform;
    private ClientData _clientData;
    private bool _isFacingUp;

    public void Initialize(ClientData clientData, Transform deskTransform, bool isFacingUp)
    {
        _clientData = clientData;
        _deskTransform = deskTransform;
        _isFacingUp = isFacingUp;

        _animator.runtimeAnimatorController = clientData.clientAnimator;
        _animator.SetFloat(moveX, 0f);
        _animator.SetFloat(moveY, 1f);
        
        StartCoroutine(GoToDeskPosition());
    }

    private IEnumerator GoToDeskPosition()
    {
        var walkingSequence = DOTween.Sequence();

        var yDistance = Math.Abs(transform.position.y - _deskTransform.position.y);
        var yDuration = yDistance switch
        {
            < 2f => 1f,
            > 5f => 3f,
            _ => 2f
        };
        walkingSequence.Append(transform.DOMoveY(_deskTransform.position.y, yDuration).SetEase(Ease.Linear));
        walkingSequence.JoinCallback(() => _animator.SetFloat(moveY, 1f));
        walkingSequence.Append(transform.DOMoveX(_deskTransform.position.x, 1f).SetEase(Ease.Linear));
        walkingSequence.JoinCallback(() =>
        {
            _animator.SetFloat(moveY, 0f);
            _animator.SetFloat(moveX, _deskTransform.position.x > 0f ? 1f : -1f);
        });

        walkingSequence.OnComplete(() =>
        {
            transform.position = _deskTransform.position;

            _animator.enabled = false;
            _spriteRenderer.sprite = _isFacingUp ? _clientData.sittingUp : _clientData.sittingDown;

            OnAnimationEnd?.Invoke();
        });
        // while (Vector3.Distance(transform.position, _deskTransform.position) > 0.05f)
        // {
        //     transform.position = Vector3.MoveTowards(transform.position,_deskTransform.position,_moveSpeed * Time.deltaTime);
        //     
        //     //Esperar ter algum sprite pra fazer as animaçoes
        //     
        //     yield return null;
        // }

        yield break;
    }
}
