using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Desks
{
    public Transform deskPosition;
    public bool isFacingUp;
}

public class SpawnManager : MonoBehaviour
{
    //Vai usar grid? trocar de transform pra grid
    [Header("Posições nas mesas")]
    [SerializeField] private Desks[] deskPositions;
    private bool[] _isPositionFree;
    
    [Header("Prefab")]
    [SerializeField] private GameObject clientPrefab;
    
    private float _dayTimer;
    private GameManager gm;

    private void Start()
    {
        _isPositionFree = new bool[deskPositions.Length];

        for (int i = 0; i < deskPositions.Length; i++)
        {
            _isPositionFree[i] = true;
        }
        
        gm = GameManager.Instance;

        _dayTimer = gm.GetDayTimer();

        StartCoroutine(SpawnClient());

    }

    private IEnumerator SpawnClient()
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
                    GenerateClientItem(positionIndex);
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
        for (int i = 0; i < deskPositions.Length; i++)
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
    
    private void GenerateClientItem(int positionIndex)
    {
        GameObject newClient = Instantiate(clientPrefab, transform.position, Quaternion.identity);
        
        ClientController clientController = newClient.GetComponent<ClientController>();
        
        clientController.Initialize(this, gm.GetRandomClient(), gm.GetRandomItem(), deskPositions[positionIndex].deskPosition, deskPositions[positionIndex].isFacingUp, positionIndex);
    }
}
