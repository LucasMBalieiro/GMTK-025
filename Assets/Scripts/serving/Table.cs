using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    private Dictionary<int, ClientController> clients = new Dictionary<int, ClientController>();

    private bool isEnabled = false;
    private bool wasAtended = false;
    public static readonly UnityEvent<List<ItemData>> OnOrderMade = new();
    
    private SpawnManager _spawnManager;
    private int _tableIndex;
    
    protected override void Interact()
    {
        if (!isEnabled) return;
        
        if (!wasAtended)
        {
            List<ItemData> itemsList = new List<ItemData>();
            foreach (ClientController client in clients.Values)
            {
                itemsList.Add(client.GetCurrentWantedItem());
                client.ShowItem();
            }
            OnOrderMade?.Invoke(itemsList);
            wasAtended = true;
        }
        else
        {
            if (clients.Count <= 0) return;

            var serverSlot = PlayerSlot.Instance;
            var slot = serverSlot.GetSlot();
            if (slot is null) return;
            
            ClientController targetClient = clients.Values.FirstOrDefault(c => c.GetCurrentWantedItem() == slot.Data);
            if (targetClient == null) return;

            serverSlot.RemoveOrderFromSlot();
            targetClient.CompleteCurrentItem();
            
            ItemData nextItemToOrder = targetClient.GetCurrentWantedItem();
            
            if (nextItemToOrder)
            {
                OnOrderMade?.Invoke(new List<ItemData> { nextItemToOrder });
            }

            if (clients.Values.All(c => c.IsFinished())) { ClearTable(); }
        }
    }

    public void EnableTable() {if (!isEnabled) isEnabled = true;}

    private void ClearTable()
    {
        wasAtended = false;
        isEnabled = false;
        _spawnManager.FreePosition(_tableIndex);
        
        foreach (ClientController client in clients.Values)
        {
            client.DeleteClient();
        }
        
        clients.Clear();
    }

    public void AddClient(int slotIndex, ClientController controller, SpawnManager spawnManager, int tableIndex)
    {
        if (_spawnManager == null)
        {
            _spawnManager = spawnManager;
            _tableIndex = tableIndex;
        }
        clients.Add(slotIndex, controller);
    }

}
