using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CombatEnemy : MonoBehaviour {
    [Header("Atributtes")]
    public float totalHealth = 100;
    public float attackDamage;
    public float movementSpeed;
    public float lookRadius;
    public float colliderRadius = 2;
    public float rotationSpeed;
    public float waitTimePatrol;
    
    [Header("Components")]
    private Animator anim;
    private CapsuleCollider capsule;
    private NavMeshAgent agent;

    [Header("Others")]
    private Transform player;
    private bool walking;
    private bool attacking;
    private bool waitFor;
    private bool hiting;
    public bool playerIsDead;

    [Header("Waypoints")]
    public List<Transform> waypoints = new List<Transform>();
    public int currentPathIndex;
    public float pathDistance;
    public bool isStop;

    void Start() {
        anim = GetComponent<Animator>();
        capsule = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentPathIndex = Random.Range(0, waypoints.Count);
        isStop = true;
    }

    void Update() {
        if(totalHealth > 0) {
            float distance = Vector3.Distance(player.position, transform.position);

            if(distance <= lookRadius) { //Raio de ação, detecção do player, perseguição
                agent.isStopped = false;
                if(!attacking) {
                    agent.SetDestination(player.position);
                    anim.SetBool("Walk Forward", true);
                }
                if(distance <= agent.stoppingDistance) { //Raio de attack
                    agent.isStopped = true;
                    StartCoroutine("Attack");
                    LookTarget();
                } else {
                    attacking = false;
                    agent.isStopped = false;
                }
            } else {
                anim.SetBool("Walk Forward", false);
                walking = false;
                attacking = false;
                MoveToWaypoint();
            }
        }
    }

    IEnumerator Attack() {
        if(!waitFor && !hiting && !playerIsDead) {
            waitFor = true;
            attacking = true;
            walking = false;
            anim.SetBool("Bite Attack", true);
            anim.SetBool("Walk Forward", false);
            yield return new WaitForSeconds(0.8f);
            GetPlayer();
            yield return new WaitForSeconds(1f);
            waitFor = false;
        }

        if(playerIsDead) {
            anim.SetBool("Bite Attack", false);
            anim.SetBool("Walk Forward", false);
            walking = false;
            attacking = false;
            agent.isStopped = true;
        }
    }

    void GetPlayer() {
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * colliderRadius), colliderRadius)) {
            if(c.gameObject.CompareTag("Player")) {
                c.gameObject.GetComponent<Player>().GetHit(attackDamage);
                playerIsDead = c.gameObject.GetComponent<Player>().isDead;
            }
        }
    }

    public void GetHit(float damage) {
        totalHealth -= damage;
        if(totalHealth > 0) {
            StopCoroutine("Attack");
            anim.SetTrigger("Take Damage");
            hiting = true;
            StartCoroutine("RecoveryFromHit");
        } else {
            anim.SetTrigger("Die");
        }
    }

    IEnumerator RecoveryFromHit() {
        yield return new WaitForSeconds(1f);
        anim.SetBool("Bite Attack", false);
        anim.SetBool("Walk Forward", false);
        hiting = false;
        waitFor = false;
    }

    void LookTarget() {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    void MoveToWaypoint() {
        if(waypoints.Count > 0) {
            float distance = Vector3.Distance(waypoints[currentPathIndex].position, transform.position);// checando a distancia entre o objeto e o proximo ponto]
            agent.destination = waypoints[currentPathIndex].position; // passando a posição do elemento de destino atual
            if(distance <= pathDistance) {
                // indo para o proximo ponto
                StartCoroutine("SortNewWaypoint");
            }
            if(isStop) {
               anim.SetBool("Walk Forward", true);
                walking = true; 
            }
        }
    }

    IEnumerator SortNewWaypoint() {
        if(isStop) {
            isStop = false;
            waitTimePatrol = Random.Range(0, 30);
            yield return new WaitForSeconds((float) waitTimePatrol);
            currentPathIndex = Random.Range(0, waypoints.Count);
            isStop = true;
        }
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}