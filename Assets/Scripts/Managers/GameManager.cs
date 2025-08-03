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

    public int CurrentDay { get; private set; }
    
    [SerializeField] private float dayDuration;
    [ProgressBar("Timer", nameof(dayDuration), EColor.Green)]
    [ReadOnly] public float dayTimer;
    
    private bool _dayStarted;
    
    [Header("Sprites")]
    [SerializeField] private List<ClientData> clients;
    [SerializeField] private List<ItemData> items;
    [SerializeField] private Sprite dotsSprite;
    
    private Dictionary<int, ClientData> clientDictionary;
    private Dictionary<int, ItemData> itemDictionary;

    public float CurrentXp { get; private set; }
    public float dayXp { get; private set; }
    public int CurrentLevel { get; private set; }
    [SerializeField] public int xpThreshold;
    public JobRoles CurrentRole; 
    
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
        
        CurrentDay = 1;
        
        CurrentXp = 0f;
        dayXp = 0f;
        CurrentLevel = 1;
        CurrentRole = JobRoles.TRAINEE;
    }

    public float GetDayTimer()
    {
        return dayTimer;
    }
    public float GetDayDuration()
    {
        return dayDuration;
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
        dayXp = 0f;
        _dayStarted = true;
    }
    public void EndDay()
    {
        _dayStarted = false;
        CurrentDay += (int)CurrentRole;
        OnEndDay?.Invoke();
    }

    public bool CheckEnd()
    {
        return (CurrentRole == JobRoles.GENERAL_MANAGER && CurrentLevel > (int)JobRoles.GENERAL_MANAGER);
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

    public Sprite GetDotsSprite()
    {
        return dotsSprite;
    }

    public void AddXpToDay(float xpAmount)
    {
        dayXp += xpAmount;
    }
    public (bool leveledUp, bool promoted) GainXp()
    {
        CurrentXp += dayXp;

        if (CurrentXp < xpThreshold) 
            return (false, false);

        var promoted = false;
        CurrentXp -= xpThreshold;
        CurrentLevel++;
        if (Enum.IsDefined(typeof(JobRoles), CurrentLevel))
        {
            CurrentRole = (JobRoles)CurrentLevel;
            promoted = true;
        }
         
        return (true, promoted);
    }

    public static void DestroyInstance()
    {
        var go = Instance.gameObject;
        Instance = null;
        Destroy(go);
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
