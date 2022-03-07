using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterModel : MonoBehaviour {
    [Header("Entity")]
    public Entity entity;
    public int baseHealth;
    public int baseMana;

    [Header("Monster Regen System")]
    public bool regenHPEnabled = true;
    public float regenHPTime = 5.0f;
    public int regenHPValue = 5;
    public bool regenMPEnabled = true;
    public float regenMPTime = 5.0f;
    public int regenMPValue = 5;

    [Header("GameManager")]
    public GameManager manager;

    [Header("Monster UI")]
    public Slider HPSlider;

    public ViewInformationsScreen informations;

    void Start() {
        if(manager == null) {
            Debug.LogFormat("Você precisa anexar o game manager aqui no monstro");
            return;
        }
        InitStatus();
        //Iniciar o regenHealth
        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());
    }

    void Update() {
        HPSlider.value = entity.currentHealth;
    }

    void InitStatus() {
        GenerateAttributes();
        informations.SetName(entity.name);

        entity.maxHealth = baseHealth + manager.CalculateHealth(entity);
        entity.maxMana = baseMana + manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);

        entity.currentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;

        HPSlider.maxValue = entity.maxHealth;
        HPSlider.value = HPSlider.maxValue;

        entity.physicsDamage = manager.CalculateDamage(entity, 3);
        entity.defense = manager.CalculateDefense(entity, 2);
    }

    IEnumerator RegenHealth() {
        while(true) {
            if(regenHPEnabled) {
                if(entity.currentHealth < entity.maxHealth) {
                    entity.currentHealth += regenHPValue;
                    yield return new WaitForSeconds(regenHPTime);
                } else {
                    yield return null;
                }
            } else {
                yield return null;
            }
        }
    }

    IEnumerator RegenMana() {
        while(true) {
            if(regenMPEnabled) {
                if(entity.currentMana < entity.maxMana) {
                    entity.currentMana += regenMPValue;
                    yield return new WaitForSeconds(regenMPTime);
                } else {
                    yield return null;
                }
            } else {
                yield return null;
            }
        }
    }

    public void Die() {
        entity.currentHealth = 0;
        entity.dead = true;
        StopAllCoroutines();
    }

    void GenerateAttributes() {
        entity.strength = Random.Range(3, 7);
        entity.vitality = Random.Range(3, 7);
        entity.intelligence = Random.Range(5, 8);
        entity.wisdom = Random.Range(5, 7);
        entity.agility = Random.Range(6, 9);
        entity.dexterity = Random.Range(5,9);
    }
}