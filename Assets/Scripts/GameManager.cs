using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct SceneCard
    {
        [SerializeField] public GameObject CardObject;
        [SerializeField] UserRotator Rotator;
        [SerializeField] Vector3 OriginalLocation;

        public SceneCard(GameObject Card) : this()
        {
            this.CardObject = Card;
            this.Rotator = Card.GetComponent<UserRotator>();
            this.OriginalLocation = Card.transform.position;
        }
        
        public void SetRotation(bool rotate)
        {
            Rotator.CanRotate = rotate;
        }

        public Vector3 GetOriginalLocation()
        {
            return this.OriginalLocation;
        }

        public void SetLocation(Vector3 Location, float speed)
        {
            if(Vector3.Distance(CardObject.transform.position, Location) > 0.01f)
            {
                CardObject.transform.position = Vector3.Lerp(CardObject.transform.position, Location, Time.deltaTime * speed);
            }
            else
            {
                CardObject.transform.position = Location;
            }
        }
    }

    public enum MenuState 
    {
        MAIN = 0,
        COLLECTION = 1,
        PACK = 2,
        OPEN_ONE = 3,
        OPEN_TEN = 4,
        START = 5
    }

    [SerializeField] public MenuState UserMenuState = MenuState.START;
    [SerializeField] GameObject CardHolder;
    [SerializeField] GameObject Pack;
    [SerializeField] SceneCard MainCard;
    [SerializeField] private Transform MainCameraPoint;
    List<SceneCard> cards = new List<SceneCard>();

    void Start()
    {
        foreach (Transform child in CardHolder.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.CompareTag("Card"))
            {
                cards.Add(new SceneCard(child.gameObject));
                cards[cards.Count-1].SetRotation(false);
            }
        }

        MainCard = cards[0];

        ChangeMenuState(0);
    }

    void Update()
    {
        switch (UserMenuState)
        {
            case MenuState.MAIN:
                MainCard.SetLocation(MainCameraPoint.transform.position, 2);
                SetObjectLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                SetObjectLocation(CardHolder.transform, new Vector3(0, -20, 5), 2);
                break;
            case MenuState.COLLECTION:
                MainCard.SetLocation(MainCard.GetOriginalLocation(), 0.25f);
                SetObjectLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                SetObjectLocation(CardHolder.transform, new Vector3(0, -20, 5), 2);
                break;
            case MenuState.PACK:
                MainCard.SetLocation(MainCard.GetOriginalLocation(), 0.5f);
                SetObjectLocation(Pack.transform, new Vector3(0, 0.75f, 0), 2);
                break;
            case MenuState.OPEN_ONE:
                MainCard.SetLocation(MainCameraPoint.transform.position, 2);
                SetObjectLocation(Pack.transform, new Vector3(0, -10, 0), 2);
                break;

            case MenuState.OPEN_TEN:
                SetObjectLocation(CardHolder.transform, new Vector3(0, 0.5f, 5), 2);
                SetObjectLocation(Pack.transform, new Vector3(0, -10, 5), 2);
                break;
        }
    }

    public void ChangeMenuState(int newState)
    {
        if (UserMenuState == (MenuState)newState) return;
        
        UserMenuState = (MenuState)newState;
        switch (UserMenuState)
        {
            case MenuState.MAIN:
                MainCard.SetRotation(true);
                CardHolder.GetComponent<UserRotator>().CanRotate = false;
                CardHolder.transform.position = new Vector3(0,-20,5);
                break;
            case MenuState.COLLECTION:
                MainCard.SetRotation(false);
                CardHolder.GetComponent<UserRotator>().CanRotate = false;
                CardHolder.transform.position = new Vector3(0,-20,5);
                break;
            case MenuState.PACK:
                MainCard.SetRotation(false);
                break;
            case MenuState.OPEN_ONE:
                Pack.GetComponent<Pack>().GetOneCard(cards[0]);
                MainCard.SetRotation(true);
                break;

            case MenuState.OPEN_TEN:
                Pack.GetComponent<Pack>().GetTenCards(cards);
                foreach (SceneCard card in cards)
                {
                    card.SetRotation(false);
                    card.CardObject.transform.position = card.GetOriginalLocation(); //SetLocation(card.GetOriginalLocation(), 100);
                }
                CardHolder.GetComponent<UserRotator>().CanRotate = true;
                break;
            default:
                break;
        }
    }

    private void SetObjectLocation(Transform objectTransform, Vector3 Location, float speed)
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
}
