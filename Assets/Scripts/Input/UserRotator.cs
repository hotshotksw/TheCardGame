using System;
using UnityEngine;

public class UserRotator : MonoBehaviour
{
    [SerializeField] float sensitivity = 10f;
    public bool CanRotate = false;

    private float _yaw = 0f;
    private float _pitch = 0f;

    void Update()
    {
        if (CanRotate == true)
        {
            if ((Input.touchCount > 0))
            {
                HandleInputMobile();
                transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            }
            else if (Input.GetMouseButton(1) == true)
            {
                HandleInputDesktop();
                transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            }
        }
        else
        {
            Debug.Log("RESETTING");
            _yaw = 0f;
            _pitch = 0f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, Time.deltaTime * sensitivity);
        }
    }

    void HandleInputMobile()
    {
        Vector2 inputDelta = Vector2.zero;

        Touch touch = Input.GetTouch(0);
        inputDelta = touch.deltaPosition;

        _yaw += -inputDelta.x * sensitivity * Time.deltaTime;
        //_pitch += inputDelta.y * sensitivity * Time.deltaTime;
    }

    void HandleInputDesktop()
    {
        Vector2 inputDelta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        _yaw += -inputDelta.x * (sensitivity * 50) * Time.deltaTime;
        //_pitch += inputDelta.y * (sensitivity * 50) * Time.deltaTime;
    }
}