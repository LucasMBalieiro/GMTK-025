using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderListUI : MonoBehaviour
{
    [SerializeField] private OrderUI orderUiPrefab;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Transform instanceRoot;

    private Dictionary<OrderEntity, OrderUI> _uiDictionary;

    private void Awake()
    {
        _uiDictionary = new Dictionary<OrderEntity, OrderUI>();
    }

    private void Start()
    {
        Kitchen.OnOrderQueued.AddListener(AddOrder);
        Kitchen.OnOrderUpdated.AddListener(UpdateOrder);
        Kitchen.OnOrderTaken.AddListener(RemoveOrder);
        
        DrinkTable.OnOrderTaken.AddListener(RemoveOrder);
    }
    
    private void AddOrder(OrderEntity order)
    {
        var newOrderUi = Instantiate(orderUiPrefab, instanceRoot);

        newOrderUi.InitUI(order.Data);
        _uiDictionary.Add(order, newOrderUi);

        scrollRect.verticalNormalizedPosition = 1f;
    }
    
    private void UpdateOrder(OrderEntity order, float cookTime)
    {
        _uiDictionary[order].StartProgress(cookTime);
    }
    
    private void RemoveOrder(OrderEntity order)
    {
        var orderRef = _uiDictionary[order];

        _uiDictionary.Remove(order);
        orderRef.gameObject.SetActive(false);
        Destroy(orderRef.gameObject, .1f);
    }
    
}
