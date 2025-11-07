using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Zenject.Asteroids;
using System;
using Zenject;

public class MaterialSpawner : MonoBehaviour
    {
        public event Action<StarMaterial> eMeterialSpawn;

        private StarMaterial materialPrefab;
        private Transform spawnArea;
        private Transform materialTransform;
        private ObjectPool<StarMaterial> materialPool;
        private ReactiveVariable<int> spawnPerMinute;
        private int materialLimit;


        [Inject]
        public void Construct(StarMaterial _materialPrefab, ReactiveVariable<int> _spawnPerMinute, int _materialLimit)
        {
            materialPrefab = _materialPrefab;
            materialLimit = _materialLimit;
            spawnPerMinute = _spawnPerMinute;

            spawnArea = transform;

            materialTransform = new GameObject("Material Transform").transform;
            materialTransform.SetParent(transform.parent);
            materialTransform.localPosition = Vector3.zero;
            materialTransform.localScale = Vector3.one;

            materialPool = new ObjectPool<StarMaterial>(materialPrefab, materialLimit, materialTransform);
        }

    private void OnEnable()
        {
            if (materialPrefab == null) { Debug.Log("Spawner not init"); return; }
            StartCoroutine(cSpawnDelay());
        }


    public void SpawnMaterial()
        {
            var material = materialPool.GetFreeElement();
            if (material == null) { return; }

            material.gameObject.SetActive(true);
            material.transform.position = new Vector3(UnityEngine.Random.Range(spawnArea.position.x + spawnArea.localScale.x / 2, spawnArea.position.x - spawnArea.localScale.x / 2),
                                                        UnityEngine.Random.Range(spawnArea.position.y + spawnArea.localScale.y / 2, spawnArea.position.y - spawnArea.localScale.y / 2), 0f);
            eMeterialSpawn?.Invoke(material);
        }

    public IEnumerator cSpawnDelay()
    {
        while (true)
        {
            SpawnMaterial();
            yield return new WaitForSeconds(60.0f / spawnPerMinute.Value);
        }
    }
}
