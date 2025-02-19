using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class InventorySaving : MonoBehaviour
{
    [SerializeField] private string path;

    
    public void Write(Inventory inventory)
    {
        CollectionSetWrapper wrapper = new CollectionSetWrapper();
        wrapper.collectionSets = inventory.GetCompleteCollection().ToArray();
        string json = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(Path.Combine(Application.dataPath, path), json);
    }

    public void Read(Inventory inventory)
    {
        CollectionSetWrapper collectionSetList = JsonUtility.FromJson(File.ReadAllText(Path.Combine(Application.dataPath, path)), typeof(CollectionSetWrapper)) as CollectionSetWrapper;
        inventory.SetCollection(collectionSetList.collectionSets.ToList());
    }
}
[System.Serializable]
public class CollectionSetWrapper
{
    public CollectionSet[] collectionSets;
}