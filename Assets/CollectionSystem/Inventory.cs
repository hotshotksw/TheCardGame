using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<int, CollectionSet> collection = new Dictionary<int, CollectionSet>();
    public static Action<Inventory> OnCollectionChanged = delegate { };

    public void AddCard(int cardID, bool holographic = false)
    {
        if (collection.ContainsKey(cardID))
        {
            collection[cardID].quantity++;
            if(holographic) collection[cardID].holographic = true;
        }
        else
        {
            collection.Add(cardID, new CollectionSet() { cardID = cardID, quantity = 1, holographic = holographic});
        }
        OnCollectionChanged?.Invoke(this);
    }

    public void RemoveCard(int cardID)
    {
        if (collection.ContainsKey(cardID))
        {
            if (collection[cardID].Has)
            {
                collection[cardID].quantity--;
                OnCollectionChanged?.Invoke(this);
            }
        }
    }

    public CollectionSet SetOfCard(int matchID)
    {
        if (collection.ContainsKey(matchID))
        {
            return collection[matchID];
        }
        return new CollectionSet() { cardID = matchID, quantity = 0 };
    }

    public void LoadCardID(int cardID)
    {
        if (!collection.ContainsKey(cardID))
        {
            collection.Add(cardID, new CollectionSet() { cardID = cardID, quantity = 0 });
        }
        OnCollectionChanged?.Invoke(this);
    }

    public List<CollectionSet> GetCompleteCollection()
    {
        return new List<CollectionSet>(collection.Values);
    }

    public void SetCollection(List<CollectionSet> inventory)
    {
        collection.Clear();
        foreach (var item in inventory)
        {
            collection.Add(item.cardID, item);
        }
        OnCollectionChanged?.Invoke(this);
    }

    /// <summary>
    /// Iterates through the collection, finding the card at a specific index of the collection.
    /// </summary>
    /// <param name="index"></param>
    /// <returns> returns -1 if index is out of range or doesn't exist. </returns>
    public int GetCardAtIndex(int index)
    {
        Debug.LogWarning("Collection Count: " + collection.Count.ToString());
        Debug.LogWarning("Input Index: " + index.ToString());
        if (collection.Count <= (index)) return -1;

        int tempIndex = 0;
        foreach (KeyValuePair<int, CollectionSet> pair in collection)
        {
            Debug.LogWarning("Temp Index" + tempIndex.ToString());
            if (tempIndex == index)
            {
                Debug.LogWarning("Index Get: " + tempIndex.ToString());
                return pair.Key;
            }
            tempIndex++;
        }

        return -1;
    }
}

[Serializable]
public class CollectionSet
{
    public int cardID;
    public int quantity;
    public bool holographic;
    public bool Has => quantity > 0;

    public bool isHolo()
    {
        return holographic;
    }
}