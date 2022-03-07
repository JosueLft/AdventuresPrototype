using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerModel : MonoBehaviour {

    public static PlayerModel playerInstance;

    [Header("Entity")]
    public Entity entity;

    [Header("Player Regen System")]
    public bool regenHPEnabled = true;
    public float regenHPTime = 5.0f;
    public int regenHPValue = 5;
    public bool regenMPEnabled = true;
    public float regenMPTime = 5.0f;
    public int regenMPValue = 5;

    [Header("GameManager")]
    public GameManager manager;

    [Header("Player UI")]
    public Slider HPSlider;
    public Slider MPSlider;
    public Slider EXPSlider;
    public TextMeshProUGUI currentHP;
    public TextMeshProUGUI currentMP;
    public TextMeshProUGUI level;
    public TextMeshProUGUI levelPercentage;

    void Awake() {
        if(playerInstance = null) {
            playerInstance = this;
        }
    }

    void Start() {
        if(manager == null) {
            Debug.LogFormat("Você precisa anexar o game manager aqui no player");
            return;
        }
        InitStatus();
        //Iniciar o regenHealth
        StartCoroutine(RegenHealth());
        StartCoroutine(RegenMana());
    }

    void Update() {
        if(entity.dead) {
            return;
        }
        if(entity.currentHealth <= 0) {
            Die();
        }
        UpdateCurrentStatus();
    }

    void UpdateCurrentStatus() {
        // total
        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);
        HPSlider.maxValue = entity.maxHealth;
        MPSlider.maxValue = entity.maxMana;

        // current
        HPSlider.value = entity.currentHealth;
        currentHP.text = entity.currentHealth + " / " + entity.maxHealth;
        MPSlider.value = entity.currentMana;
        currentMP.text = entity.currentMana + " / " + entity.maxMana;

        level.text = "lvl. " + entity.level;
        EXPSlider.value = entity.exp;
        levelPercentage.text = entity.exp + "%";

        entity.physicsDamage = manager.CalculateDamage(entity, 3);
        entity.defense = manager.CalculateDefense(entity, 2);
    }

    void InitStatus() {
        entity.maxHealth = manager.CalculateHealth(entity);
        entity.maxMana = manager.CalculateMana(entity);
        entity.maxStamina = manager.CalculateStamina(entity);

        //entity.currentHealth = entity.maxHealth;
        entity.currentMana = entity.maxMana;
        entity.currentStamina = entity.maxStamina;

        HPSlider.maxValue = entity.maxHealth;
        HPSlider.value = entity.currentHealth;
        currentHP.text = entity.maxHealth.ToString();

        MPSlider.maxValue = entity.maxMana;
        MPSlider.value = entity.currentMana;
        currentMP.text = entity.maxMana.ToString();

        EXPSlider.value = entity.exp;
        levelPercentage.text = entity.exp + "%";
        level.text = "lvl. " + entity.level;

        entity.physicsDamage = manager.CalculateDamage(entity, 3);
        entity.defense = manager.CalculateDefense(entity, 2);
    }

    IEnumerator RegenHealth() {
        while(true) {
            if(regenHPEnabled) {
                if(entity.currentHealth < entity.maxHealth) {
                    entity.currentHealth = (entity.currentHealth + regenHPValue > entity.maxHealth)? entity.maxHealth : entity.currentHealth + regenHPValue;
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
                    entity.currentMana = (entity.currentMana + regenMPValue > entity.maxMana)? entity.maxMana : entity.currentMana + regenMPValue;
                    yield return new WaitForSeconds(regenMPTime);
                } else if(entity.currentMana > entity.maxMana) {
                    entity.currentMana = entity.maxMana;
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
        UpdateCurrentStatus();
        StopAllCoroutines();
    }
}