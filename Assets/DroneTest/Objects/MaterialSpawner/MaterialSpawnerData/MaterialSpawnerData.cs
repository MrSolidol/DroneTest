using UnityEngine;

[CreateAssetMenu(fileName = "MaterialSpawnerData", menuName = "Gameplay/MaterialSpawnerData")]
public class MaterialSpawnerData : ScriptableObject
{
    [SerializeField] private StarMaterial _materialPrefab;
    [SerializeField] private int _materialLimit;
    [SerializeField] private int _spawnPerMinute;


    public StarMaterial materialPrefab => _materialPrefab;
    public int materialLimit => _materialLimit;
    public int spawnPerMinute => _spawnPerMinute;
}
