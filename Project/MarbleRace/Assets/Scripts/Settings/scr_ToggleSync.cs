using UnityEngine;
using UnityEngine.UI;

public class scr_ToggleSync : MonoBehaviour
{
	public enum Settings
	{
		RememberMe,
	}

	public Settings Setting;

	private Toggle m_Toggle;

	// Use this for initialization
	private void Start()
	{
		m_Toggle = GetComponentInChildren<Toggle>();

		// Event
		m_Toggle.onValueChanged.AddListener(ToggleCallback);

		// Set Placeholder and add Callbacks
		switch (Setting)
		{
			case Settings.RememberMe:
				SetToggle(scr_InputManager.Username != "");
				break;
		}
	}

	private void ToggleCallback(bool b)
	{
		switch (Setting)
		{
			case Settings.RememberMe:
				SetRememberMe(b);
				break;
		}
	}

	private void SetToggle(bool b)
	{
		m_Toggle.isOn = b;
	}

	private void SetRememberMe(bool b)
	{
		scr_InputManager.Username = b ? scr_InputManager.ChannelName : "";
	}
}