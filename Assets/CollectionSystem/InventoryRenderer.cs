using Newtonsoft.Json;
using System.Collections.Generic;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

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
    [SerializeField] private Inventory inventory;
    [SerializeField] private Vector2 offsetPerCard;
    [SerializeField] private int offsetPerRow;
    [SerializeField] private GameObject cardPrefab;
    private List<GameObject> activeCards = new();

    private CardData cardData;
    private Dictionary<string, List<CardDataBase>> cardDictionary;
    private void Render(Inventory inven)
    {
        ClearActiveCards();
        int index = 0;
        int yOffset = 0;
        var invenCopy = inven;
        foreach (var item in invenCopy.GetCompleteCollection())
        {
            if(((index % cardPerRow) == 0) && (index != 0))
            {
                yOffset += offsetPerRow;
            }
            Debug.LogWarning("CompleteCollection: " + invenCopy.GetCompleteCollection().Count.ToString());
            Vector3 pos = (index - ((yOffset / offsetPerRow) * 3)) * offsetPerCard;
            pos.y = yOffset;

            GameObject card = Instantiate(cardPrefab, transform.position + pos, Quaternion.identity, transform);

            int temp = inventory.GetCardAtIndex(index);
            Debug.LogWarning(temp);
            // Set up Cards

            TextAsset cards = Resources.Load<TextAsset>("cards");
            cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);

            //cardData.LoadData(cardDictionary["cards"][temp]);
            //card.GetComponent<CardJSONReader>().cardData = cardData;
            card.GetComponent<CardJSONReader>().cardDictionary = cardDictionary;
            card.GetComponent<CardJSONReader>().cardID = temp;

            activeCards.Add(card);
            index++;
        }
    }

    private void ClearActiveCards()
    {
        for (int i = activeCards.Count - 1; i >= 0; i--)
        {
            Destroy(activeCards[i].gameObject);
        }
        activeCards.Clear();
    }

    public void ToggleDisplay(bool isVisible)
    {
        if(isVisible)
        {
            OnEnable();
           
        }
        else
        {
            ClearActiveCards();
            OnDisable();
        }
        
    }
}