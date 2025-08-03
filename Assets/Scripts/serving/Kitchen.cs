using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class Kitchen : InteractableStation
{
    private Queue<OrderEntity> _backlogOrders, _doneOrders;
    private Coroutine _cookCoroutine;

    public static readonly UnityEvent<OrderEntity> OnOrderQueued = new();
    public static readonly UnityEvent<OrderEntity, float> OnOrderUpdated = new();
    public static readonly UnityEvent<OrderEntity> OnOrderTaken = new();
    
    private void Awake()
    {
        _backlogOrders = new Queue<OrderEntity>();
        _doneOrders = new Queue<OrderEntity>();
        
        Table.OnOrderMade.AddListener(ProcessOrder);
        _cookCoroutine = StartCoroutine(Cook());
    }

    protected override void Interact()
    {
        // Dequeue
        if (_doneOrders.Count <= 0)
            return;


        Debug.Log("Getting Order");
        var orderToTake = _doneOrders.Dequeue();
        if(PlayerSlot.Instance.AddOrderToSlot(orderToTake))
            OnOrderTaken?.Invoke(orderToTake);
    }
    
    private void ProcessOrder(List<ItemData> order)
    {
        foreach (var item in order)
        {
            var entity = new OrderEntity(item);
            _backlogOrders.Enqueue(entity);
            OnOrderQueued?.Invoke(entity);
            Debug.Log($"Added {item.itemName} to queue");
        }
    }

    private IEnumerator Cook()
    {
        while (true)
        {
            // Waits for the backlog to have items
            if (_backlogOrders.Count <= 0)
                yield return new WaitUntil(() => _backlogOrders.Count > 0);
            
            // Grabs next order
            var nextOrder = _backlogOrders.Dequeue();
            var cookingTime = Random.Range(nextOrder.Data.cookingTime.x, nextOrder.Data.cookingTime.y);

            OnOrderUpdated?.Invoke(nextOrder, cookingTime);
            //Debug.Log($"Preparing {nextOrder.Data.itemName}. It will take {cookingTime}s to be ready");
            
            // Cooks the order
            yield return new WaitForSeconds(cookingTime);
            
            // Move it to done
            _doneOrders.Enqueue(nextOrder);
            //Debug.Log($"{nextOrder.Data.itemName} ready!!");
        }
    }
    public void StopCooking()
    {
        StopCoroutine(_cookCoroutine);
    }
}
