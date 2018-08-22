using UnityEngine;
using UnityEngine.UI;

public class scr_InputFieldSync : MonoBehaviour
{
	public enum Settings
	{
		Username,
		DataPath
	}

	public Settings Setting;

	private InputField m_InputField;

	// Use this for initialization
	private void Start()
	{
		m_InputField = GetComponentInChildren<InputField>();

		// Event
		m_InputField.onEndEdit.AddListener(InputFieldCallback);

		// Set Placeholder and add Callbacks
		switch (Setting)
		{
			case Settings.Username:
				SetPlaceholder(scr_InputManager.Username);
				scr_InputManager.OnUsernameChanged = SetPlaceholder;
				break;
			case Settings.DataPath:
				SetPlaceholder(scr_InputManager.DataPath);
				scr_InputManager.OnDataPathChanged = SetPlaceholder;
				break;
		}
	}

	private void InputFieldCallback(string s)
	{
		switch (Setting)
		{
			case Settings.Username:
				SetUsername(s);
				break;
			case Settings.DataPath:
				SetDataPath(s);
				break;
		}
	}

	private void SetPlaceholder(string s)
	{
		m_InputField.text = "";
		m_InputField.placeholder.GetComponent<Text>().text = s;
	}

	private void SetUsername(string s)
	{
		scr_InputManager.Username = s;
		SetPlaceholder(s);
	}

	private void SetDataPath(string s)
	{
		// Ignore empty inputs
		if (s.Length == 0)
			return;

		// Must be full path
		if (s.IndexOf(':') == -1)
		{
			SetPlaceholder("FULL PATH required!");
			return;
		}

		// Must end with /
		if (!s.EndsWith("/") && !s.EndsWith("\\"))
			s += "/";

		scr_InputManager.DataPath = s;
		SetPlaceholder(s);
	}
}