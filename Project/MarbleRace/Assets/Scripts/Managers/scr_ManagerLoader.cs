using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_ManagerLoader : MonoBehaviour
{
	// Use this for initialization
	void Awake ()
    {
        // Input Manager
        scr_InputManager.Instance();
	    scr_TwitchChat.Instance();
    }
}
