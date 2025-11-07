using UnityEngine;
using UnityEngine.AI;

public class DroneSearchBehaviour : DroneBehaviour
{
    private NavMeshAgent navMeshAgent;

    public DroneSearchBehaviour(Drone _drone, NavMeshAgent _navMeshAgent) : base(_drone)
    {
        navMeshAgent = _navMeshAgent;
    }

    public override void Update()
    {
        navMeshAgent.SetDestination(drone.targetMaterial.transform.position);
    }

    public override void Collide(Collider2D collider2D)
    {
        if (collider2D.gameObject.transform == drone.targetMaterial.transform) { StartReapMaterial(); }
    }

    private void StartReapMaterial()
    {
        if (drone.targetMaterial != null)
        {
            drone.SetBehaviour<DroneReapBehaviour>();
        }
    }
}
