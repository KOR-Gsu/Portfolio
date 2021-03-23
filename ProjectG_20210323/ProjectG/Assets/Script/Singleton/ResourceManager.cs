using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject prefab = Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.Log($"Filed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, parent);
    }

    public GameObject Instantiate(string path, Vector3 position)
    {
        GameObject prefab = Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.Log($"Filed to load prefab : {path}");
            return null;
        }

        return Object.Instantiate(prefab, position, Quaternion.identity);
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        Object.Destroy(go);
    }
}
