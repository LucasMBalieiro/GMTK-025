using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    //Vai usar grid? trocar de transform pra grid
    [Header("Posições nas mesas")]
    [SerializeField] private GameObject[] deskGameObjects;
    private bool[] _isPositionFree;
    
    [Header("Prefab")]
    [SerializeField] private GameObject clientPrefab;
    
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

        _dayTimer = gm.GetDayTimer();

        StartCoroutine(CheckAvailableTables());

    }

    private IEnumerator CheckAvailableTables()
    {
        while (true)
        {
            if (_dayTimer > 0f)
            {
                _dayTimer -= Time.deltaTime;
                
                int positionIndex = CheckIfPositionIsFree();

                if (positionIndex != -1)
                {
                    yield return new WaitForSeconds(2f);
                    SetupTable( deskGameObjects[positionIndex], positionIndex);
                }
            }
            else
            {
                Debug.Log("Dia acabou");
                //rola algum event no gameManager pra passar pra prox fase ou algo do tipo
                yield break;
            }
            
            yield return null;
        }
    }


    private int CheckIfPositionIsFree()
    {
        for (int i = 0; i < deskGameObjects.Length; i++)
        {
            if (_isPositionFree[i])
            {
                _isPositionFree[i] = false;
                return i;
            }
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
