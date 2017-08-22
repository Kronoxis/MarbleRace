using UnityEngine;
using System.Collections;

public class scr_WakeUp : MonoBehaviour {

    Rigidbody m_Rigidbody;

	// Use this for initialization
	void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        // Don't let Rigidbodies sleep
        if (m_Rigidbody.IsSleeping())
            m_Rigidbody.WakeUp();
	}
}
