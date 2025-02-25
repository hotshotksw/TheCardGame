using Newtonsoft.Json;
using System;
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

    public enum Filter
    {
        NONE,
        RARITY,
        ARTIST,
        ID,
        HOLLOW
    }

    public Filter cardFilter = Filter.NONE;
    

    private CardData cardData;
    private Dictionary<string, List<CardDataBase>> cardDictionary;
    public void Render(Inventory inven)
    {
        ClearActiveCards();
        int index = 0;
        int yOffset = 0;
        var invenCopy = inven;

        switch(cardFilter)
        {
            case Filter.NONE: // NO FILTER
                foreach (var item in invenCopy.GetCompleteCollection())
                {
                    if (((index % cardPerRow) == 0) && (index != 0))
                    {
                        yOffset += offsetPerRow;
                    }
                    Vector3 pos = (index - ((yOffset / offsetPerRow) * 3)) * offsetPerCard;
                    pos.y = yOffset;

                    GameObject card = Instantiate(cardPrefab, transform.position + pos, Quaternion.identity, transform);

                    int temp = inventory.GetCardAtIndex(index);
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
                break;
            case Filter.RARITY: // RARITY FILTER
                foreach(CardRarity rarity in Enum.GetValues(typeof(CardRarity))) // Sort through common rarities commmon -> mythical
                {
                    foreach (var c in invenCopy.GetCompleteCollection())
                    {
                        if (((index % cardPerRow) == 0) && (index != 0))
                        {
                            yOffset += offsetPerRow;
                        }
                        Vector3 pos = (index - ((yOffset / offsetPerRow) * 3)) * offsetPerCard;
                        pos.y = yOffset;

                        GameObject card = Instantiate(cardPrefab, transform.position + pos, Quaternion.identity, transform);

                        int temp = inventory.GetCardAtIndex(index);
                        Debug.LogWarning(rarity);
                        // Set up Cards

                        TextAsset cards = Resources.Load<TextAsset>("cards");
                        cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);

                        //cardData.LoadData(cardDictionary["cards"][temp]);
                        //card.GetComponent<CardJSONReader>().cardData = cardData;
                        card.GetComponent<CardJSONReader>().cardDictionary = cardDictionary;
                        card.GetComponent<CardJSONReader>().cardID = temp;

                        if (card.GetComponent<CardData>().cardRarity == rarity) // Add them in order from common -> mythical
                        {
                            activeCards.Add(card);
                            index++;
                        }
                    }
                }
                break;
            case Filter.ARTIST: // ARTIST FILTER
                foreach (CardArtist artist in Enum.GetValues(typeof(CardArtist)))
                {
                    foreach (var c in invenCopy.GetCompleteCollection())
                    {
                        if (((index % cardPerRow) == 0) && (index != 0))
                        {
                            yOffset += offsetPerRow;
                        }
                        Vector3 pos = (index - ((yOffset / offsetPerRow) * 3)) * offsetPerCard;
                        pos.y = yOffset;

                        GameObject card = Instantiate(cardPrefab, transform.position + pos, Quaternion.identity, transform);

                        int temp = inventory.GetCardAtIndex(index);
                        Debug.LogWarning(artist);
                        // Set up Cards

                        TextAsset cards = Resources.Load<TextAsset>("cards");
                        cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);

                        //cardData.LoadData(cardDictionary["cards"][temp]);
                        //card.GetComponent<CardJSONReader>().cardData = cardData;
                        card.GetComponent<CardJSONReader>().cardDictionary = cardDictionary;
                        card.GetComponent<CardJSONReader>().cardID = temp;

                        if (card.GetComponent<CardData>().cardArtist == artist)
                        {
                            activeCards.Add(card);
                            index++;
                        }
                    }
                }
                break;
            case Filter.ID: // ID FILTER
                //foreach (int id in cardDictionary.Keys)
                //{
                //    foreach (var c in invenCopy.GetCompleteCollection())
                //    {
                //        if (((index % cardPerRow) == 0) && (index != 0))
                //        {
                //            yOffset += offsetPerRow;
                //        }
                //        Vector3 pos = (index - ((yOffset / offsetPerRow) * 3)) * offsetPerCard;
                //        pos.y = yOffset;

                //        GameObject card = Instantiate(cardPrefab, transform.position + pos, Quaternion.identity, transform);

                //        int temp = inventory.GetCardAtIndex(index);
                //        Debug.LogWarning(id);
                //        Set up Cards

                //       TextAsset cards = Resources.Load<TextAsset>("cards");
                //        cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);

                //        cardData.LoadData(cardDictionary["cards"][temp]);
                //        card.GetComponent<CardJSONReader>().cardData = cardData;
                //        card.GetComponent<CardJSONReader>().cardDictionary = cardDictionary;
                //        card.GetComponent<CardJSONReader>().cardID = temp;

                //        if (card.GetComponent<CardData>().cardID == id)
                //        {
                //            activeCards.Add(card);
                //            index++;
                //        }
                //    }
                //}
                break;
            case Filter.HOLLOW: // HOLLOW FILTER
                //foreach (Card in Enum.GetValues(typeof(CardRarity)))
                //{
                //    foreach (var c in invenCopy.GetCompleteCollection())
                //    {
                //        if (((index % cardPerRow) == 0) && (index != 0))
                //        {
                //            yOffset += offsetPerRow;
                //        }
                //        Vector3 pos = (index - ((yOffset / offsetPerRow) * 3)) * offsetPerCard;
                //        pos.y = yOffset;

                //        GameObject card = Instantiate(cardPrefab, transform.position + pos, Quaternion.identity, transform);

                //        int temp = inventory.GetCardAtIndex(index);
                //        Debug.LogWarning(rarity);
                //        // Set up Cards

                //        TextAsset cards = Resources.Load<TextAsset>("cards");
                //        cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);

                //        //cardData.LoadData(cardDictionary["cards"][temp]);
                //        //card.GetComponent<CardJSONReader>().cardData = cardData;
                //        card.GetComponent<CardJSONReader>().cardDictionary = cardDictionary;
                //        card.GetComponent<CardJSONReader>().cardID = temp;

                //        if (card.GetComponent<CardData>().cardRarity == rarity)
                //        {
                //            activeCards.Add(card);
                //            index++;
                //        }
                //    }
                //}
                break;

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