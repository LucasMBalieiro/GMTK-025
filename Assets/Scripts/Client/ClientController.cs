using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    private ClientAnimation _clientAnimation;
    
    private SpawnManager _spawnManager;
    private ItemData _itemData;
    private Transform _destination;
    private int _positionIndex;
    
    [SerializeField] private SpriteRenderer itemSlot;
    [SerializeField] private BoxCollider2D boxCollider;

    public void Awake()
    {
        _clientAnimation = GetComponent<ClientAnimation>();
        _clientAnimation.OnAnimationEnd += ShowItem;
        boxCollider.enabled = false;
    }

    public void Initialize(SpawnManager spawnManager, ClientData clientData, ItemData itemData, Transform deskTransform, bool isFacingUp, int positionIndex)
    {
        _clientAnimation.Initialize(clientData, deskTransform, isFacingUp);
        
        _itemData = itemData;
        _spawnManager = spawnManager;
        _positionIndex = positionIndex;
    }
    
    private void ShowItem()
    {
        itemSlot.sprite = _itemData.itemSprite;
        boxCollider.enabled = true;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Compara se o player ta carregando o pedido certo e chama FinalizeClient
        }
    }
    
    public void FinalizeClient()
    {
        _spawnManager.FreePosition(_positionIndex);
        Destroy(gameObject);
    }
}
