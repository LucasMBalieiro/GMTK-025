using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    //Vai usar grid? trocar de transform pra grid
    [Header("Posições nas mesas")]
    [SerializeField] private GameObject[] deskGameObjects;
    private bool[] _isPositionFree;
    
    [Header("Prefab")]
    [SerializeField] private GameObject clientPrefab;

    [Header("Random intervals")] 
    [SerializeField, MinMaxSlider(2f, 20f)]
    private Vector2 waitSpawnInterval;

    private float _dayTimer;
    private GameManager gm;

    private void Start()
    {
        _isPositionFree = new bool[deskGameObjects.Length];

        for (int i = 0; i < deskGameObjects.Length; i++)
        {
            _isPositionFree[i] = true;
        }
        
        gm = GameManager.Instance;
        gm.StartDay();

        StartCoroutine(CheckAvailableTables());
    }

    private IEnumerator CheckAvailableTables()
    {
        var positionIndex = CheckIfPositionIsFree();
        if (positionIndex != -1)
        {
            var waitTime = Random.Range(1f, 3f);
            yield return new WaitForSeconds(waitTime);
            SetupTable(deskGameObjects[positionIndex], positionIndex);
        }
        
        while (true)
        {
            if (gm.GetDayTimer() > gm.GetDayTimer() / 5f)
            {
                positionIndex = CheckIfPositionIsFree();

                if (positionIndex != -1)
                {
                    var waitTime = Random.Range(waitSpawnInterval.x, waitSpawnInterval.y);
                    yield return new WaitForSeconds(waitTime);
                    SetupTable(deskGameObjects[positionIndex], positionIndex);
                }
            }
            
            yield return null;
        }
    }


    private int CheckIfPositionIsFree()
    {
        // Generate index sequence
        var idxTable = new int[deskGameObjects.Length];
        for (var i = 0; i < deskGameObjects.Length; i++)
        {
            idxTable[i] = i;
        }

        // Shuffle
        var n = idxTable.Length;
        while (n > 1)
        {
            var k = Random.Range(0, n--); // índice aleatório entre 0 e n-1
            (idxTable[n], idxTable[k]) = (idxTable[k], idxTable[n]);
        }

        // Runs through the shuffled tables
        for (var i = 0; i < deskGameObjects.Length; i++)
        {
            var curIdx = idxTable[i];
            if (curIdx >= 0 && _isPositionFree[curIdx])
            {
                _isPositionFree[curIdx] = false;
                return curIdx;
            }

            idxTable[i] = -1;
        }

        return -1;
    }

    public void FreePosition(int positionIndex)
    {
        _isPositionFree[positionIndex] = true;
    }

    private void SetupTable(GameObject table, int tableIndex)
    {
        int numClients = UnityEngine.Random.Range(1, table.transform.childCount+1);

        for (int i = 0; i < numClients; i++)
        {
            bool isFacingUp = (i % 2 == 0);
            
            GenerateClientItem(table, table.transform.GetChild(i).transform, isFacingUp, tableIndex, i);
        }
    }
    
    private void GenerateClientItem(GameObject table, Transform seatPosition, bool isFacingUp, int tableIndex, int slotIndex)
    {
        GameObject newClient = Instantiate(clientPrefab, transform.position, Quaternion.identity);
        
        ClientController clientController = newClient.GetComponent<ClientController>();
        
        clientController.Initialize(this, gm.GetRandomClient(), gm.GetRandomItem(), seatPosition, isFacingUp, table, tableIndex, slotIndex);
    }
}
