using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Card", menuName = "Card System/Card")]
public class CardData : MonoBehaviour
{

    public string CardName;
    public CardType cardType;
    public CardRarity cardRarity;
    public string CardDescription;
    public CardStages cardStages;
    public CardArtist cardArtist;
    public Texture2D cardImage;
    public Texture2D cardBorder;
    public bool isHolo;

    public void LoadData(CardDataBase cardData)
    {
        CardName = cardData.cardName;
        cardType = Enum.Parse<CardType>(cardData.type);
        cardRarity = Enum.Parse<CardRarity>(cardData.rarity.Replace(" ",""));
        CardDescription = cardData.description;
        cardArtist = Enum.Parse<CardArtist>(cardData.artist);
        cardImage = Resources.Load<Texture2D>(cardData.image);
        string borderName = "Card/border_" + cardType.ToString().ToLowerInvariant();
        cardBorder = Resources.Load<Texture2D>(borderName);
        //isHolo = cardData.holographic;
    }
}



public enum CardType
{
    Aerial,
    Basilisk,
    Boulder,
    Brawl,
    Fae,
    Flame,
    Floral,
    Hydro,
    Lumen,
    Metal,
    Psyonic,
    Shadow,
    Toxic,
    Voltaic,
    Virtuous,
    Evil
}

public enum CardRarity
{
    Common,
    Uncommon,
    Rare,
    SuperRare,
    Mythical
}

public enum CardStages
{
    Basic,
    Stage1,
    Stage2
}

public enum CardArtist
{
    Kyle,
    Bennie,
    Joshua,
    Caden,
    Abby,
    Brendon,
    David,
    Jackson,
    Jayden,
    Jacob,
    Chase,
    Kian
}