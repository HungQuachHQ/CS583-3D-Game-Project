using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour {
    [SerializeField] private LayerMask interactablesLayer;

    public Transform cam;
    public float playerActivateDistance;
    bool active = false;

    private void Update() {
        RaycastHit hit;
        active = Physics.Raycast(cam.position, cam.TransformDirection(Vector3.forward), out hit, playerActivateDistance, interactablesLayer);

        if (Input.GetKeyDown(KeyCode.F) && active == true) {
            Debug.Log("Interaction detected");
        }
    }
}
