using UnityEngine;
using UnityEngine.AI;

public class DroneReturnBehaviour : DroneBehaviour
{
    private NavMeshAgent navMeshAgent;

    public DroneReturnBehaviour(Drone _drone, NavMeshAgent _navMeshAgent) : base(_drone)
    {
        navMeshAgent = _navMeshAgent;
    }

    public override void Update()
    {
        navMeshAgent.SetDestination(drone.droneBase.transform.position);
    }

    public override void Collide(Collider2D collider2D)
    {
        if (collider2D.gameObject.transform == drone.droneBase.transform)
        {
            drone.TargetRealise();
        }
    }
}
