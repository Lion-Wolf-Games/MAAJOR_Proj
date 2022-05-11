using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Transform cam;

    private void Start() {
        cam = Camera.main.transform;
    }

    private void Update() {
        transform.forward = transform.position - cam.position;
    }
}
