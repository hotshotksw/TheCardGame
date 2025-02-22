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

    [SerializeField] private int cardPerRow;

    [SerializeField] private Vector2 offsetPerCard;
    [SerializeField] private int offsetPerRow;
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
        int yOffset = 0;
        foreach (var item in inven.GetCompleteCollection())
        {
            if(((index % cardPerRow) == 0) && (index != 0))
            {
                yOffset += offsetPerRow;
            }
            Vector3 pos = (index - ((yOffset / offsetPerRow) * 3)) * offsetPerCard;
            pos.y = yOffset;
            CollectedCardDisplay card = Instantiate(cardPrefab, transform.position + pos, Quaternion.identity, transform);
            card.Setup(item);
            activeCards.Add(card);
            index++;
        }
    }
}