using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager_OLD : MonoBehaviour
{
    [Serializable]
    public struct SceneCard
    {
        [SerializeField] public GameObject CardObject;
        public UserRotator Rotator;
        [SerializeField] Vector3 OriginalLocation;

        public SceneCard(GameObject Card) : this()
        {
            this.CardObject = Card;
            this.Rotator = Card.GetComponent<UserRotator>();
            this.OriginalLocation = Card.transform.position;
        }

        public Vector3 GetOriginalLocation()
        {
            return this.OriginalLocation;
        }
    }

    public enum MenuState {
        MAIN = 0,
        COLLECTION = 1,
        PACK = 2,
        OPEN_ONE = 3,
        OPEN_TEN = 4
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
                    ChangeMenuState(2);
                    break;
                default:
                    ChangeMenuState(0);
                    break;
            }
        }

        switch (menuState)
        {
            case MenuState.MAIN:
                if (OneShot == false)
                {
                    cards[0].Rotator.CanRotate = true;
                    CardHolder.GetComponent<UserRotator>().CanRotate = false;
                    CardHolder.transform.position = new Vector3(0,-20,5);
                    OneShot = true;
                }
                
                SetLocation(cards[0].CardObject.transform, MainCameraPoint.transform.position, 2);
                SetLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                break;

            case MenuState.COLLECTION:
                if (OneShot == false)
                {
                    cards[0].Rotator.CanRotate = false;
                    CardHolder.GetComponent<UserRotator>().CanRotate = false;
                    CardHolder.transform.position = new Vector3(0,-20,5);
                    OneShot = true;
                }
                SetLocation(cards[0].CardObject.transform, cards[0].GetOriginalLocation(), 0.25f);
                SetLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                SetLocation(CardHolder.transform, new Vector3(0, -20, 5), 2);
                break;

            case MenuState.PACK:
                if (OneShot == false)
                {
                    cards[0].Rotator.CanRotate = false;
                    OneShot = true;
                }
                
                SetLocation(cards[0].CardObject.transform, cards[0].GetOriginalLocation(), 0.5f);
                SetLocation(Pack.transform, new Vector3(0, 0.75f, 0), 2);
                break;

            case MenuState.OPEN_ONE:
                    if (OneShot == false)
                    {
                        //Pack.GetComponent<Pack>().GetOneCard(cards[0]);
                        cards[0].Rotator.CanRotate = true;
                        OneShot = true;
                    }

                    SetLocation(cards[0].CardObject.transform, MainCameraPoint.transform.position, 2);
                    SetLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                    break;

            case MenuState.OPEN_TEN:
                    if (OneShot == false)
                    {
                        //Pack.GetComponent<Pack>().GetTenCards(cards);
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
                    card.Rotator.CanRotate = false;
                    SetLocation(card.CardObject.transform, card.GetOriginalLocation(), 0.25f);
                }
                break;
        }
    }

    public void ChangeMenuState(int newState)
    {
        menuState = (MenuState)newState;
        OneShot = false;
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
