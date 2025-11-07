using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;

public class Drone : MonoBehaviour
{
    public event Action<Drone, bool> eDroneEnabled;
    public event Action<MaterialData> eCargoSended;
    public event Action<StarMaterial> eMaterialCanceled;


    [SerializeField] private DroneData droneData;
    
    public StarMaterial targetMaterial;
    public MaterialData materialCargo;
    public Base droneBase;    
    

    private NavMeshAgent navMeshAgent;
    private SpriteRenderer spriteRenderer;
    private float manipulationDuration;

    private Dictionary<Type, DroneBehaviour> droneBehaviourMap = new Dictionary<Type, DroneBehaviour>();
    private DroneBehaviour currentBehaviour;


    public void Construct(Base _base, Color _newColor)
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        manipulationDuration = droneData.manipulationDuration;

        droneBase = _base;
        spriteRenderer.color = _newColor;

        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;

        navMeshAgent.speed = droneData.droneSpeed;
        navMeshAgent.acceleration = droneData.droneSpeed * 4f;
        navMeshAgent.stoppingDistance = droneData.reapDistance;


        AddBehaviour(new DroneWaitBehaviour(this));
        AddBehaviour(new DroneSearchBehaviour(this, navMeshAgent));
        AddBehaviour(new DroneReapBehaviour(this, manipulationDuration));
        AddBehaviour(new DroneReturnBehaviour(this, navMeshAgent));
    }

    private void OnEnable()
    {
        transform.position = Vector3.zero;
        SetBehaviour<DroneWaitBehaviour>();
        eDroneEnabled?.Invoke(this, true);
    }

    private void OnDisable()
    {
        eDroneEnabled?.Invoke(this, false);
        if (targetMaterial == null) { return; }
        eMaterialCanceled?.Invoke(targetMaterial);
    }

    private void Update()
    {
        currentBehaviour.Update();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        currentBehaviour.Collide(collision);
    }


    public void SetNewTarget(StarMaterial _targetMaterial)
    {
        targetMaterial = _targetMaterial;
        eDroneEnabled?.Invoke(this, false);

        SetBehaviour<DroneSearchBehaviour>();
    }

    public void TargetRealise()
    {
        SetBehaviour<DroneWaitBehaviour>();
        eCargoSended?.Invoke(materialCargo);
        materialCargo = null;
        targetMaterial = null;
        eDroneEnabled?.Invoke(this, true);
    }

    public void Reaload()
    {
        SetBehaviour<DroneWaitBehaviour>();
        eDroneEnabled?.Invoke(this, true);
    }
    
    public void AddBehaviour(DroneBehaviour droneBehaviour)
    {
        droneBehaviourMap.Add(droneBehaviour.GetType(), droneBehaviour);
    }

    public void SetBehaviour<T>() where T : DroneBehaviour
    {
        var type = typeof(T);
        if (currentBehaviour == null || currentBehaviour.GetType() != type) 
        {
            if (droneBehaviourMap.TryGetValue(type, out var newBehaviour))
            {

                currentBehaviour?.Exit();
                currentBehaviour = newBehaviour;
                currentBehaviour.Enter();
            }
        }
    }
}
