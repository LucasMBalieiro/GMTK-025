using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kitchen : InteractableStation
{
    protected override void Interact()
    {
        Debug.Log("Interaction with kitchen: grab order");
    }
    
    
}
