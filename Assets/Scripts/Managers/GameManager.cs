using System;
using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private int currentDay;
    [SerializeField] private float dayTimer;
    
    [Header("Sprites")]
    [SerializeField] private List<ClientData> clients;
    [SerializeField] private List<ItemData> items;
    
    private Dictionary<int, ClientData> clientDictionary;
    private Dictionary<int, ItemData> itemDictionary;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        PopulateCharacterDictionary();
        PopulateItemDictionary();
    }

    public float GetDayTimer()
    {
        return dayTimer;
    }

    public void IncrementDay()
    {
        currentDay++;
    }

    
    ////////// DICIONARIOS //////////
    private void PopulateCharacterDictionary()
    {
        clientDictionary = new Dictionary<int, ClientData>();
        for (int i = 0; i < clients.Count; i++)
        {
            clientDictionary.Add(i, clients[i]);
        }
    }

    private void PopulateItemDictionary()
    {
        itemDictionary = new Dictionary<int, ItemData>();
        for (int i = 0; i < items.Count; i++)
        {
            itemDictionary.Add(i, items[i]);
        }
    }

    public ClientData GetRandomClient()
    {
        return clientDictionary[UnityEngine.Random.Range(0, clientDictionary.Count)];
    }

    public ItemData GetRandomItem()
    {
        return itemDictionary[UnityEngine.Random.Range(0, itemDictionary.Count)];
    }

    [Button("Simulate Order")]
    public void SimulateOrder()
    {
        var orderSize = Random.Range(1, 4);
        var order = new List<ItemData>();

        for (var i = 0; i < orderSize; i++)
        {
            var item = GetRandomItem();
            order.Add(item);
        }
        
        Table.OnOrderMade?.Invoke(order);
    }
}
