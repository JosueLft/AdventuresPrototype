using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager inst;

    void Awake() {
        if(inst = null) {
            inst = this;
        }
    }

    public Int32 CalculateHealth(Entity entity) {
        Int32 result = (entity.vitality * 10) + (entity.level * 4) + 10;
        return result;
    }

    public Int32 CalculateMana(Entity entity) {
        // Formula (intelligence * 10) + (level * 4) + 5
        Int32 result = (entity.intelligence * 10) + (entity.level * 4) + 5;
        return result;
    }

    public Int32 CalculateStamina(Entity entity) {
        // Formula 50 + (vitality + agility) + (level * 2) + 5
        Int32 result = 50 + (entity.vitality + entity.agility) + (entity.level * 2) + 5;
        return result;
    }

    public Int32 CalculateDamage(Entity entity, int weaponDamage) {
        // Formula (str * 2) + (weaponDamage * 2) + (level * 3) + random (1-20)
        System.Random rmd = new System.Random();
        Int32 result = (entity.strength * 2) + (weaponDamage * 2) + (entity.level * 3) + rmd.Next(1, 20);
        return result;
    }

    public Int32 CalculateDefense(Entity entity, int armorDefense) {
        // Formula (endurence * 2) + (level * 3) + armorDefense
        Int32 result = (entity.vitality * 2) + (entity.level * 3) + armorDefense;
        return result;
    }
}
