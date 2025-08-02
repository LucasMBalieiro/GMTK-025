using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class ClientTable
{
    public int slotIndex;
    public ItemData itemData;
    public ClientController clientController;

    public ClientTable(int slotIndex, ItemData itemData, ClientController clientController)
    {
        this.slotIndex = slotIndex;
        this.itemData = itemData;
        this.clientController = clientController;
    }
}

public class Table : InteractableStation
{
    private Dictionary<int, ClientTable> orders = new Dictionary<int, ClientTable>();
    
    private bool wasAtended = false;
    public static readonly UnityEvent<List<ItemData>> OnOrderMade = new();
    
    protected override void Interact()
    {
        if (!wasAtended)
        {
            wasAtended = true;
            
        }
    }

    public void AddItem(int slotIndex, ItemData item, ClientController controller)
    {
        ClientTable clientTable = new ClientTable(slotIndex, item, controller);
        orders.Add(slotIndex, clientTable);
    }

    private void CheckItems()
    {
        orders.Remove(1);
    }
}
