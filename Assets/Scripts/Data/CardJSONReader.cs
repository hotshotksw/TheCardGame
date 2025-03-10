using UnityEngine;




using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;
using TMPro;

[RequireComponent(typeof(CardData))]
public class CardJSONReader : MonoBehaviour
{
    [SerializeField] public int cardID;
    public GameObject cardPrefab;
    public CardData cardData;
    [SerializeField] public Dictionary<string, List<CardDataBase>> cardDictionary;
    [SerializeField] Material material;
    [SerializeField] public Renderer renderer;
    [SerializeField] GameObject name;
    [SerializeField] GameObject description;
    [SerializeField] GameObject artist;
    [SerializeField] bool isHolo;
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
        isHolo = cardData.isHolo;
        var cardNAme = cardData.cardImage.ToString();
        var newMaterial = Resources.Load<Material>("Materials/"+cardDictionary["cards"][cardID].image);
        renderer.material.SetTexture("_Card_Image", cardData.cardImage);
        renderer.material.SetTexture("_Card_Border", cardData.cardBorder);
        name.GetComponent<TextMeshPro>().text = cardData.CardName;
        description.GetComponent<TextMeshPro>().text = cardData.CardDescription;
        artist.GetComponent<TextMeshPro>().text = "Art by " + cardData.cardArtist.ToString();
    }
    
    public void UpdateData(int newID)
    {
        cardID = newID;
        cardData.LoadData(cardDictionary["cards"][cardID]);
        isHolo = cardData.isHolo;
        var newMaterial = Resources.Load<Material>("Materials/" + cardDictionary["cards"][cardID].image);
        renderer.material.SetTexture("_Card_Image", cardData.cardImage);
        renderer.material.SetTexture("_Card_Border", cardData.cardBorder);
        name.GetComponent<TextMeshPro>().text = cardData.CardName;
        description.GetComponent<TextMeshPro>().text = cardData.CardDescription;
        artist.GetComponent<TextMeshPro>().text = "Art by " + cardData.cardArtist.ToString();
    }

    public static List<CardDataBase> getCards()
    {
        TextAsset cards = Resources.Load<TextAsset>("cards");
        var dict = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
        return dict["cards"];
    }

    public static int getIDbyName(string name)
    {
        TextAsset cards = Resources.Load<TextAsset>("cards");
        var dict = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
        int id = -1;

        for (int i = 0; i < dict["cards"].Count; i++)
        {
            if (dict["cards"][i].cardName == name)
            {
                id = i;
                break;
            }
        }

        return id;
    }
}
