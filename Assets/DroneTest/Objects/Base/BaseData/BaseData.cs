using UnityEngine;

[CreateAssetMenu(fileName = "BaseData", menuName = "Gameplay/BaseData")]
public class BaseData : ScriptableObject
{
    [SerializeField] private Drone _dronePrefab;
    [SerializeField] private int _droneLimit;


    public Drone dronePrefab => _dronePrefab;
    public int droneLimit => _droneLimit;
}
