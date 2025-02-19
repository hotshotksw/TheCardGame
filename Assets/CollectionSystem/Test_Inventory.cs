using UnityEngine;

public class Test_Inventory : MonoBehaviour
{
    
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
        _inventory.AddCard(cardToAdd);
    }

    [ContextMenu("Remove")]
    public void RemoveCard()
    {
        _inventory.RemoveCard(cardToAdd);
    }

    [SerializeField] private int cardToAdd;
}