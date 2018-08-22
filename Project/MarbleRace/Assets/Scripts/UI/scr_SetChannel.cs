using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InputField))]
public class scr_SetChannel : MonoBehaviour
{
	private InputField m_InputField;

	private void Start()
	{
		m_InputField = GetComponent<InputField>();
		m_InputField.onEndEdit.AddListener(OnEndEdit);
	}

	public void OnEndEdit(string s)
	{
		scr_InputManager.ChannelName = s;
	}
}