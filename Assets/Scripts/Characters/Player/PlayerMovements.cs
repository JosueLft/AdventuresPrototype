using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovements : MonoBehaviour {

    public float speed;
    public float smoothRotTime;

    private float turnSmoothVelocity;
    private CharacterController controller;
    private Transform cam;

    void Start() {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
    }

    void FixedUpdate() {
        Move();
    }

    void Move() {
        float horizontal = Input.GetAxisRaw("Horizontal"); // capturando os eixos horizontais e verticais
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if(direction.magnitude > 0) {
            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }

    void CombatMoviment() {
        float horizontal = Input.GetAxisRaw("Horizontal"); // capturando os eixos horizontais e verticais
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if(direction.magnitude > 0) {
            //Vector3 moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

            //controller.Move(moveDirection * speed * Time.deltaTime);
        }
    }
}