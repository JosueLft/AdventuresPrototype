using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float totalHealth;
    public float speed;
    public float smoothRotTime;
    public float gravity = 10;
    public float colliderRadius;
    public int damage = 20;
    public List<Transform> enemyList = new List<Transform>();
    private float turnSmoothVelocity;
    private CharacterController controller;
    private Transform cam;
    private Animator anim;
    private bool isWalking;
    private bool waitFor;
    private bool isHitting;
    public bool isDead;
    Vector3 moveDirection;

    void Start() {
        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
    }

    void Update() {
        if(!isDead) {
            Move();
            GetMouseInput();
        }
    }

    void Move() {
        if(controller.isGrounded) { // verificação de toque ao chao
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            Vector3 direction = new Vector3(horizontal, 0f, vertical);
            if(direction.magnitude > 0) { // retorna um valor resultado do vetor

                if(!anim.GetBool("Attack")) {
                    // variavel local que armazena a rotação e o angulo de visualização da camera
                    float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
                    // variavel local que armazena a rotacao porem mais suave // retorna um valor baseado em angulo suavizado
                    float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, smoothRotTime);

                    // passa a rotação suave ao personagem
                    transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                    // armazena direcao com base na direção da camera// passando uma rotação baseada na direção e direção da camera e fazendo com que o player sempre ande na direção que a camera esta olhando
                    moveDirection = Quaternion.Euler(0f, angle, 0f) * Vector3.forward * speed;

                    anim.SetInteger("Transition", 1);
                    isWalking = true;
                } else {
                    anim.SetBool("Walking", false);
                    moveDirection = Vector3.zero;
                }
            } else if(isWalking) {
                anim.SetBool("Walking", false);
                anim.SetInteger("Transition", 0);
                moveDirection = Vector3.zero;
                isWalking = false;
            }
        }

        moveDirection.y -= gravity * Time.deltaTime;
        // move o personagem
        controller.Move(moveDirection * Time.deltaTime);
    }

    void GetMouseInput() {
        if(controller.isGrounded) {
            if(Input.GetMouseButtonDown(0)) {
                if(anim.GetBool("Walking")) {
                    anim.SetBool("Walking", false);
                    anim.SetInteger("Transition", 0);
                }
                if(!anim.GetBool("Walking")) {
                    StartCoroutine("Attack");
                }
            }
        }
    }

    IEnumerator Attack() {
        if(!waitFor && !isHitting) {
            waitFor = true;
            anim.SetBool("Attack", true);
            anim.SetInteger("Transition", 2);
            yield return new WaitForSeconds(0.4f);

            GetEnemiesList();
            foreach(Transform enemy in enemyList) {
                CombatEnemy e = enemy.GetComponent<CombatEnemy>();
                if(e != null) {
                    e.GetHit(damage);
                }
            }

            yield return new WaitForSeconds(1f);

            anim.SetInteger("Transition", 0);
            anim.SetBool("Attack", false);
            waitFor = false;
        }
    }

    void GetEnemiesList() {
        enemyList.Clear();
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius)) {
            if(c.gameObject.CompareTag("Enemy")) {
                enemyList.Add(c.transform);
            }
        }
    }

    public void GetHit(float damage) {
        totalHealth -= damage;
        if(totalHealth > 0) {
            // Player ainda está vivo
            StopCoroutine("Attack");
            anim.SetInteger("Transition", 3);
            isHitting = true;
            StartCoroutine("RecoveryFromHit");
        } else {
            // Player morre
            isDead = true;
            anim.SetTrigger("Die");
        }
    }

    IEnumerator RecoveryFromHit() {
        yield return new WaitForSeconds(1f);
        anim.SetInteger("Transition", 0);
        isHitting = false;
        anim.SetBool("Attack", false);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, colliderRadius);
    }
}