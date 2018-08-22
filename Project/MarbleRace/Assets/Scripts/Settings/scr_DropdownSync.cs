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

	private Dropdown m_Dropdown;

    // Use this for initialization
	private void Start ()
    {
        m_Dropdown = GetComponentInChildren<Dropdown>();

        // Event
        m_Dropdown.onValueChanged.AddListener(DropdownCallback);

		// Set Placeholder and add Callbacks
		switch (Setting)
        {
            case Settings.SpriteSources:
                SetPlaceholder(scr_InputManager.SpriteSource);
	            scr_InputManager.OnSpriteSourceChanged = SetPlaceholder;
                break;
        }
    }

	private void DropdownCallback(int i)
    {
        switch (Setting)
        {
            case Settings.SpriteSources:
                scr_InputManager.SpriteSource = (scr_InputManager.ESpriteSources)System.Enum.Parse(typeof(scr_InputManager.ESpriteSources), m_Dropdown.options[i].text);
                break;
        }
    }

	private void SetPlaceholder(scr_InputManager.ESpriteSources e)
    {
        for (var i = 0; i < m_Dropdown.options.Count; ++i)
        {
	        if (m_Dropdown.options[i].text == e.ToString())
		        m_Dropdown.value = i;
        }
    }
}
