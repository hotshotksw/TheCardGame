using UnityEngine;

public class Test_Inventory : MonoBehaviour
{
    void Start()
    {
    }

    [SerializeField] private Inventory _inventory;
    [SerializeField] private InventorySaving saver;

    [ContextMenu("Write")]
    public void Write()
    {
        saver.Write(_inventory);
    }

    [ContextMenu("Read")]
    public void Read()
    {
        saver.Read(_inventory);
    }

    [ContextMenu("AddCard")]
    public void AddCard()
    {
        _inventory.AddCard(new CardDataBase() { cardName = cardToAdd.cardName, image = cardToAdd.image, artist = cardToAdd.artist, description = cardToAdd.description, rarity = cardToAdd.rarity, type = cardToAdd.type, holographic = cardToAdd.holographic });
    }

    [ContextMenu("Remove")]
    public void RemoveCard()
    {
        _inventory.RemoveCard(cardToAdd);
    }

    [SerializeField] private CardDataBase cardToAdd;
}