using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableStation : MonoBehaviour
{
    [SerializeField] private KeyCode interactionKey;
    protected bool _canInteract = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        _canInteract = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        _canInteract = false;
    }

    private void Update()
    {
        if (!Input.GetKeyUp(interactionKey))
            return;
        if (!_canInteract)
            return;

        Interact();
    }

    protected abstract void Interact();
}
