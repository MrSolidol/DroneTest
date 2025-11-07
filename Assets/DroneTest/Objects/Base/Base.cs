using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Base : MonoBehaviour
{
    [SerializeField] private BaseData baseData;
    [SerializeField] private Color baseColor;

    private MaterialSpawner materialSpawner;
    private ObjectPool<Drone> dronePool;
    private List<Drone> avaliableDrones = new List<Drone>();
    private List<StarMaterial> avaliableMaterials = new List<StarMaterial>();
    private ReactiveVariable<int> droneCount;
    private ReactiveVariable<int> materialCount;
    private Drone dronePrefab;
    private SpriteRenderer spriteRenderer;
    private int droneLimit;


    public void Construct(MaterialSpawner _materialSpawner, ReactiveVariable<int> _droneCount, ReactiveVariable<int> _materialCount)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = baseColor;

        materialSpawner = _materialSpawner;
        materialSpawner.eMeterialSpawn += AddTarget;

        dronePrefab = baseData.dronePrefab;
        droneLimit = baseData.droneLimit;

        droneCount = _droneCount;
        droneCount.eChanged += OnDroneCountChange;
        materialCount = _materialCount;

        Transform dronesTransform = new GameObject("Drones Transform").transform;
        dronesTransform.SetParent(transform.parent);
        dronesTransform.localPosition = Vector3.zero;
        dronesTransform.localScale = Vector3.one;

        dronePool = new ObjectPool<Drone>(dronePrefab, droneLimit, dronesTransform);

        foreach (var drone in dronePool.GetPoolList())
        {
            drone.Construct(this, baseColor);

            drone.eCargoSended += GetCargo;
            drone.eDroneEnabled += AddDrone;
            
            drone.transform.position = new Vector3(UnityEngine.Random.Range(-1f, 1f),
                                                    UnityEngine.Random.Range(-1f, 1f), 0f);
            drone.gameObject.SetActive(true);
        }
    }

    private void AddTarget(StarMaterial newMaterial)
    {
        avaliableMaterials.Add(newMaterial);
        SendDrone();
    }

    private void AddDrone(Drone drone, bool flag)
    {
        if (flag)
        {
            avaliableDrones.Add(drone);
            SendDrone();
            return;
        }
        avaliableDrones.Remove(drone);
    }

    private void SendDrone()
    {
        var freeDrone = GetFreeDrones();
        if (avaliableMaterials.Count != 0 && freeDrone != null)
        {
            StarMaterial closestMaterial = avaliableMaterials[0];
            foreach (var material in avaliableMaterials)
            {
                if (Vector3.Distance(material.transform.position, freeDrone.transform.position) < Vector3.Distance(closestMaterial.transform.position, freeDrone.transform.position))
                {
                    closestMaterial = material;
                }
            }
            avaliableMaterials.Remove(closestMaterial);
            freeDrone.SetNewTarget(closestMaterial);
        }
    }

    private void GetCargo(MaterialData materialData)
    {
        materialCount.Value += materialData.materialWeight;
    }

    private void OnDroneCountChange(int oldValue, int newValue)
    {
        ChangeDroneCount(newValue);
    }

    private void ChangeDroneCount(int value)
    {
        List<Drone> drones = dronePool.GetPoolList();

        int currentlyActive = 0;
        foreach (Drone drone in drones)
        {
            if (drone.gameObject.activeInHierarchy)
                currentlyActive++;
        }

        int delta = value - currentlyActive;
        if (delta == 0) return;

        foreach (Drone drone in drones)
        {
            if (delta > 0 && !drone.gameObject.activeInHierarchy)
            {
                drone.gameObject.SetActive(true);
                delta--;
            }
            else if (delta < 0 && drone.gameObject.activeInHierarchy)
            {
                drone.gameObject.SetActive(false);
                delta++;
            }

            if (delta == 0) break;
        }
    }
    
    public Drone GetFreeDrones()
    {
        if (avaliableDrones.Count != 0) return avaliableDrones[0];
        return null;
    }
}