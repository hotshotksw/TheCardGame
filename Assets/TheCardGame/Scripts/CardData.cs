using UnityEngine;

public class CardData : ScriptableObject
{

    public string CardName;
    public CardType cardType;
    public CardRarity cardRarity;
    public string CardDescription;
    public CardStages cardStages;
    public CardArtist cardArtist;

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
    Caden
}