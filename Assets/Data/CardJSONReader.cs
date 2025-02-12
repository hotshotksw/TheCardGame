using UnityEngine;




using System.IO;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using Newtonsoft.Json;

[RequireComponent(typeof(CardData))]
public class CardJSONReader : MonoBehaviour
{
    [SerializeField] int cardID;
    public GameObject cardPrefab;
    public CardData cardData;
    private Dictionary<string, List<CardDataBase>> cardDictionary;
    [SerializeField] Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cardData = gameObject.GetComponent<CardData>();
        //material = gameObject.GetComponent<Material>();
        TextAsset cards = Resources.Load<TextAsset>("cards");
        cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
        //var CurrentCard = Instantiate(cardPrefab);
        //CurrentCard.GetComponent<CardData>().LoadData(cardDictionary["cards"][cardID]);
        cardData.LoadData(cardDictionary["cards"][cardID]);
        UpdateData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateData()
    {
        cardData.LoadData(cardDictionary["cards"][cardID]);
        material.SetTexture("_Card_Image", cardData.cardImage);
        material.SetTexture("_Card_Border", cardData.cardBorder);
    }

    void UpdateData(int newID)
    {
        cardID = newID;
        cardData.LoadData(cardDictionary["cards"][cardID]);
    }
}
