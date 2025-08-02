using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderEntity
{
    public enum OrderStatus {WAITING, READY}
    
    public ItemData Data;
    public int progress;
    public OrderStatus Status => progress >= 100 ? OrderStatus.READY : OrderStatus.WAITING;
    
    public OrderEntity(ItemData data)
    {
        Data = data;
    }
}
