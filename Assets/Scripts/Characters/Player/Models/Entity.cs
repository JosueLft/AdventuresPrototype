using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Entity {
    // caracteristicas
    [Header("Name")] public string name;
    [Header("Tribe")] public string tribe;
    [Header("Level")] public int level;
    [Header("Experience")] public int exp;

    [Header("Health")]
    public int currentHealth;
    public int maxHealth;

    [Header("Mana")] 
    public int currentMana;
    public int maxMana;

    [Header("Stamina")]
    public int currentStamina;
    public int maxStamina;

    // attributes
    [Header("Stats")]
    public int strength = 1; // afeta poder de atk fisico, força de carga, resistencia de impacto, 
    public int vitality = 1; // afeta quantidade de vida, defesa, estamina, resistencia alguns efeitos de debuff
    public int intelligence = 1; // afeta poder de atk magico, gasto de mana
    public int wisdom = 1; // afeta quantidade de mana, velocidade de casting, percepção
    public int agility = 1; // afeta velocidade de ataque, esquiva, 
    public int dexterity = 1; // afeta precisao, esquiva, dano, efeitos de furtividade, 

    // Properties
    [Header("Statistics")]
    public int physicsDamage = 1;
    public int magicDamage = 1;
    public int defense = 1;
    public float speed = 2f;
    public float attackSpeed = 1.0f; // atk por segundo
    public int evasion = 1;
    public float perception = 1.0f; // distancia de percepção contra inimigos furtivos
    public int precision = 1;
    public int stealth = 0;
    public int points = 0;

    [Header("Combat")]
    public float attackDistance = 0.5f;
    public float attackTimer = 1; // tempo para poder atkar
    public float cooldown = 2;
    public bool inCombat = false;
    public bool combatMode;
    public GameObject target;
    public bool combatCoroutine = false; //saber se a rotina de ataque esta funcionando
    public bool dead = false;
}