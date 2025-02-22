using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform playerBody;
    public Transform cameraTransform;
    public float sensitivity = 0.2f;
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;
    private float xRotation = 0f;
    private Vector2 lastDragPosition;
    private bool isDragging = false;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                isDragging = true;
                lastDragPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastDragPosition;
                lastDragPosition = touch.position;

                playerBody.Rotate(Vector3.up * delta.x * sensitivity);

                xRotation -= delta.y * sensitivity;
                xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                lastDragPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
            }
            else if (Input.GetMouseButton(0) && isDragging)
            {
                Vector2 mousePos = (Vector2)Input.mousePosition;
                Vector2 delta = mousePos - lastDragPosition;
                lastDragPosition = mousePos;
                
                playerBody.Rotate(Vector3.up * delta.x * sensitivity);

                xRotation -= delta.y * sensitivity;
                xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
    }
}
