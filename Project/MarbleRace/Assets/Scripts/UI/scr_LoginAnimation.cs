using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_LoginAnimation : MonoBehaviour
{
	private RectTransform m_Panel;

	private void Start()
	{
		var rt = GetComponent<RectTransform>();
		StartCoroutine(AnimatePanel(rt, rt.anchoredPosition, 2.0f, 1.0f));
	}

	private IEnumerator AnimatePanel(RectTransform rt, Vector2 origPos, float time, float delay)
	{
		rt.anchoredPosition = new Vector2(origPos.x, origPos.y - 540);

		if (delay > 0.0001f)
			yield return new WaitForSeconds(delay);

		var dt = 1.0f;
		while (dt > 0.0f)
		{
			dt -= Time.deltaTime / time;
			rt.anchoredPosition = new Vector2(origPos.x, origPos.y - 540 * dt);
			yield return 0;
		}
		yield return 0;
	}
}
