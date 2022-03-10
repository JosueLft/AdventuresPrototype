using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class ShooterSystem : MonoBehaviour {

    public float rotateSpeed = 15;
    public float aimMaxDistance = 100;
    public GameObject aimCamera;
    public GameObject ammunitionPrefab;
    public Transform ammunitionSpawn;
    StarterAssetsInputs input;
    ThirdPersonController tpc;
    Animator animator;
    Camera mainCam;
    Vector3 aimPosition = Vector3.zero;

    void Start() {
        InitComponents();
    }

    void Update() {
        Aim();
        Shot();
    }

    void InitComponents() {
        input = GetComponent<StarterAssetsInputs>();
        tpc = GetComponent<ThirdPersonController>();
        animator = GetComponent<Animator>();
        mainCam = Camera.main;
    }

    void Aim() {
        Vector2 screenCenterPos = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = mainCam.ScreenPointToRay(screenCenterPos);
        if(Physics.Raycast(ray, out RaycastHit hit, aimMaxDistance)) {
            aimPosition = hit.point;
        } else {
            aimPosition = ray.origin + ray.direction * aimMaxDistance;
        }
        if(input.aim) {
            animator.SetLayerWeight(2, 1);
            tpc.SetRotateOnMove(false);
            aimCamera.SetActive(true);
            float yawCamera = mainCam.transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0, yawCamera, 0), Time.deltaTime * rotateSpeed);
        } else {
            animator.SetLayerWeight(2, 0);
            tpc.SetRotateOnMove(true);
            aimCamera.SetActive(false);
        }
    }

    void Shot() {
        if(input.HotKey_1 && input.aim) {
            input.HotKey_1 = false;
            Vector3 bulletDirection = (aimPosition - ammunitionSpawn.position).normalized;
            Instantiate(ammunitionPrefab, ammunitionSpawn.position, Quaternion.LookRotation(bulletDirection));
        }
    }
}
