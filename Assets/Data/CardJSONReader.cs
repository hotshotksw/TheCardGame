using UnityEngine;




using System.IO;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CardJSONReader : MonoBehaviour
{
    public GameObject cardPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset cards = Resources.Load<TextAsset>("cards");
        Dictionary<string, List<CardDataBase>> cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
        var CurrentCard = Instantiate(cardPrefab);
        CurrentCard.GetComponent<CardData>().Instantiate(cardDictionary["cards"][0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
