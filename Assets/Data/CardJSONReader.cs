using UnityEngine;




using System.IO;
using UnityEngine.Windows.Speech;
using System.Collections.Generic;
using Newtonsoft.Json;

public class CardJSONReader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TextAsset cards = Resources.Load<TextAsset>("cards");
        Dictionary<string, List<CardDataBase>> cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
