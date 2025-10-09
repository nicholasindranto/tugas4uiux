using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Shop/ItemData")]
public class ScriptableObjectItem : ScriptableObject
{
    public string itemName;
    public int price;
    public Sprite itemIcon;
}
