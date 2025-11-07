using UnityEngine;

[CreateAssetMenu(fileName = "DroneData", menuName = "Gameplay/DroneData")]
public class DroneData : ScriptableObject
{
    [SerializeField] private float _droneSpeed;
    [SerializeField] private float _reapDistance;
    [SerializeField] private float _manipulationDuration;

    public float droneSpeed => _droneSpeed;
    public float reapDistance => _reapDistance;
    public float manipulationDuration => _manipulationDuration;
}
