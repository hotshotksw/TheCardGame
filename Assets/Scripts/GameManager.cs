using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct SceneCard
    {
        [SerializeField] public GameObject CardObject;
        [SerializeField] Vector3 OriginalLocation;
        [SerializeField] bool InUse;

        public SceneCard(GameObject Card) : this()
        {
            this.CardObject = Card;
            OriginalLocation = Card.transform.position;
            InUse = false;
        }

        public Vector3 GetOriginalLocation()
        {
            return this.OriginalLocation;
        }
    }

    public enum MenuState {
        MAIN,
        PACK,
        OPENING
    }

    [SerializeField] private List<SceneCard> cards = new List<SceneCard>();
    [SerializeField] private GameObject CardHolder;
    [SerializeField] private GameObject Pack;
    [SerializeField] private Transform MainCameraPoint;
    [SerializeField] public MenuState menuState = MenuState.MAIN;
    
    void Start()
    {
        foreach (Transform child in CardHolder.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.CompareTag("Card"))
            {
                child.GetComponent<UserRotator>().CanRotate = false;
                cards.Add(new SceneCard(child.gameObject));

            }
            
        }
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            switch (menuState)
            {
                case MenuState.MAIN:
                    menuState = MenuState.PACK;
                    break;
                case MenuState.PACK:
                    menuState = MenuState.MAIN;
                    break;
            }
        }

        switch (menuState)
        {
            case MenuState.MAIN:
                cards[0].CardObject.GetComponent<UserRotator>().CanRotate = true;
                SetLocation(cards[0].CardObject.transform, MainCameraPoint.transform.position, 2);
                SetLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                break;
            case MenuState.PACK:
                cards[0].CardObject.GetComponent<UserRotator>().CanRotate = false;
                SetLocation(cards[0].CardObject.transform, cards[0].GetOriginalLocation(), 0.25f);
                SetLocation(Pack.transform, MainCameraPoint.transform.position, 2);

                if(Input.GetMouseButton(0))
                {
                    menuState = MenuState.OPENING;
                }
                break;
            case MenuState.OPENING:
                    
                    break;
            default:
                foreach( SceneCard card in cards)
                {
                    card.CardObject.GetComponent<UserRotator>().CanRotate = false;
                    SetLocation(card.CardObject.transform, card.GetOriginalLocation(), 0.25f);
                }
                break;
        }
    }

    private void SetLocation(Transform objectTransform, Vector3 Location, float speed)
    {
        objectTransform.position = Vector3.Lerp(objectTransform.position, Location, Time.deltaTime * speed);
    }

    public List<SceneCard> GetSceneCards()
    {
        return cards;
    }
}
