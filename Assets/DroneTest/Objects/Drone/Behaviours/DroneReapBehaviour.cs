using UnityEngine;

public class DroneReapBehaviour : DroneBehaviour
{
    private float manipulationDuration;
    public DroneReapBehaviour(Drone drone, float _manipulationDuration) : base(drone)
    {
        manipulationDuration = _manipulationDuration;
    }

    public override void Enter()
    {
        if (drone.targetMaterial.DroneReaping(drone, manipulationDuration))
        { drone.targetMaterial.eMaterialReaped += EndReapMaterial; return; }

        drone.Reaload();
    }

    public override void Exit()
    {
        drone.targetMaterial.eMaterialReaped -= EndReapMaterial;
        drone.targetMaterial = null;
    }

    private void EndReapMaterial(MaterialData materialData)
    {
        drone.materialCargo = materialData;
        drone.SetBehaviour<DroneReturnBehaviour>();
    }
}
