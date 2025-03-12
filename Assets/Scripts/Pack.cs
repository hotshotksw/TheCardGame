using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class Pack : MonoBehaviour
{
    Dictionary<string, List<CardDataBase>> cardDictionary;
    [SerializeField] List<int> PullList;
    [SerializeField] Inventory inventory;
    float CommonChance = 60.0f;
    float UncommonChance = 25.0f;
    float RareChance = 9.0f;
    float SuperRareChance = 5.0f;
    float MythicalChance = 1.0f;

    void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<Inventory>();
        
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

    public void GetOneCard(GameManager_OLD.SceneCard card)
    {
        string rarity = getRarity();

        int random = UnityEngine.Random.Range(0, cardDictionary[rarity].Count);
        while(cardDictionary[rarity][random] == null)
        {
            random = UnityEngine.Random.Range(0, cardDictionary[rarity].Count);
        }

        card.CardObject.GetComponent<CardJSONReader>().UpdateData(cardDictionary[rarity][random].ID);

        if (UnityEngine.Random.Range(0, 101) <= 10)
        {
            card.CardObject.GetComponent<CardJSONReader>().cardData.isHolo = true;
            card.CardObject.GetComponent<CardJSONReader>().renderer.material.SetInt("_Holographic", 1);
            inventory.AddCard(cardDictionary[rarity][random].ID, true);
        } else {
            card.CardObject.GetComponent<CardJSONReader>().cardData.isHolo = false;
            card.CardObject.GetComponent<CardJSONReader>().renderer.material.SetInt("_Holographic", 0);
            inventory.AddCard(cardDictionary[rarity][random].ID, false);
        }
    }

    public void GetTenCards(List<GameManager_OLD.SceneCard> cards)
    {
        for(int i = 0; i < 10; i++)
        {
            GetOneCard(cards[i]);
        }
    }

    private string getRarity()
    {
        string rarity = "";
        if ((CommonChance + UncommonChance + RareChance + SuperRareChance + MythicalChance) != 100.0f)
        {
            rarity = ((CardRarity)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CardRarity)).Length)).ToString();
            while (cardDictionary[rarity].Count == 0)
            {
                rarity = ((CardRarity)UnityEngine.Random.Range(0, Enum.GetValues(typeof(CardRarity)).Length)).ToString();
            }
        }
        else
        {
            float CommonCeiling = CommonChance;
            float UncommonCeiling = CommonCeiling + UncommonChance;
            float RareCeiling = UncommonCeiling + RareChance;
            float SuperRareCeiling = RareCeiling + SuperRareChance;
            float MythicalCeiling = SuperRareCeiling + MythicalChance;
            float selection = UnityEngine.Random.Range(0.0f, 100.0f);
            if (selection > 0 && selection <= CommonCeiling)
            {
                return "Common";
            }
            else if (selection > CommonCeiling && selection <= UncommonCeiling)
            {
                return "Uncommon";
            }
            else if (selection > UncommonCeiling && selection <= RareCeiling)
            {
                return "Rare";
            }
            else if (selection > RareCeiling && selection <= SuperRareCeiling)
            {
                return "SuperRare";
            }
            else if (selection > SuperRareCeiling && selection <= MythicalCeiling)
            {
                return "Mythical";
            }
            return "Something Broke";
        }



        return rarity;
    }
}
