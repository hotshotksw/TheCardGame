using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SceneViewCard : MonoBehaviour
{
    [SerializeField] private InputAction DragPressed, ScreenPos, RotatePressed, Axis;
    [SerializeField] private Vector3 originalPosition;
    [SerializeField] private Vector3 originalRotation;
    [SerializeField] private float speed = 1;

    private Vector2 rotation;
    private Vector3 curScreenPos;
    //private bool CanDrag;
    private bool CanRotate;
    private bool isDragging;
    private bool isRotating;
    private bool isClickedOn
    {
        get
        {
            Ray ray = camera.ScreenPointToRay(curScreenPos);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                return hit.transform == transform;
            }
            return false;
        }
    }

    private Rigidbody rb;
    private Camera camera;
    private Vector3 WorldPos
    {
        get
        {
            float z = camera.WorldToScreenPoint(transform.position).z;
            return camera.ScreenToWorldPoint(curScreenPos + new Vector3(0, 0, z));
        }
    }


    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        camera = Camera.main;

        DragPressed.Enable();
        RotatePressed.Enable();
        ScreenPos.Enable();
        Axis.Enable();

        RotatePressed.performed += _ => {  StartCoroutine(Rotate()); };
        RotatePressed.canceled += _ => { isRotating = false; }; //
        Axis.performed += context => { rotation = context.ReadValue<Vector2>(); };

        ScreenPos.performed += context => { curScreenPos = context.ReadValue<Vector2>(); };
        DragPressed.performed += _ => { if(isClickedOn) StartCoroutine(Drag()); };
        DragPressed.canceled += _ => { isDragging = false; };
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            CanRotate = !CanRotate;
        }
        
        if (!isRotating && !CanRotate)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(originalRotation.x, originalRotation.y, originalRotation.z), Time.deltaTime * speed * 10); 
            transform.position = Vector3.Slerp(transform.position, originalPosition, Time.deltaTime * speed * 10);
        }
    }
// new Vector3(0,1.61f,-8.17f)
    private IEnumerator Rotate()
    {
        isRotating = true;
        
        while (isRotating)
        {
            // apply rotation
            rotation *= speed;
            transform.Rotate(camera.transform.up, -rotation.x, Space.World);
            transform.Rotate(camera.transform.right, rotation.y, Space.World);
            yield return null;
        }
    }

    private IEnumerator Drag()
    {
        isDragging = true;
        Vector3 offset = transform.position - WorldPos;
        while(isDragging)
        {
            // dragging
            transform.position = WorldPos + offset;
            yield return null;
        }
        // drop
    }
}
