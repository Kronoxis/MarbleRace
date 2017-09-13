using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_SliderSync : MonoBehaviour
{
    public enum Settings
    {
        DownloadDelay,
        UpdateDelay,
        ReleaseTimer,
        MoveSpeed,
        ZoomSpeed
    }

    public Settings Setting;

    Slider m_Slider;
    InputField m_InputField;

	// Use this for initialization
	void Start ()
    {
        m_Slider = GetComponentInChildren<Slider>();
        m_InputField = GetComponentInChildren<InputField>();

        // Event
        var eS = new Slider.SliderEvent();
        eS.AddListener(SliderCallback);
        m_Slider.onValueChanged= eS;
        var eI = new InputField.SubmitEvent();
        eI.AddListener(InputFieldCallback);
        m_InputField.onEndEdit = eI;
        m_InputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return InputFieldValidate(addedChar); };

        // Set Placeholder
        switch (Setting)
        {
            case Settings.DownloadDelay:
                SetPlaceholder(scr_InputManager.DownloadDelay);
                break;
            case Settings.UpdateDelay:
                SetPlaceholder(scr_InputManager.UpdateDelay);
                break;
            case Settings.ReleaseTimer:
                SetPlaceholder(scr_InputManager.ReleaseTimer);
                break;
            case Settings.MoveSpeed:
                SetPlaceholder(scr_InputManager.MoveSpeed);
                break;
            case Settings.ZoomSpeed:
                SetPlaceholder(scr_InputManager.ZoomSpeed);
                break;
        }
    }

    char InputFieldValidate(char c)
    {
        // Change comma into dot
        if (c == ',' || c == '.')
            return '.';

        // Ignore everything but letters
        if (!char.IsDigit(c))
            return '\0';

        return c;
    }

    void SliderCallback(float f)
    {
        switch (Setting)
        {
            case Settings.DownloadDelay:
                scr_InputManager.DownloadDelay = f;
                break;
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

    void InputFieldCallback(string s)
    {
        // Ignore empty inputs
        if (s.Length == 0)
            return;

        // Only valid with one dot. Ignore from second dot on
        if (s.IndexOf('.') != s.LastIndexOf('.'))
        {
            int firstDot = s.IndexOf('.');
            s = s.Substring(0,  firstDot + 1 + s.Substring(firstDot + 1).IndexOf('.'));
            // Length of 1 means only dot. Invalid
            if (s.Length == 1)
                return;
        }
        float f = float.Parse(s);

        // Clamp value
        f = Mathf.Clamp(f, m_Slider.minValue, m_Slider.maxValue);

        SliderCallback(f);
    }

    void SetPlaceholder(float f)
    {
        m_Slider.value = f;
        m_InputField.text = "";
        m_InputField.placeholder.GetComponent<Text>().text = f.ToString();
    }

    void SetPlaceholderError(string s)
    {
        m_InputField.text = "";
        m_InputField.placeholder.GetComponent<Text>().text = s;
    }
}
