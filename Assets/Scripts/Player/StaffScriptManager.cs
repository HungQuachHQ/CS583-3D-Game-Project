using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaffScriptManager : MonoBehaviour {
    public StaffDamage staffScript;

    public void EnableStaffCollider() {
        staffScript.EnableStaffCollider();
    }

    public void DisableStaffCollider() {
        staffScript.DisableStaffCollider();
    }
}
