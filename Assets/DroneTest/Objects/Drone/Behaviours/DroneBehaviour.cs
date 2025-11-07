using UnityEngine;

public abstract class DroneBehaviour
{
    protected readonly Drone drone;


    public DroneBehaviour(Drone _drone)
    {
        drone = _drone;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Update() { }
    public virtual void Collide(Collider2D collider2D) { }
}
