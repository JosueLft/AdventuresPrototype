using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using System;

[RequireComponent(typeof(PlayerModel))]
public class CombatMode : MonoBehaviour {

    public float weaponRange;
    public PlayerModel playerModel;
    public Animator animator;
    public List<Transform> enemiesList = new List<Transform>();
    private bool isWalking;
    private bool waitFor;
    private bool isHitting;
    StarterAssetsInputs input;
    ThirdPersonController tpc;

    void Start() {
        InitComponents();
    }

    void Update() {
        BasicAttack();
    }

    void InitComponents() {
        input = GetComponent<StarterAssetsInputs>();
        animator = GetComponent<Animator>();
        tpc = GetComponent<ThirdPersonController>();
    }

    void BasicAttack() {
        if(input.HotKey_1) {
            StartCoroutine("SwordAttack");
        }
    }

    IEnumerator SwordAttack() {
        if(!waitFor && !isHitting) {
            waitFor = true;
            animator.SetInteger("SwordSkill", 1);
            animator.SetLayerWeight(3, 1);
            yield return new WaitForSeconds(0.4f);

            GetEnemiesList();

            foreach(Transform enemy in enemiesList) {
                CombatEnemy e = enemy.GetComponent<CombatEnemy>();
                if(e != null) {
                    e.GetHit(playerModel.entity.physicsDamage);
                }
            }
            yield return new WaitForSeconds(1f);
            animator.SetInteger("SwordSkill", 0);
            animator.SetLayerWeight(3, 0);
            waitFor = false;
        }
    }

    void GetEnemiesList() {
        enemiesList.Clear();
        foreach(Collider c in Physics.OverlapSphere((transform.position + transform.forward * weaponRange), weaponRange)) {
            if(c.gameObject.CompareTag("Enemy")) {
                enemiesList.Add(c.transform);
            }
        }
    }

    public void GetHit(int damage) {
        Debug.LogFormat("Dano gerado: {0}", damage);
        int damageReceived = (damage - playerModel.entity.defense) > 0 ? damage - playerModel.entity.defense : 0;
        Debug.LogFormat("Dano Recebido: {0}", damageReceived);
        playerModel.entity.currentHealth -= damageReceived;
        if(playerModel.entity.currentHealth > 0) {
            StopCoroutine("SwordAttack");
            animator.SetTrigger("HitReaction");
            isHitting = true;
            StartCoroutine("RecoveryFromHit");
        } else {
            animator.SetTrigger("Die");
            playerModel.Die();
        }
    }

    IEnumerator RecoveryFromHit() {
        animator.SetInteger("SwordSkill", 0);
        yield return new WaitForSeconds(1f);
        isHitting = false;
        waitFor = false;
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + transform.forward, weaponRange);
    }
}