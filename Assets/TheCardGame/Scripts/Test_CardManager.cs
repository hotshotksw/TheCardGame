using System.Security.Cryptography;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UIElements;

public class Test_CardManager : MonoBehaviour
{
    [SerializeField] GameObject CardPrefab;
    [SerializeField] GameObject CurrentCard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawNewCard()
    {
        Destroy(CurrentCard);

        CurrentCard = Instantiate(CardPrefab);

        CardJSONReader json = CurrentCard.GetComponent<CardJSONReader>();
        Debug.LogWarning("Draw Card:\n" + json);
        if (json)
        {
            json.UpdateData(Random.Range(0,15));
            json.UpdateData();
        }
    }
}
