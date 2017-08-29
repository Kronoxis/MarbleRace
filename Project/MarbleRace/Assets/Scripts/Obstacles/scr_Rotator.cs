using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Rotator : MonoBehaviour {

    [Range(0, 360)]
    public float RotateSpeed = 45.0f;

    public bool TurnLeft = false;    
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, RotateSpeed * Time.deltaTime * (TurnLeft ? 1 : -1)));
	}
}
