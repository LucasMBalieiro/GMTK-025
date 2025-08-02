using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : InteractableStation
{
    protected override void Interact()
    {
        Debug.Log("Interaction with table: serving");
    }
}
