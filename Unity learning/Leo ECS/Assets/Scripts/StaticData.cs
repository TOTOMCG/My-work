using UnityEngine;

[CreateAssetMenu]
public class StaticData : ScriptableObject
{
    public GameObject ballprefab;
    public float ballSpeed;
    public Vector3 ballDirection;
    public Vector3 gravity;
    public Vector3 ballSpawn;
}