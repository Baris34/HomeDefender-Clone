using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Yatay dönme yapacağımız Player veya pivot transform
    public Transform playerBody;

    // Dikey dönmeyi yapacak kamera transform
    public Transform cameraTransform;

    // Dönme hassasiyeti (hem PC hem mobil için)
    public float sensitivity = 0.2f;

    // Dikey bakış açısını sınırlandırmak için 
    public float minVerticalAngle = -80f;
    public float maxVerticalAngle = 80f;

    // Bu değer, kameranın o anki dikey rotasyon miktarını tutar
    private float xRotation = 0f;

    // Dokunma veya fare pozisyonunu kaydetmek için
    private Vector2 lastDragPosition;
    private bool isDragging = false;

    void Update()
    {
        // 1) Mobil Dokunma Varsa Onu Oku
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

                // X ekseni: player gövdesini sağ-sol döndür
                playerBody.Rotate(Vector3.up * delta.x * sensitivity);

                // Y ekseni: kameraTransform’u yukarı-aşağı çevir
                xRotation -= delta.y * sensitivity;
                xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
        // 2) Dokunma Yoksa PC Fare Girişi Kontrol Et
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

                // X ekseni: player gövdesini sağ-sol döndür
                playerBody.Rotate(Vector3.up * delta.x * sensitivity);

                // Y ekseni: kameraTransform’u yukarı-aşağı çevir
                xRotation -= delta.y * sensitivity;
                xRotation = Mathf.Clamp(xRotation, minVerticalAngle, maxVerticalAngle);
                cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            }
        }
    }
}
