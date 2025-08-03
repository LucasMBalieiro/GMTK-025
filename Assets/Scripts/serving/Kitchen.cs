using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Kitchen : InteractableStation
{
    private Queue<OrderEntity> _backlogOrders;
    private Coroutine _cookCoroutine;

    public static readonly UnityEvent<OrderEntity> OnOrderQueued = new();
    public static readonly UnityEvent<OrderEntity, float> OnOrderUpdated = new();
    public static readonly UnityEvent<OrderEntity> OnOrderTaken = new();
    
    private void Awake()
    {
        _backlogOrders = new Queue<OrderEntity>();
        Table.OnOrderMade.AddListener(ProcessOrder);
        _cookCoroutine = StartCoroutine(Cook());
    }

    protected override void Interact()
    {
        if(!PlayerSlot.Instance.PlayerSlotIsFree()) return;
        
        var orderToTake = OrderManager.Instance.TryTakeCompletedOrder(isBeverage: false);
        if (orderToTake == null) return;

        if (PlayerSlot.Instance.AddOrderToSlot(orderToTake))
            OnOrderTaken?.Invoke(orderToTake);
    }
    
    private void ProcessOrder(List<ItemData> order)
    {
        foreach (var item in order)
        {
            if (!item.isBeverage)
            {
                var entity = new OrderEntity(item);
                _backlogOrders.Enqueue(entity);
                OnOrderQueued?.Invoke(entity);
            }
        }
    }

    private IEnumerator Cook()
    {
        while (true)
        {
            yield return new WaitUntil(() => _backlogOrders.Count > 0);
            
            var nextOrder = _backlogOrders.Dequeue();
            var cookingTime = Random.Range(nextOrder.Data.cookingTime.x, nextOrder.Data.cookingTime.y);

            OnOrderUpdated?.Invoke(nextOrder, cookingTime);
            
            yield return new WaitForSeconds(cookingTime);
            
            OrderManager.Instance.AddCompletedOrder(nextOrder);
        }
    }
    public void StopCooking()
    {
        StopCoroutine(_cookCoroutine);
    }
}
