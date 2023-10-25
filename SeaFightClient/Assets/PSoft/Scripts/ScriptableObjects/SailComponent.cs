using UnityEngine;

[CreateAssetMenu(fileName = "New Sail", menuName = "Crafted/Sail")]
public class SailComponent : ScriptableObject
{
    [Header("Standard values")]
    public int id;
    public new string name;
    [Space]
    public float speed;
}