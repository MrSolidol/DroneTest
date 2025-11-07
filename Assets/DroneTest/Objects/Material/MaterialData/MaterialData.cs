using UnityEngine;

[CreateAssetMenu(fileName = "MaterialData", menuName = "Gameplay/MaterialData")]
public class MaterialData : ScriptableObject
{
    [SerializeField] private string _materialName;
    [SerializeField] private int _materialWeight;
    [SerializeField] private float _materialCollectionTime;


    public string materialName => this._materialName;
    public int materialWeight => this._materialWeight;
    public float materialCollectionTime => this._materialCollectionTime;
}
