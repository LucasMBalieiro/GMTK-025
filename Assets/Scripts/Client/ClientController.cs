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
    private int _tableIndex;
    private int _slotIndex;
    private GameObject _table;
    
    [SerializeField] private SpriteRenderer itemSlot;
    [SerializeField] private BoxCollider2D boxCollider;

    public void Awake()
    {
        _clientAnimation = GetComponent<ClientAnimation>();
        _clientAnimation.OnAnimationEnd += ShowItem;
        boxCollider.enabled = false;
    }

    public void Initialize(SpawnManager spawnManager, ClientData clientData, ItemData itemData, Transform seatPosition, bool isFacingUp, GameObject table, int tableIndex, int slotIndex)
    {
        _clientAnimation.Initialize(clientData, seatPosition, isFacingUp);
        
        _table = table;
        _itemData = itemData;
        _spawnManager = spawnManager;
        _tableIndex = tableIndex;
        _slotIndex = slotIndex;
    }
    
    private void ShowItem()
    {
        itemSlot.sprite = _itemData.itemSprite;
        boxCollider.enabled = true;
        
        Table table = _table.GetComponent<Table>();
        table.AddItem(_slotIndex, _itemData, this);
    }
    
    public void FinalizeClient()
    {
        _spawnManager.FreePosition(_tableIndex);
        Destroy(gameObject);
    }
}
