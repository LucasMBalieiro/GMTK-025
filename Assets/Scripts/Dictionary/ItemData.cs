using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    [MinMaxSlider(1f, 30f)] public Vector2 cookingTime ;
    public Sprite itemSprite;
}
