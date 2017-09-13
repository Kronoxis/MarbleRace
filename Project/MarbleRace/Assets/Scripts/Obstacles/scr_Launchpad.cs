using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]

public class scr_Launchpad : MonoBehaviour {

    [Range(-90, 90)]
    [Tooltip("Left = -90\nUp = 0\nRight = 90")]
    public float LaunchAngle = 0.0f;
    public float LaunchStrength = 3.0f;

    Vector3 m_Force;

	// Use this for initialization
	void Start () {
        float angle = transform.rotation.eulerAngles.z * Mathf.PI / 180.0f;
        angle -= LaunchAngle * Mathf.PI / 180.0f;
        m_Force = new Vector3(Mathf.Cos(angle) * LaunchStrength, Mathf.Sin(angle) * LaunchStrength, 0);
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Marble") return;
        other.attachedRigidbody.AddForce(m_Force, ForceMode.Impulse);
    }
}