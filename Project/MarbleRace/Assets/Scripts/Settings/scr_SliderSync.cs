using UnityEngine;
using UnityEngine.UI;

public class scr_SliderSync : MonoBehaviour
{
	public enum Settings
	{
		UpdateDelay,
		ReleaseTimer,
		MoveSpeed,
		ZoomSpeed
	}

	public Settings Setting;

	private Slider m_Slider;
	private InputField m_InputField;

	// Use this for initialization
	private void Start()
	{
		m_Slider = GetComponentInChildren<Slider>();
		m_InputField = GetComponentInChildren<InputField>();

		// Event
		m_Slider.onValueChanged.AddListener(SliderCallback);
		m_InputField.onEndEdit.AddListener(InputFieldCallback);
		m_InputField.onValidateInput += (input, charIndex, addedChar) => InputFieldValidate(addedChar);

		// Set Placeholder and add Callbacks
		switch (Setting)
		{
			case Settings.UpdateDelay:
				SetPlaceholder(scr_InputManager.UpdateDelay);
				scr_InputManager.OnUpdateDelayChanged = SetPlaceholder;
				break;
			case Settings.ReleaseTimer:
				SetPlaceholder(scr_InputManager.ReleaseTimer);
				scr_InputManager.OnReleaseTimerChanged = SetPlaceholder;
				break;
			case Settings.MoveSpeed:
				SetPlaceholder(scr_InputManager.MoveSpeed);
				scr_InputManager.OnMoveSpeedChanged = SetPlaceholder;
				break;
			case Settings.ZoomSpeed:
				SetPlaceholder(scr_InputManager.ZoomSpeed);
				scr_InputManager.OnZoomSpeedChanged = SetPlaceholder;
				break;
		}
	}

	private char InputFieldValidate(char c)
	{
		// Change comma into dot
		if (c == ',' || c == '.')
			return '.';

		// Ignore everything but letters
		if (!char.IsDigit(c))
			return '\0';

		return c;
	}

	private void SliderCallback(float f)
	{
		switch (Setting)
		{
			case Settings.UpdateDelay:
				scr_InputManager.UpdateDelay = f;
				break;
			case Settings.ReleaseTimer:
				scr_InputManager.ReleaseTimer = f;
				break;
			case Settings.MoveSpeed:
				scr_InputManager.MoveSpeed = f;
				break;
			case Settings.ZoomSpeed:
				scr_InputManager.ZoomSpeed = f;
				break;
		}

		SetPlaceholder(f);
	}

	private void InputFieldCallback(string s)
	{
		// Ignore empty inputs
		if (s.Length == 0)
			return;

		// Only valid with one dot. Ignore from second dot on
		if (s.IndexOf('.') != s.LastIndexOf('.'))
		{
			var firstDot = s.IndexOf('.');
			s = s.Substring(0, firstDot + 1 + s.Substring(firstDot + 1).IndexOf('.'));
			// Length of 1 means only dot. Invalid
			if (s.Length == 1)
				return;
		}
		var f = float.Parse(s);

		// Clamp value
		f = Mathf.Clamp(f, m_Slider.minValue, m_Slider.maxValue);

		SliderCallback(f);
	}

	private void SetPlaceholder(float f)
	{
		m_Slider.value = f;
		m_InputField.text = "";
		m_InputField.placeholder.GetComponent<Text>().text = f.ToString();
	}

	private void SetPlaceholderError(string s)
	{
		m_InputField.text = "";
		m_InputField.placeholder.GetComponent<Text>().text = s;
	}
}