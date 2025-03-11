using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Serializable]
    public struct SceneCard
    {
        [SerializeField] GameObject CardObject;
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
        OPEN_TEN = 4
    }

    [SerializeField] public MenuState UserMenuState = MenuState.MAIN;
    [SerializeField] GameObject CardHolder;
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
    }

    void Update()
    {
        switch (UserMenuState)
        {
            case MenuState.MAIN:
                MainCard.SetLocation(MainCameraPoint.transform.position, 2);
                break;
            
            case MenuState.PACK:
                break;
        }
    }

    public void ChangeMenuState(int newState)
    {
        UserMenuState = (MenuState)newState;
    }

    
}
