using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_KeyButtonSync : MonoBehaviour
{
    public enum Settings
    {
        StartKey,
        ZoomInKey,
        ZoomOutKey,
        AddTestKey,
        MoveKey,
        RecenterKey
    }

    public Settings Setting;

    Button m_Button;

	// Use this for initialization
	void Start ()
    {
        foreach (var button in GetComponentsInChildren<Button>())
            if (button.interactable) { m_Button = button; break; }

        // Event
        var e = new Button.ButtonClickedEvent();
        e.AddListener(ButtonCallback);
        m_Button.onClick = e;

        // Set Placeholder
        switch (Setting)
        {
            case Settings.StartKey:
                SetPlaceholder(scr_InputManager.KeyCodeToString(scr_InputManager.Key_Start));
                break;
            case Settings.ZoomInKey:
                SetPlaceholder(scr_InputManager.KeyCodeToString(scr_InputManager.Key_ZoomIn));
                break;
            case Settings.ZoomOutKey:
                SetPlaceholder(scr_InputManager.KeyCodeToString(scr_InputManager.Key_ZoomOut));
                break;
            case Settings.AddTestKey:
                SetPlaceholder(scr_InputManager.KeyCodeToString(scr_InputManager.Key_AddTest));
                break;
            case Settings.MoveKey:
                SetPlaceholder(scr_InputManager.KeyCodeToString(scr_InputManager.Key_Move));
                break;
            case Settings.RecenterKey:
                SetPlaceholder(scr_InputManager.KeyCodeToString(scr_InputManager.Key_Recenter));
                break;
        }
    }

    void ButtonCallback()
    {
        m_Button.gameObject.GetComponentInChildren<Text>().text = "<i>Press a Key...</i>";
        StartCoroutine(WaitForKey());
    }

    void SetPlaceholder(string s)
    {
        m_Button.gameObject.GetComponentInChildren<Text>().text = s;
    }

    IEnumerator WaitForKey()
    {
        while (!Input.anyKeyDown)
            yield return 0;

        KeyCode pressedKey = KeyCode.None;
        foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
            if (Input.GetKeyDown(kc)) { pressedKey = kc; break; }

        SetKey(pressedKey);
    }

    void SetKey(KeyCode key)
    {
        // Check for Duplicates
        KeyCode prevKey = KeyCode.None;
        int duplicateIdx = -1;
        for (int i = 0; i < scr_InputManager.Keys.Count; ++i) 
        {
            if (scr_InputManager.Keys[i] == key)
            {
                prevKey = scr_InputManager.Keys[(int)Setting];
                duplicateIdx = i;
                break;
            }
        }

        scr_InputManager.Keys[(int)Setting] = key;
        SetPlaceholder(scr_InputManager.KeyCodeToString(key));

        if (duplicateIdx != -1)
        {
            scr_InputManager.Keys[duplicateIdx] = prevKey;
            transform.parent.GetComponentsInChildren<scr_KeyButtonSync>()[duplicateIdx].SetPlaceholder(scr_InputManager.KeyCodeToString(prevKey));
        }

        scr_InputManager.SetKeysFromList();
    }
}
