using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotatableObject : MonoBehaviour
{
    [SerializeField] private InputAction pressed, axis;
    [SerializeField] private float speed = 1;
    [SerializeField] private bool resetRotation = false;
    private Vector2 rotation;
    private bool rotateAllowed;

    private Rigidbody rb;
    private Transform cameraPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraPosition = Camera.main.transform;

        pressed.Enable();
        axis.Enable();
        pressed.performed += _ => {  StartCoroutine(Rotate()); };
        pressed.canceled += _ => { rotateAllowed = false; }; //
        axis.performed += context => { rotation = context.ReadValue<Vector2>(); };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            resetRotation = !resetRotation;
        }
        
        if (!rotateAllowed && resetRotation)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(-90, 0, -90), Time.deltaTime * speed * 10); 
            transform.position = Vector3.Slerp(transform.position, new Vector3(0,1.61f,-8.17f), Time.deltaTime * speed * 10);
        }
    }

    private IEnumerator Rotate()
    {
        rotateAllowed = true;
        while (rotateAllowed)
        {
            // apply rotation
            rotation *= speed;
            transform.Rotate(cameraPosition.up, -rotation.x, Space.World);
            transform.Rotate(cameraPosition.right, rotation.y, Space.World);
            yield return null;
        }
    }
}
