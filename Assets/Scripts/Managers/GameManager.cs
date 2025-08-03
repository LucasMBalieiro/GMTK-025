using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField, ReadOnly] private int currentDay;
    
    [SerializeField] private float dayDuration;
    [ProgressBar("Timer", nameof(dayDuration), EColor.Green)]
    [ReadOnly] public float dayTimer;
    
    private bool _dayStarted;
    
    [Header("Sprites")]
    [SerializeField] private List<ClientData> clients;
    [SerializeField] private List<ItemData> items;
    
    private Dictionary<int, ClientData> clientDictionary;
    private Dictionary<int, ItemData> itemDictionary;
    
    public event Action OnEndDay;
    
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
        currentDay = 0;
    }

    public float GetDayTimer()
    {
        return dayTimer;
    }

    private void Update()
    {
        if  (!_dayStarted) return;
        
        if (dayTimer > 0)
            dayTimer -= Time.deltaTime;
        else
        {
            Debug.Log("Dia acabou");
            dayTimer = 0;
            EndDay();
        }
    }

    public void StartDay()
    {
        dayTimer = dayDuration;
        _dayStarted = true;
    }
    public void EndDay()
    {
        _dayStarted = false;
        currentDay++;
        OnEndDay?.Invoke();
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

    [Button("End Day")]
    public void EndGameButton()
    {
        EndDay();
    }
}
