using System;
using System.Collections;
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

	private Button m_Button;

	// Use this for initialization
	private void Start()
	{
		foreach (var button in GetComponentsInChildren<Button>())
		{
			if (button.interactable)
			{
				m_Button = button;
				break;
			}
		}

		// Event
		m_Button.onClick.AddListener(ButtonCallback);

		// Set Placeholder and add Callbacks
		switch (Setting)
		{
			case Settings.StartKey:
				SetPlaceholder(scr_InputManager.KeyStart);
				scr_InputManager.OnKeyStartChanged = SetPlaceholder;
				break;
			case Settings.ZoomInKey:
				SetPlaceholder(scr_InputManager.KeyZoomIn);
				scr_InputManager.OnKeyZoomInChanged = SetPlaceholder;
				break;
			case Settings.ZoomOutKey:
				SetPlaceholder(scr_InputManager.KeyZoomOut);
				scr_InputManager.OnKeyZoomOutChanged = SetPlaceholder;
				break;
			case Settings.AddTestKey:
				SetPlaceholder(scr_InputManager.KeyAddTest);
				scr_InputManager.OnKeyAddTestChanged = SetPlaceholder;
				break;
			case Settings.MoveKey:
				SetPlaceholder(scr_InputManager.KeyMove);
				scr_InputManager.OnKeyMoveChanged = SetPlaceholder;
				break;
			case Settings.RecenterKey:
				SetPlaceholder(scr_InputManager.KeyRecenter);
				scr_InputManager.OnKeyRecenterChanged = SetPlaceholder;
				break;
		}
	}

	private void ButtonCallback()
	{
		m_Button.gameObject.GetComponentInChildren<Text>().text = "<i>Press a Key...</i>";
		StartCoroutine(WaitForKey());
	}

	private void SetPlaceholder(KeyCode k)
	{
		m_Button.gameObject.GetComponentInChildren<Text>().text = scr_InputManager.KeyCodeToString(k);
	}

	private IEnumerator WaitForKey()
	{
		while (!Input.anyKeyDown)
		{
			yield return 0;
		}

		var pressedKey = KeyCode.None;
		foreach (KeyCode kc in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(kc))
			{
				pressedKey = kc;
				break;
			}
		}

		SetKey(pressedKey);
	}

	private void SetKey(KeyCode key)
	{
		// Check for Duplicates
		var prevKey = KeyCode.None;
		var duplicateIdx = -1;
		for (var i = 0; i < scr_InputManager.Keys.Count; ++i)
		{
			if (scr_InputManager.Keys[i] == key)
			{
				prevKey = scr_InputManager.Keys[(int) Setting];
				duplicateIdx = i;
				break;
			}
		}

		scr_InputManager.Keys[(int) Setting] = key;
		SetPlaceholder(key);

		if (duplicateIdx != -1)
		{
			scr_InputManager.Keys[duplicateIdx] = prevKey;
			transform.parent.GetComponentsInChildren<scr_KeyButtonSync>()[duplicateIdx].SetPlaceholder(prevKey);
		}

		scr_InputManager.SetKeysFromList();
	}
}