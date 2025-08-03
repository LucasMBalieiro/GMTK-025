using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientController : MonoBehaviour
{
    private ClientAnimation _clientAnimation;
    private SpawnManager _spawnManager;
    private GameObject _table;
    
    private int _tableIndex;
    private int _slotIndex;
    
    private List<ItemData> _orderedItems = new List<ItemData>();
    private int _currentItemIndex = 0;

    [SerializeField] private SpriteRenderer bubbleSpeech;
    [SerializeField] private SpriteRenderer itemSlot;

    public void Awake()
    {
        _clientAnimation = GetComponent<ClientAnimation>();
        _clientAnimation.OnAnimationEnd += ShowOrder;
    }

    public void Initialize(SpawnManager spawnManager, ClientData clientData, List<ItemData> items, Transform seatPosition, GameObject table, int tableIndex, int slotIndex)
    {
        
        _table = table;
        _orderedItems = items;
        _spawnManager = spawnManager;
        _tableIndex = tableIndex;
        _slotIndex = slotIndex;
        
        
        _clientAnimation.Initialize(clientData, seatPosition, (_slotIndex % 2 == 0));
    }
    
    private void ShowOrder()
    {
        if (_slotIndex < 2)
        {
            bubbleSpeech.gameObject.transform.position = bubbleSpeech.gameObject.transform.position - new Vector3(1.5f,0f,0f);
            bubbleSpeech.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        bubbleSpeech.gameObject.SetActive(true);
        itemSlot.sprite = GameManager.Instance.GetDotsSprite();
        
        Table table = _table.GetComponent<Table>();
        table.EnableTable();
        table.AddClient(_slotIndex, this, _spawnManager, _tableIndex);
    }

    public void ShowItem()
    {
        if (_orderedItems.Count > _currentItemIndex)
        {
            itemSlot.sprite = _orderedItems[_currentItemIndex].itemSprite;
        }
        else
        {
            bubbleSpeech.gameObject.SetActive(false);
            itemSlot.sprite = null;
        }
    }
    
    public void CompleteCurrentItem()
    {
        _currentItemIndex++;
        ShowItem();
    }
    
    public ItemData GetCurrentWantedItem()
    {
        return IsFinished() ? null : _orderedItems[_currentItemIndex];
    }
    
    public bool IsFinished()
    {
        return _currentItemIndex >= _orderedItems.Count;
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
