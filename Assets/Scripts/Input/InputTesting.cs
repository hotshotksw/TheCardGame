using UnityEngine;

public class InputTesting : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _distanceFromTarget = 10f;
    [SerializeField] float sensitivity = 1000f;

    private float _yaw = 0f;
    private float _pitch = 0f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();

        Quaternion yawRotation = Quaternion.Euler(_pitch, _yaw, 0f);

        RotateCamera(yawRotation);
    }

    void HandleInput()
    {
        Vector2 inputDelta = Vector2.zero;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            inputDelta = touch.deltaPosition;
        }
        else if (Input.GetMouseButton(0))
        {
            inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        _yaw += -inputDelta.x * sensitivity * Time.deltaTime;
        _pitch += inputDelta.y * sensitivity * Time.deltaTime;
    }

    void RotateCamera(Quaternion rotation)
    {
        Vector3 positionOffset = rotation * new Vector3(0, 0, -_distanceFromTarget);
        //transform.position = _target.position + positionOffset;
        _target.transform.rotation = rotation;
    }
}

// Get Touch
    // Input.touchCount
    // Input.GetTouch(0); // Gets first touch input

// Touch Phases
    // TouchPhase.Began // When input starts
    // TouchPhase.Canceled // Systems cancels input
    // TouchPhase.Ended // User cancels input
    // TouchPhase.Moved // When input position changes
    // TouchPhase.Stationary // When input remains in same position

// Touch Position Examples
    // Vector2 touchPosition = touch.position;
    // Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

// 2D Touch Transform
    // this.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));

// 3D Touch Transform Using NavMesh
    // public NavMeshAgent navMeshAgent;
    // Ray ray = Camera.main.ScreenPointToRay(touch.position);
    // RaycastHit it;
    // if (Physics.Raycast(ray, out hit))
    // {
    //      navMeshAgent.destination = hit.point;
    // }

// Move Object using Raycast
    // private Vector3 newPosition;
    //
    //
    // newPosition = transform.position;
    //
    //
    // if (Input.touchCount > 0)
    // {
    //     Touch touch = Input.GetTouch(0);

    //     Ray ray = Camera.main.ScreenPointToRay(touch.position);
    //     RaycastHit hit;
    //     if (Physics.Raycast(ray, out hit))
    //     {
    //         newPosition = new Vector3(hit.point.x, hit.point.y + 0.5f, hit.point.z);
    //     }
    // }

    // if (this.transform.position != newPosition) 
    // {
    //     this.transform.position = Vector3.Slerp(this.transform.position, newPosition, Time.deltaTime * 10);
    // }