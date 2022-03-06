using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ViewInformationsScreen : MonoBehaviour {
    public TextMeshProUGUI name;
    public Canvas canvas;
    public static string entityName;

    void Start() {
    }

    void Update() {
        canvas.gameObject.transform.LookAt(Camera.main.transform);
        name.text = entityName;
    }

    public static void SetName(string n) {
        entityName = n;
    }
}