using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class Pack : MonoBehaviour
{
    private bool Opened = false;
    Dictionary<string, List<CardDataBase>> cardDictionary;
    [SerializeField] List<int> PullList;
    
    void Start()
    {
        TextAsset cards = Resources.Load<TextAsset>("cards");
        cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
        var names = Enum.GetNames(typeof(CardRarity));

        if (PullList.Count == 0)
        {
            foreach (var name in names)
            {
                cardDictionary[name] = new List<CardDataBase>();
            }
            foreach (var item in cardDictionary["cards"])
            {
                item.ID = cardDictionary["cards"].IndexOf(item);
                cardDictionary[item.rarity.Replace(" ","")].Add(item);
            }
        }
        else
        {
            foreach (var name in names)
            {
                cardDictionary[name] = new List<CardDataBase>();
            }
            foreach (var item in PullList)
            {
                var card = cardDictionary["cards"][item];
                card.ID = item;
                cardDictionary[card.rarity.Replace(" ","")].Add(card);
            }
        }
    }

    public void GetOneCard(GameManager.SceneCard card)
    {
        string rarity = ((CardRarity)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CardRarity)).Length)).ToString();
        while (cardDictionary[rarity].Count == 0)
        {
            rarity = ((CardRarity)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CardRarity)).Length)).ToString();
        }

        int random = UnityEngine.Random.Range(0, cardDictionary[rarity].Count);
        while(cardDictionary[rarity][random] == null)
        {
            random = UnityEngine.Random.Range(0, cardDictionary[rarity].Count);
        }

        card.CardObject.GetComponent<CardJSONReader>().UpdateData(cardDictionary[rarity][random].ID);
        card.CardObject.GetComponent<CardJSONReader>().renderer.material.SetInt("_Holographic", UnityEngine.Random.Range(0, 2));
    }

    public void GetTenCards(List<GameManager.SceneCard> cards)
    {
        for(int i = 0; i < 10; i++)
        {
            GetOneCard(cards[i]);
        }
    }
}
