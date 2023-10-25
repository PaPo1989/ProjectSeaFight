using UnityEngine;

[CreateAssetMenu(fileName = "New Cammpm", menuName = "Crafted/Cannon")]
public class CannonComponent : ScriptableObject
{
    [Header("Standard values")]
    public int id;
    public new string name;
    [Space]
    public int damage;
    public float range;
    public float reloadTime;
    [Space]
    [Range(0f, 100f)] public float hitchance;
    [Range(0f, 100f)] public float critchance;
}