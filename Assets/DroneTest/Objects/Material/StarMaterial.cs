using System;
using System.Collections;
using UnityEngine;

public class StarMaterial : MonoBehaviour
{
    public Action<Drone> eMaterialOccupied;
    public Action<MaterialData> eMaterialReaped;

    [SerializeField] private MaterialData materialData;
    private Coroutine reapProcess;
    private Drone drone;
    private bool isReaped = false;


    public bool DroneReaping(Drone _droneUnit, float _manipulationTime = 1f)
    {
        if (isReaped) { return false; }
        isReaped = true;

        drone = _droneUnit;

        eMaterialOccupied?.Invoke(_droneUnit);
        drone.eMaterialCanceled += OnMaterialCanceled;

        reapProcess = StartCoroutine(cReapDelay(_manipulationTime));

        return true;
    }

    private void OnMaterialCanceled(StarMaterial material)
    {
        StopCoroutine(reapProcess);
        drone.eMaterialCanceled -= OnMaterialCanceled;
        drone = null;
        isReaped = false;
    }

    private IEnumerator cReapDelay(float _manipulationTime)
    {
        yield return new WaitForSeconds(materialData.materialCollectionTime * _manipulationTime);

        eMaterialReaped?.Invoke(materialData);
        drone.eMaterialCanceled -= OnMaterialCanceled;

        drone = null;
        isReaped = false;
        gameObject.SetActive(false);
    }
}
