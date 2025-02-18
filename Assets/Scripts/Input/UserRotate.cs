using UnityEngine;

public class UserRotator : MonoBehaviour
{
    [SerializeField] float sensitivity = 10f;

    private float _yaw = 0f;
    private float _pitch = 0f;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            HandleInput();
            transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        }
        else
        {
            _yaw = 0f;
            _pitch = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * sensitivity);
        }
    }

    void HandleInput()
    {
        Vector2 inputDelta = Vector2.zero;

        Touch touch = Input.GetTouch(0);
        inputDelta = touch.deltaPosition;

        _yaw += -inputDelta.x * sensitivity * Time.deltaTime;
        //_pitch += inputDelta.y * sensitivity * Time.deltaTime;
    }
}