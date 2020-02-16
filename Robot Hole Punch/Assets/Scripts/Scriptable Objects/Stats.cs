using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Entities/Stats")]
public class Stats : ScriptableObject
{
    public float health = 100f;
    public float damage = 20f;
}