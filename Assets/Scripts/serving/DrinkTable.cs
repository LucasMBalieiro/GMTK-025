using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

public class DrinkTable : InteractableStation
{
    private Queue<OrderEntity> _drinkBacklog;
    private Coroutine _prepareCoroutine;

    public static readonly UnityEvent<OrderEntity> OnOrderTaken = new();

    private void Awake()
    {
        _drinkBacklog = new Queue<OrderEntity>();
        Table.OnOrderMade.AddListener(ProcessOrder);
        _prepareCoroutine = StartCoroutine(PrepareDrinks());
    }

    protected override void Interact()
    {
        if(!PlayerSlot.Instance.PlayerSlotIsFree()) return;
        
        var orderToTake = OrderManager.Instance.TryTakeCompletedOrder(isBeverage: true);
        if (orderToTake == null) return;

        if (PlayerSlot.Instance.AddOrderToSlot(orderToTake))
        {
            OnOrderTaken?.Invoke(orderToTake);
        }
    }

    private void ProcessOrder(List<ItemData> order)
    {
        foreach (var item in order)
        {
            if (item.isBeverage)
            {
                var entity = new OrderEntity(item);
                _drinkBacklog.Enqueue(entity);
                
                Kitchen.OnOrderQueued?.Invoke(entity);
            }
        }
    }
    
    private IEnumerator PrepareDrinks()
    {
        while (true)
        {
            yield return new WaitUntil(() => _drinkBacklog.Count > 0);

            var nextDrink = _drinkBacklog.Dequeue();
            var prepTime = Random.Range(nextDrink.Data.cookingTime.x, nextDrink.Data.cookingTime.y);

            Kitchen.OnOrderUpdated?.Invoke(nextDrink, prepTime);
            
            yield return new WaitForSeconds(prepTime);
            
            OrderManager.Instance.AddCompletedOrder(nextDrink);
        }
    }
}