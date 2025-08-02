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

    private bool isEnabled = false;
    private bool wasAtended = false;
    public static readonly UnityEvent<List<ItemData>> OnOrderMade = new();
    
    private List<ClientController> _clientControllers = new();
    
    protected override void Interact()
    {
        if (!isEnabled) return;
        
        if (!wasAtended)
        {
            List<ItemData> itemsList = new List<ItemData>();
            foreach (ClientTable clientOrder in orders.Values)
            {
                itemsList.Add(clientOrder.itemData);
            }

            foreach (ClientController clientController in _clientControllers)
            {
                clientController.ShowItem();
            }
            OnOrderMade?.Invoke(itemsList);
            wasAtended = true;
        }
        else
        {
            
        }
    }

    public void EnableTable()
    {
        if (!isEnabled)
        {
            isEnabled = true;
        }
    }

    public void AddItem(int slotIndex, ItemData item, ClientController controller)
    {
        ClientTable clientTable = new ClientTable(slotIndex, item, controller);
        orders.Add(slotIndex, clientTable);
        _clientControllers.Add(controller);
    }

}
