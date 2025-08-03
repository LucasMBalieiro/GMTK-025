using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    private Queue<OrderEntity> _doneOrders;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            _doneOrders = new Queue<OrderEntity>();
        }
    }
    
    public void AddCompletedOrder(OrderEntity order)
    {
        _doneOrders.Enqueue(order);
    }
    
    public OrderEntity TryTakeCompletedOrder(bool isBeverage)
    {
        
        // Create a temporary queue to hold the items we want to keep.
        Queue<OrderEntity> tempQueue = new Queue<OrderEntity>();
        OrderEntity orderToTake = null;
        bool itemTaken = false;

        // Loop through the original queue until it's empty.
        while (_doneOrders.Count > 0)
        {
            var currentOrder = _doneOrders.Dequeue();

            // Check if this is the first matching item we've found.
            if (!itemTaken && currentOrder.Data.isBeverage == isBeverage)
            {
                // This is the item we want. Store it but don't add it to the temp queue.
                orderToTake = currentOrder;
                itemTaken = true; 
            }
            else
            {
                // This is not the item we're taking, so add it to our temporary queue to keep it.
                tempQueue.Enqueue(currentOrder);
            }
        }

        // The original queue is now empty. Replace it with our temporary queue,
        // which contains all the original items except for the one we took.
        _doneOrders = tempQueue;

        return orderToTake;
    }
}