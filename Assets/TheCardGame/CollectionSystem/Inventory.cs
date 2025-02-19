using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, CollectionSet> collection = new Dictionary<string, CollectionSet>();
    public static Action<Inventory> OnCollectionChanged = delegate { };

    public void AddCard(CardDataBase cardData)
    {
        if (collection.ContainsKey(cardData.cardName))
        {
            collection[cardData.cardName].quantity++;
        }
        else
        {
            collection.Add(cardData.cardName, new CollectionSet() { data = cardData, quantity = 1 });
        }
        OnCollectionChanged?.Invoke(this);
    }

    public void RemoveCard(CardDataBase cardData)
    {
        if (collection.ContainsKey(cardData.cardName))
        {
            if (collection[cardData.cardName].Has)
            {
                collection[cardData.cardName].quantity--;
                OnCollectionChanged?.Invoke(this);
            }
        }
    }

    public CollectionSet SetOfCard(CardDataBase match)
    {
        if (collection.ContainsKey(match.cardName))
        {
            return collection[match.cardName];
        }
        return new CollectionSet() { data = match, quantity = 0 };
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
            collection.Add(item.data.cardName, item);
        }
    }
}

[Serializable]
public class CollectionSet
{
    public CardDataBase data;
    public int quantity;
    public bool Has => quantity > 0;
}