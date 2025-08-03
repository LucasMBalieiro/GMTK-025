using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ClientData", menuName = "ScriptableObjects/ClientData")]
public class ClientData : ScriptableObject
{
    public RuntimeAnimatorController clientAnimator;
    public Sprite sittingUp;
    public Sprite sittingDown;
    [Range(5f, 20f)] public float awardingXp;
}
