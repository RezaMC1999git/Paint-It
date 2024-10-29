using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScratchMaskObjectPool : MonoBehaviour
{
    // Summary : Generates Masks PreRendered GameObjects, to Use Whenever Needed

    public static ScratchMaskObjectPool instance;

    public LevelManager levelManager;
    public ScratchMask maskPrefab;

    [HideInInspector] public List<ScratchMask> pooledObjects = new List<ScratchMask>();

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        // Generate Masks as much as We Need
        PoolMasks();
    }

    private void PoolMasks()
    {
        for (int i = 0; i < levelManager.requiredMasksForFinishingLevel; i++)
        {
            ScratchMask newMask = Instantiate(maskPrefab, this.transform);
            newMask.transform.SetParent(this.transform);
            newMask.gameObject.SetActive(false);
            newMask.name = "mask" + i;
            pooledObjects.Add(newMask);
        }
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            pooledObjects[i].transform.position = new Vector3(1f, 1f, 1f); 
        }
    }

    public ScratchMask getMaskPooledObject()
    {
        // returns first diactive mask
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].gameObject.activeInHierarchy)
                return pooledObjects[i];
        }

        return null;
    }
}
