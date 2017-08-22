using UnityEngine;
using System.Collections;

public class scr_ApplicationSettings : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        Application.targetFrameRate = 60;
        Application.runInBackground = true;
	}
}
