using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Wind : MonoBehaviour {

    public float WindStrength = 1.0f;

    Vector3 m_Force;
    bool m_IsRotating = false;

	// Use this for initialization
	void Start () {
        m_Force = transform.right * WindStrength;
        m_IsRotating = (GetComponent<scr_Rotator>() != null);
	}

    void Update()
    {
        if (m_IsRotating)
            m_Force = transform.right * WindStrength;
    }

    void OnTriggerStay(Collider other)
    {
        // Ignore if Collider is not a Marble
        if (other.tag != "Marble") return;
        other.attachedRigidbody.AddForce(m_Force, ForceMode.VelocityChange);
    }
}
