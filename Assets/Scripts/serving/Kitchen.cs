using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Kitchen : InteractableStation
{
    private Queue<ItemData> _backlogOrders, _doneOrders;
    private Coroutine _cookCoroutine;
    
    private void Awake()
    {
        _backlogOrders = new Queue<ItemData>();
        _doneOrders = new Queue<ItemData>();
        
        Table.OnOrderMade.AddListener(ProcessOrder);
        _cookCoroutine = StartCoroutine(Cook());
    }

    protected override void Interact()
    {
        Debug.Log("Interaction with kitchen: grab order");
    }
    
    private void ProcessOrder(List<ItemData> order)
    {
        foreach (var item in order)
        {
            _backlogOrders.Enqueue(item);
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
            var cookingTime = Random.Range(nextOrder.cookingTime.x, nextOrder.cookingTime.y);

            Debug.Log($"Preparing {nextOrder.itemName}. It will take {cookingTime}s to be ready");
            
            // Cooks the order
            yield return new WaitForSeconds(cookingTime);
            
            // Move it to done
            _doneOrders.Enqueue(nextOrder);
            Debug.Log($"{nextOrder.itemName} ready!!");
        }
    }
    public void StopCooking()
    {
        StopCoroutine(_cookCoroutine);
    }
}
