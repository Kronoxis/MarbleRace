using UnityEngine;

public class scr_CheckChannelName : MonoBehaviour
{
	private scr_Animation m_Login;
	private scr_Animation[] m_Others;

	private void Start()
	{
		var animations = GameObject.Find("Canvas").GetComponentsInChildren<scr_Animation>();
		m_Others = new scr_Animation[animations.Length - 1];
		var i = 0;
		foreach (var anim in animations)
		{
			if (anim.name == "LoginPanel")
				m_Login = anim;
			else
			{
				m_Others[i] = anim;
				++i;
			}
		}

		InvokeRepeating("Check", 0.2f, 1.0f);
	}

	private void Check()
	{
		if (scr_InputManager.ChannelName == "" && !m_Login.IsShowing)
		{
			foreach (var anim in m_Others)
			{
				if (anim.IsShowing)
					anim.Hide(0);
			}
			m_Login.Show(0.5f);
		}
	}
}