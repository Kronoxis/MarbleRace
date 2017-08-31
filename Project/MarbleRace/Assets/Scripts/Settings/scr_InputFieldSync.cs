using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_InputFieldSync : MonoBehaviour
{
    public enum Settings
    {
        URL,
        DataPath
    }

    public Settings Setting;

    InputField m_InputField;

	// Use this for initialization
	void Start ()
    {
        m_InputField = GetComponentInChildren<InputField>();

        // Event
        var e = new InputField.SubmitEvent();
        e.AddListener(InputFieldCallback);
        m_InputField.onEndEdit = e;

        // Set Placeholder
        switch (Setting)
        {
            case Settings.URL:
                SetPlaceholder(scr_InputManager.URL);
                break;
            case Settings.DataPath:
                SetPlaceholder(scr_InputManager.DataPath);
                break;
        }
	}

    void InputFieldCallback(string s)
    {
        switch (Setting)
        {
            case Settings.URL:
                SetUrl(s);
                break;
            case Settings.DataPath:
                SetDataPath(s);
                break;
        }
    }

    void SetPlaceholder(string s)
    {
        m_InputField.text = "";
        m_InputField.placeholder.GetComponent<Text>().text = s;
    }

    void SetUrl(string s)
    {
        // Ignore empty inputs
        if (s.Length == 0)
            return;

        // HTTPS not supported
        if (s.ToLower().StartsWith("https://"))
        {
            SetPlaceholder("HTTPS is NOT supported!");
            return;
        }

        scr_InputManager.URL = s;
        SetPlaceholder(s);
    }

    void SetDataPath(string s)
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
