using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform pivot;
    [SerializeField] private float walkDistance, walkDuration;

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
            return;

        if (Mathf.Abs(directionWalked.x) > Mathf.Abs(directionWalked.y))
        {
            var isRight = directionWalked.x > 0;
            pivot.position = new Vector3(pivot.position.x + ((isRight ? 1 : -1) * walkDistance), pivot.position.y, pivot.position.z);
        }
        else
        {
            var isUp = directionWalked.y > 0;
            pivot.position = new Vector3(pivot.position.x, pivot.position.y + ((isUp ? 1 : -1) * walkDistance), pivot.position.z);
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
