using UnityEngine;
using Zenject;

public class MainSceneEnterPoint : MonoInstaller
{
    [SerializeField] private MaterialSpawnerData spawnerData;
    ReactiveVariable<int> spawnPerMinute;

    [SerializeField] private Base firstBase;
    [SerializeField] private NumberView firstNumberView;
    [SerializeField] private SliderView firstSliderView;
    [SerializeField] private Base secondBase;
    [SerializeField] private NumberView secondNumberView;
    [SerializeField] private SliderView secondSliderView;
    private ReactiveVariable<int> firstDroneCount;
    private ReactiveVariable<int> secondDroneCount;
    private ReactiveVariable<int> firstMaterialCount;
    private ReactiveVariable<int> secondMaterialCount;

    public override void InstallBindings()
    {
        BindMaterialSpawner();
        BindBases();
    }

    private void BindMaterialSpawner()
    {
        StarMaterial materialPrefab = spawnerData.materialPrefab;
        spawnPerMinute = new ReactiveVariable<int>(spawnerData.spawnPerMinute);
        int materialLimit = spawnerData.materialLimit;

        Container.BindInstance(materialPrefab).WhenInjectedInto<MaterialSpawner>();
        Container.BindInstance(spawnPerMinute).WhenInjectedInto<MaterialSpawner>();
        Container.BindInstance(materialLimit).WhenInjectedInto<MaterialSpawner>();
        Container.BindInterfacesAndSelfTo<MaterialSpawner>().FromComponentInHierarchy().AsSingle().NonLazy();
    }

    private void BindBases()
    {
        firstDroneCount = new ReactiveVariable<int>(5);
        secondDroneCount = new ReactiveVariable<int>(5);

        firstMaterialCount = new ReactiveVariable<int>(0);
        secondMaterialCount = new ReactiveVariable<int>(0);



        MaterialSpawner spawner = FindAnyObjectByType<MaterialSpawner>();
        firstBase.Construct(spawner, firstDroneCount, firstMaterialCount);
        firstNumberView.Construct(firstMaterialCount);
        firstSliderView.Construct(firstDroneCount);
        secondBase.Construct(spawner, secondDroneCount, secondMaterialCount);
        secondNumberView.Construct(secondMaterialCount);
        secondSliderView.Construct(secondDroneCount);
    }
}
