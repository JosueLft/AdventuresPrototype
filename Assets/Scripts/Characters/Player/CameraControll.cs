using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControll : MonoBehaviour {

    public Transform player;
    public float yOffset;
    public float sensibility;
    public float limitRotation;

    float rotX;
    float rotY;

    void Start() {
        
    }

    void Update() {
        float mouseX = Input.GetAxis("Mouse Y");
        float mouseY = Input.GetAxis("Mouse X");

        rotX -= mouseX * sensibility * Time.deltaTime;
        rotY += mouseY * sensibility * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -limitRotation, limitRotation);
        transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }

    void LateUpdate() {
        transform.position = player.position + player.up * yOffset;
    }
}