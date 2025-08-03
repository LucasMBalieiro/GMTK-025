using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerMovement : MonoBehaviour
{
    
    [Foldout("Movement Parameters"), SerializeField] private Transform pivot;
    [Foldout("Movement Parameters"), SerializeField] private float walkDistance, walkDuration;
    
    [FormerlySerializedAs("_movementAnimator")] [SerializeField] private Animator movementAnimator;
    private readonly int _walkX = Animator.StringToHash("WalkX");
    private readonly int _walkY = Animator.StringToHash("WalkY");
    
    private Tween _walkTween;
    private Vector2 _lastSafePosition;
    
    private void Update()
    {
        if (_walkTween is not null && _walkTween.active)
            return;
        
        var directionWalked = Vector2.zero;
        directionWalked.x = Input.GetAxisRaw("Horizontal");
        directionWalked.y = Input.GetAxisRaw("Vertical");

        if (Mathf.Abs(directionWalked.x - directionWalked.y) == 0)
        {
            movementAnimator.SetFloat(_walkX, 0);
            movementAnimator.SetFloat(_walkY, 0);
            return;
        }

        if (Mathf.Abs(directionWalked.x) > Mathf.Abs(directionWalked.y))
        {
            var isRight = directionWalked.x > 0;
            pivot.position = new Vector3(pivot.position.x + ((isRight ? 1 : -1) * walkDistance), pivot.position.y, pivot.position.z);
            
            movementAnimator.SetFloat(_walkX, isRight ? 1 : -1);
            movementAnimator.SetFloat(_walkY, 0);
        }
        else
        {
            var isUp = directionWalked.y > 0;
            pivot.position = new Vector3(pivot.position.x, pivot.position.y + ((isUp ? 1 : -1) * walkDistance),
                pivot.position.z);
            
            movementAnimator.SetFloat(_walkY, isUp ? 1 : -1);
            movementAnimator.SetFloat(_walkX, 0);
        }
        
        _walkTween = transform.DOMove(pivot.position, walkDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                pivot.position = transform.position;
                _lastSafePosition = transform.position;
            });
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        _walkTween.Kill();
        pivot.position = _lastSafePosition;
        _walkTween = transform.DOMove(pivot.position, walkDuration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                pivot.position = transform.position;
                _lastSafePosition = transform.position;
            });
    }
}
