using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewInformationsScreen : MonoBehaviour {
    public TextMeshProUGUI monsterName;
    public Canvas canvas;
    string nameMonster;

    public static ViewInformationsScreen inst;

    void Awake() {
        if(inst = null) {
            inst = this;
        }
    }

    void Update() {
        canvas.gameObject.transform.LookAt(Camera.main.transform);
        monsterName.text = nameMonster;
    }

    public void SetName(string n) {
        nameMonster = n;
    }
}