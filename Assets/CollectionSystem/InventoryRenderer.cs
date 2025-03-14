using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Android.Gradle.Manifest;
using Unity.Mathematics;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

public class InventoryRenderer : MonoBehaviour
{
    private void OnEnable()
    {
        Inventory.OnCollectionChanged += Render;
    }

    private void OnDisable()
    {
        Inventory.OnCollectionChanged -= Render;
    }

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private int cardPerRow;
    [SerializeField] private Inventory inventory;
    [SerializeField] private Vector2 offsetPerCard;
    [SerializeField] private int offsetPerRow;
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;  // The container to hold cards in the scene
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MenuManage menuManager;
    private List<GameObject> activeCards = new();
    private bool isActive = false;

    private bool isDragging = false;
    private Vector3 initialContainerPos;
    private Vector3 mouseStartPos;

    // Scroll lock variables
    private float maxScrollHeight;
    private float minScrollHeight;

    public enum Filter
    {
        NONE,
        RARITY,
        ARTIST,
        ID,
        HOLLOW
    }

    public Filter cardFilter = Filter.NONE;

    private CardData cardData;
    private Dictionary<string, List<CardDataBase>> cardDictionary;

    public void Render(Inventory inven)
    {
        ClearActiveCards();
        int index = 0;
        int yOffset = 0;

        // Get a sorted copy of the inventory based on the current filter
        List<CollectionSet> sortedCollection = new List<CollectionSet>(inven.GetCompleteCollection());

        switch (cardFilter)
        {
            case Filter.NONE:
                break;

            case Filter.RARITY:
                sortedCollection.Sort((a, b) =>
                    cardDictionary["cards"][a.cardID].rarity.CompareTo(cardDictionary["cards"][b.cardID].rarity));
                break;

            case Filter.ARTIST:
                sortedCollection.Sort((a, b) =>
                    cardDictionary["cards"][a.cardID].artist.CompareTo(cardDictionary["cards"][b.cardID].artist));
                break;

            case Filter.ID:
                sortedCollection.Sort((a, b) => a.cardID.CompareTo(b.cardID));
                break;

            case Filter.HOLLOW:
                sortedCollection = sortedCollection.Where(card => card.holographic).ToList();
                break;
        }

        foreach (var item in sortedCollection)
        {
            if (((index % cardPerRow) == 0) && (index != 0))
            {
                yOffset += offsetPerRow;
            }

            Vector3 pos = new Vector3((index % cardPerRow) * offsetPerCard.x, yOffset, 0);
            GameObject card = Instantiate(cardPrefab, cardContainer.position + pos, Quaternion.identity, cardContainer);

            // Get card ID from sorted inventory
            int temp = item.cardID;

            // Load card data from JSON
            TextAsset cards = Resources.Load<TextAsset>("cards");
            cardDictionary = JsonConvert.DeserializeObject<Dictionary<string, List<CardDataBase>>>(cards.text);
            card.GetComponent<CardJSONReader>().cardDictionary = cardDictionary;
            card.GetComponent<CardJSONReader>().cardID = temp;
            card.GetComponent<CardJSONReader>().UpdateData();

            var tempCard = sortedCollection.FirstOrDefault(c => c.cardID == temp);
            if (tempCard.holographic)
            {
                card.GetComponent<CardJSONReader>().renderer.material.SetInt("_Holographic", 1);
            }
            else
            {
                card.GetComponent<CardJSONReader>().renderer.material.SetInt("_Holographic", 0);
            }

            activeCards.Add(card);
            index++;


        }

        // Adjust card container size after adding cards (if necessary)
        int rows = Mathf.CeilToInt((float)sortedCollection.Count / cardPerRow);
        maxScrollHeight = rows * offsetPerRow; // Max scroll height based on number of rows
        minScrollHeight = -2000f; // Scroll can't go past the top
    }

    private void ClearActiveCards()
    {
        for (int i = activeCards.Count - 1; i >= 0; i--)
        {
            Destroy(activeCards[i].gameObject);
        }
        activeCards.Clear();
    }

    public void ToggleDisplay(bool isVisible)
    {
        if (isVisible)
        {
            OnEnable();
            isActive = true;
        }
        else
        {
            ClearActiveCards();
            OnDisable();
            isActive = false;
        }
    }

    private void Update()
    {
        if (gameManager.UserMenuState == GameManager.MenuState.COLLECTION)
        {
            // Check if the user clicks and starts dragging
            if (Input.GetMouseButtonDown(0))
            {
                mouseStartPos = Input.mousePosition;
                initialContainerPos = cardContainer.position;
                isDragging = true;
            }

            // If dragging, update the position based on mouse movement
            if (isDragging)
            {
                float deltaY = (Input.mousePosition.y - mouseStartPos.y) / 100;
                Vector3 newPosition = initialContainerPos + new Vector3(0, deltaY, 0);

                // Smooth movement by ensuring it doesn't instantly snap back
                newPosition.y = Mathf.Clamp(newPosition.y, -maxScrollHeight, 0); // Prevent going past the top or bottom

                cardContainer.position = Vector3.Lerp(cardContainer.position, newPosition, Time.deltaTime * 10f); // Smooth transition
            }

            // Stop dragging when the user releases the mouse button
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 1000))
                {
                    List<CollectionSet> sortedCollection = new List<CollectionSet>(inventory.GetCompleteCollection());

                    Debug.DrawRay(cameraTransform.position, cameraTransform.transform.forward, Color.red, 5.0f);
                    Debug.LogWarning("hit");
                    gameManager.ChangeMenuState(0);
                    menuManager.OpenMenu("Home");

                    // Get clicked Card
                    GameObject clickedCard = hit.transform.gameObject;
                    int clickedCardID = clickedCard.GetComponent<CardJSONReader>().cardID;
                    gameManager.MainCard.CardObject.GetComponent<CardJSONReader>().UpdateData(clickedCardID);

                    var tempCard = sortedCollection.FirstOrDefault(c => c.cardID == clickedCardID);
                    if (tempCard.holographic)
                    {
                        gameManager.MainCard.CardObject.GetComponent<CardJSONReader>().cardData.isHolo = true;
                        gameManager.MainCard.CardObject.GetComponent<CardJSONReader>().renderer.material.SetInt("_Holographic", 1);
                        inventory.AddCard(clickedCardID, true);
                    }
                    else
                    {
                        gameManager.MainCard.CardObject.GetComponent<CardJSONReader>().cardData.isHolo = false;
                        gameManager.MainCard.CardObject.GetComponent<CardJSONReader>().renderer.material.SetInt("_Holographic", 0);
                        inventory.AddCard(clickedCardID, false);
                    }
                }
                else
                {
                    Debug.DrawRay(cameraTransform.position, cameraTransform.transform.forward, Color.yellow, 5.0f);
                    Debug.LogWarning("No hit");
                    gameManager.ChangeMenuState(1);
                    menuManager.OpenMenu("Collection");
                }
            }
        }
    }
}
