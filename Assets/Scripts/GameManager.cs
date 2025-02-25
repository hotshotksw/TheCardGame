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
        OPEN_ONE,
        OPEN_TEN
    }

    [SerializeField] private List<SceneCard> cards = new List<SceneCard>();
    [SerializeField] private GameObject CardHolder;
    [SerializeField] private GameObject Pack;
    [SerializeField] private Transform MainCameraPoint;
    [SerializeField] public MenuState menuState = MenuState.MAIN;
    bool OneShot = false;    
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
                    OneShot = false;
                    break;
                default:
                    OneShot = false;
                    menuState = MenuState.MAIN;
                    break;
            }
        }

        switch (menuState)
        {
            case MenuState.MAIN:
                if (OneShot == false)
                {
                    cards[0].CardObject.GetComponent<UserRotator>().CanRotate = true;
                    CardHolder.GetComponent<UserRotator>().CanRotate = false;
                    CardHolder.transform.position = new Vector3(0,-20,5);
                    OneShot = true;
                }
                
                SetLocation(cards[0].CardObject.transform, MainCameraPoint.transform.position, 2);
                SetLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                break;
            case MenuState.PACK:
                if (OneShot == false)
                {
                    cards[0].CardObject.GetComponent<UserRotator>().CanRotate = false;
                    OneShot = true;
                }
                
                SetLocation(cards[0].CardObject.transform, cards[0].GetOriginalLocation(), 0.25f);
                SetLocation(Pack.transform, MainCameraPoint.transform.position, 2);

                if (Input.GetMouseButton(0))
                {
                    OneShot = false;
                    menuState = MenuState.OPEN_ONE;
                } else if (Input.GetMouseButton(2))
                {
                    OneShot = false;
                    menuState = MenuState.OPEN_TEN;
                }
                break;
            case MenuState.OPEN_ONE:
                    if (OneShot == false)
                    {
                        Pack.GetComponent<Pack>().GetOneCard(cards[0]);
                        OneShot = true;
                    }

                    SetLocation(cards[0].CardObject.transform, MainCameraPoint.transform.position, 2);
                    SetLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                    break;
            case MenuState.OPEN_TEN:
                    if (OneShot == false)
                    {
                        Pack.GetComponent<Pack>().GetTenCards(cards);
                        foreach (SceneCard card in cards)
                        {
                            card.CardObject.GetComponent<UserRotator>().CanRotate = false;
                            card.CardObject.transform.position = card.GetOriginalLocation();
                        }
                        CardHolder.GetComponent<UserRotator>().CanRotate = true;
                        OneShot = true;
                    }
                    SetLocation(CardHolder.transform, new Vector3(0, 0.5f, 5), 2);
                    SetLocation(Pack.transform, new Vector3(0, -10, 5), 2);
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
        if(Vector3.Distance(objectTransform.position, Location) > 0.01f)
        {
            objectTransform.position = Vector3.Lerp(objectTransform.position, Location, Time.deltaTime * speed);
        }
        else
        {
            objectTransform.position = Location;
        }
    }

    public List<SceneCard> GetSceneCards()
    {
        return cards;
    }
}
