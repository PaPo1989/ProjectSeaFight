using UnityEngine;

[CreateAssetMenu(fileName = "New Ship", menuName = "Crafted/Ship")]
public class ShipComponent : ScriptableObject
{
    [Header("Standard values")]
    public int id;
    public new string name;
    [Space]
    public Sprite icon;
    [Space]
    [Header("Ship values")]
    public float view;
    public int hp;
    [Space]
    [Header("Available ship slots")]
    public int cannonSlots;
    public int sailSlots;
    // NotImplementedException public int crewSlots;
    [Space]
    public int inventorySlots;
}