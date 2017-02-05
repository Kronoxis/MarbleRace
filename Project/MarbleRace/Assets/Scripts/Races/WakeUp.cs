using UnityEngine;
using System.Collections;

public class WakeUp : MonoBehaviour {

    Rigidbody m_Rigidbody;

	// Use this for initialization
	void Start () {
        m_Rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        m_Rigidbody.WakeUp();
	}
}
