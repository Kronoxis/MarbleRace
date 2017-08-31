using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_DropdownSync : MonoBehaviour
{
    public enum Settings
    {
        SpriteSources
    }

    public Settings Setting;

    Dropdown m_Dropdown;

    // Use this for initialization
    void Start ()
    {
        m_Dropdown = GetComponentInChildren<Dropdown>();

        // Event
        var e = new Dropdown.DropdownEvent();
        e.AddListener(DropdownCallback);
        m_Dropdown.onValueChanged = e;

        // Set Placeholder
        switch (Setting)
        {
            case Settings.SpriteSources:
                SetPlaceholder(scr_InputManager.SpriteSource.ToString());
                break;
            default:
                break;
        }
    }

    void DropdownCallback(int i)
    {
        switch (Setting)
        {
            case Settings.SpriteSources:
                scr_InputManager.SpriteSource = (scr_InputManager.SpriteSources)(System.Enum.Parse(typeof(scr_InputManager.SpriteSources), m_Dropdown.options[i].text));
                break;
            default:
                break;
        }
    }

    void SetPlaceholder(string s)
    {
        for (int i = 0; i < m_Dropdown.options.Count; ++i)
            if (m_Dropdown.options[i].text == s)
                m_Dropdown.value = i;
    }
}
