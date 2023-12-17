using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool instance;
    [SerializeField] private GameObject objectToPool;

    [SerializeField] private List<GameObject> pooledObjects = new List<GameObject>();
    [SerializeField] private int numberOfPooledObjects;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < numberOfPooledObjects; i++)
        {
           GameObject obj =  Instantiate(objectToPool);
           obj.SetActive(false);
           pooledObjects.Add(obj);
        }
    }

    public GameObject GetPooledObject()
    {

        for (int j = 0; j < pooledObjects.Count; j++)
        {
            if (!pooledObjects[j].activeInHierarchy)
            {
                pooledObjects[j].SetActive(true);
                return pooledObjects[j];
            }
        }
        return null;
    }
}
