using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlot : MonoBehaviour
{
    public static PlayerSlot Instance;
    
    [SerializeField] private SpriteRenderer orderRenderer;
    
    private OrderEntity _currentOrder = null;

    private void Awake()
    {
        Instance = this;
        
        _currentOrder = null;
        orderRenderer.sprite = null;
    }

    public OrderEntity GetSlot() => _currentOrder;
    
    public bool AddOrderToSlot(OrderEntity orderToTake)
    {
        if (_currentOrder is not null)
            return false;

        _currentOrder = orderToTake;
        orderRenderer.sprite = _currentOrder.Data.itemSprite;
        orderRenderer.color = new Color(1f, 1f, 1f, 1f);
        return true;
    }

    public void RemoveOrderFromSlot()
    {
        if (_currentOrder is null)
            return;

        _currentOrder = null;
        orderRenderer.sprite = null;
        orderRenderer.color = new Color(0f, 0f, 0f, 0f);
    }
}
