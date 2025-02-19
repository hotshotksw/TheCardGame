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
}

[Serializable]
public class CollectionSet
{
    public int cardID;
    public int quantity;
    public bool holographic;
    public bool Has => quantity > 0;
}