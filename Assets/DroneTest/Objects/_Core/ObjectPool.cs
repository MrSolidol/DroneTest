using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;

public class ObjectPool<T> where T : MonoBehaviour
{
    public T prefab { get; }
    public bool autoExpand { get; set; } = false;
    public bool isActiveByDeafault { get; set; } = false;
    public Transform container { get; }
    private List<T> pool;


    public ObjectPool(T _prefab, int _count)
    {
        prefab = _prefab;
        container = null;
        CreatePool(_count);
    }

    public ObjectPool(T _prefab, int _count, Transform _container)
    {
        prefab = _prefab;
        container = _container;
        CreatePool(_count);
    }

    public ObjectPool(T _prefab, int _count, Transform _container, bool _autoExpand, bool _isActiveByDeafault)
    {
        prefab = _prefab;
        container = _container;
        autoExpand = _autoExpand;
        isActiveByDeafault = _isActiveByDeafault;
        CreatePool(_count);
    }


    public bool HasFreeElement(out T element)
    {
        foreach (var mono in pool)
        {
            if (!mono.gameObject.activeInHierarchy)
            {
                element = mono;
                mono.gameObject.SetActive(true);
                return true;
            }
        }

        element = null;
        return false;
    }

    public T GetFreeElement()
    {
        if (HasFreeElement(out var element))
        { return element; }

        if (autoExpand)
        { return CreateObject(true); }

        return null;
    }

    public List<T> GetPoolList()
    {
        return pool;
    }


    private void CreatePool(int _count)
    {
        pool = new List<T>();

        for (int i = 0; i < _count; i++) { CreateObject(); }
    }

    private T CreateObject(bool isActiveByDeafault = false)
    {
        var createdObject = Object.Instantiate(prefab, container);
        createdObject.gameObject.SetActive(isActiveByDeafault);
        pool.Add(createdObject);
        return createdObject;
    }
}
