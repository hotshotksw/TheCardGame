using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CardPack : MonoBehaviour
{
    [SerializeField] private InputAction pressed;
    [SerializeField] private Vector3 EndLocation;
    [SerializeField] private float Speed;

    private Vector3 OriginalLocation;
    private bool opened = true;
    private Test_CardManager CardManager;
    
    void Awake()
    {
        OriginalLocation = transform.position;
        CardManager = GetComponent<Test_CardManager>();

        pressed.Enable();
        pressed.performed += _ => {StartCoroutine(GetCard()); };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)){
            opened = false;
        }
        
        if (opened == false) transform.position = Vector3.Slerp(transform.position, EndLocation, Time.deltaTime * Speed * 10);
        else transform.position = Vector3.Slerp(transform.position, OriginalLocation, Time.deltaTime * Speed * 10);
    }

    private IEnumerator GetCard()
    {
        if (!opened)
        {
            opened = true;
            CardManager.DrawNewCard();
        }
        yield return null;
    }
}
