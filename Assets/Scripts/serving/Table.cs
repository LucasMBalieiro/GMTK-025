using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Table : InteractableStation
{
    public static readonly UnityEvent<List<ItemData>> OnOrderMade = new();
    
    protected override void Interact()
    {
        Debug.Log("Interaction with table: serving");
    }
}
