using UnityEngine;




using System.IO;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using TMPro;

[RequireComponent(typeof(CardData))]
public class CardJSONReader : MonoBehaviour
{
    [SerializeField] int cardID;
    public GameObject cardPrefab;
    public CardData cardData;
    [SerializeField] Dictionary<string, List<CardDataBase>> cardDictionary;
    [SerializeField] Material material;
    [SerializeField] Renderer renderer;
    [SerializeField] GameObject name;
    [SerializeField] GameObject description;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardData = gameObject.GetComponent<CardData>();
        material = gameObject.GetComponent<Material>();
        renderer = gameObject.GetComponent<Renderer>();
        TextAsset cards = Resources.Load<TextAsset>("cards");
        cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
        //var CurrentCard = Instantiate(cardPrefab);
        //CurrentCard.GetComponent<CardData>().LoadData(cardDictionary["cards"][cardID]);
        cardData.LoadData(cardDictionary["cards"][cardID]);
        var names = Enum.GetNames(typeof(CardRarity));
        foreach (var name in names)
        {
            cardDictionary[name] = new List<CardDataBase>();
        }
        foreach (var item in cardDictionary["cards"])
        {
            item.ID = cardDictionary["cards"].IndexOf(item);
            cardDictionary[item.rarity.Replace(" ","")].Add(item);
        }
        UpdateData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateData()
    {
        cardData.LoadData(cardDictionary["cards"][cardID]);
        var cardNAme = cardData.cardImage.ToString();
        var newMaterial = Resources.Load<Material>("Materials/"+cardDictionary["cards"][cardID].image);
        renderer.material = newMaterial;
        name.GetComponent<TextMeshPro>().text = cardData.CardName;
        description.GetComponent<TextMeshPro>().text = cardData.CardDescription;
    }

    public void UpdateData(int newID)
    {
        cardID = newID;
        cardData.LoadData(cardDictionary["cards"][cardID]);
        var newMaterial = Resources.Load<Material>("Materials/" + cardDictionary["cards"][cardID].image);
        renderer.material = newMaterial;
        name.GetComponent<TextMeshPro>().text = cardData.CardName;
        description.GetComponent<TextMeshPro>().text = cardData.CardDescription;
    }
}
