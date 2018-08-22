using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_RememberMe : MonoBehaviour
{
	public void OnContinue()
	{
		scr_InputManager.Username = GetComponent<Toggle>().isOn ? scr_InputManager.ChannelName : "";
	}
}
