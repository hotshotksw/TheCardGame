using System.Collections.Generic;
using UnityEngine;

public class InventoryRenderer : MonoBehaviour
{
    private void OnEnable()
    {
        Inventory.OnCollectionChanged += Render;
    }

    private void OnDisable()
    {
        Inventory.OnCollectionChanged -= Render;
    }

    [SerializeField] private Vector2 offsetPerCard;
    [SerializeField] private CollectedCardDisplay cardPrefab;
    private List<CollectedCardDisplay> activeCards = new();
    private void Render(Inventory inven)
    {
        for (int i = activeCards.Count - 1; i >= 0; i--)
        {
            Destroy(activeCards[i].gameObject);
        }
        activeCards.Clear();
        int index = 0;
        foreach (var item in inven.GetCompleteCollection())
        {
            CollectedCardDisplay card = Instantiate(cardPrefab, transform.position + (Vector3)(index * offsetPerCard), Quaternion.identity, transform);
            card.Setup(item);
            activeCards.Add(card);
            index++;
        }
    }
}