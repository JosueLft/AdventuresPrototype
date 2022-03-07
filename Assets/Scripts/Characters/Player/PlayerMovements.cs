using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(PlayerModel))]
public class PlayerMovements : MonoBehaviour {

    public float smoothRotTime;
    public bool isWalking;
    public bool isSlowRun;
    public bool isRun;
    public float rotateSpeed = 15;
    public float gravity = 10;
    public PlayerModel playerModel;

    [Header("UI")]
    public TextMeshProUGUI notificationsPopUp;

    private float turnSmoothVelocity;
    private float combatWeight;
    private CharacterController controller;
    private Transform cam;
    private Animator animator;
    Vector3 moveDirection;

    void Start() {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        animator = GetComponent<Animator>();
        combatWeight = 0;
    }

    void Update() {
        EnterCombatMode();
        ChangeSpeed();
        ChangeRunMode();
    }

    void FixedUpdate() {
        if(!playerModel.entity.dead) {
            if(!playerModel.entity.inCombat && !playerModel.entity.combatMode) {
                Move();
            } else {
                CombatMoviment();
            }
        }
    }

    void Move() {
        if(controller.isGrounded) { 
            float horizontal = Input.GetAxisRaw("Horizontal"); // capturando os eixos horizontais e verticais
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            if(direction.magnitude > 0) {
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                SetWalk(true);
            } else if(horizontal == 0 && vertical == 0) {
                SetWalk(false);
                moveDirection = Vector3.zero;
            }
        }
        ControllerWalk();
    }

    void CombatMoviment() {
        if(controller.isGrounded) { 
            float horizontal = Input.GetAxisRaw("Horizontal"); // capturando os eixos horizontais e verticais
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);

            if(direction.magnitude > 0) {
                float yawCamera = cam.rotation.eulerAngles.y;
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, yawCamera, 0), Time.deltaTime * rotateSpeed);
                float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

                moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;
                SetWalk(true);
            } else if(horizontal == 0 && vertical == 0) {
                SetWalk(false);
                moveDirection = Vector3.zero;
            }
        }
        ControllerWalk();
    }

    void ControllerWalk() {
        moveDirection.y -= gravity * Time.deltaTime;
        controller.Move(moveDirection * playerModel.entity.speed * Time.deltaTime);
    }

    void SetWalk(bool walk) {
        isWalking = walk;
        animator.SetBool("isWalking", isWalking);
        if(walk) {
            animator.SetBool("isSlowRun", isSlowRun);
        } else {
            animator.SetBool("isSlowRun", false);
        }
    }

    void ChangeSpeed(){
        if(isRun) {
            playerModel.entity.speed = 8;
        } else if(isSlowRun) {
            playerModel.entity.speed = 4;
        } else {
            playerModel.entity.speed = 2;
        }
    }

    void ChangeRunMode() {
        if(Input.GetKeyUp(KeyCode.LeftControl)) {
            isSlowRun = !isSlowRun;
            ChangeSpeed();
            animator.SetBool("isSlowRun", isSlowRun);
            StartCoroutine("FloatingText");
        }
    }

    IEnumerator FloatingText() {
        if(isSlowRun) {
            notificationsPopUp.text = "Modo corrida ativado";
        } else {
            notificationsPopUp.text = "Modo caminhada ativado";
        }
        notificationsPopUp.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        notificationsPopUp.gameObject.SetActive(false);
    }

    void EnterCombatMode() {
        // troca de postura caso não esteja em combate com monstros, caso contrario ficara em modo de combate por algum tempo
        if(Input.GetMouseButtonDown(2) && !playerModel.entity.inCombat) {
            playerModel.entity.combatMode = !playerModel.entity.combatMode;
            combatWeight = playerModel.entity.combatMode ? 1 : 0;
            animator.SetTrigger("Combat");
        }
        animator.SetLayerWeight(1, combatWeight);
        animator.SetBool("CombatMode", playerModel.entity.combatMode);
    }
}