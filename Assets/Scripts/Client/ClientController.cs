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

    public void Awake()
    {
        _clientAnimation = GetComponent<ClientAnimation>();
        _clientAnimation.OnAnimationEnd += ShowOrder;
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
    
    private void ShowOrder()
    {
        Table table = _table.GetComponent<Table>();
        table.EnableTable();
        table.AddItem(_slotIndex, _itemData, this, _spawnManager, _tableIndex);
    }

    public void ShowItem()
    {
        itemSlot.sprite = _itemData.itemSprite;
    }

    public void FinishOrder()
    {
        itemSlot.sprite = null; //Adicionar um check talvez?
    }
    
    public void DeleteClient()
    {
        Destroy(gameObject);
    }
}
