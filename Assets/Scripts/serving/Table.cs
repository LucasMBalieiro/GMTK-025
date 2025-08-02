using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    
    private SpawnManager _spawnManager;
    private int _tableIndex;
    
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
            if (orders.Count <= 0) return;

            var serverSlot = PlayerSlot.Instance;
            var slot = serverSlot.GetSlot();
            if (slot is null) return;
            
            var keyToRemove = orders.First(pair => pair.Value.itemData == slot.Data).Key;
            var clientController = orders[keyToRemove].clientController;
            
            serverSlot.RemoveOrderFromSlot();
            clientController.FinishOrder();
            orders.Remove(keyToRemove);
            if (orders.Count == 0)
            {
                ClearTable();
            }
        }
    }

    public void EnableTable()
    {
        if (!isEnabled)
        {
            isEnabled = true;
        }
    }

    private void ClearTable()
    {
        wasAtended = false;
        isEnabled = false;
        _spawnManager.FreePosition(_tableIndex);
        foreach (ClientController clientController in _clientControllers)
        {
            clientController.DeleteClient();
        }
        orders.Clear();
        _clientControllers.Clear();
        
    }

    public void AddItem(int slotIndex, ItemData item, ClientController controller, SpawnManager spawnManager, int tableIndex)
    {
        if (_spawnManager == null)
        {
            _spawnManager = spawnManager;
            _tableIndex = tableIndex;
        }
        
        ClientTable clientTable = new ClientTable(slotIndex, item, controller);
        orders.Add(slotIndex, clientTable);
        _clientControllers.Add(controller);
    }

}
