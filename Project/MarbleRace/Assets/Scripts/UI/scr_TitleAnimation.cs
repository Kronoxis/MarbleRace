using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_TitleAnimation : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(SequenceAnimateLeters());
	}

	private IEnumerator SequenceAnimateLeters()
	{
		var origPos = new List<Vector2>();
		for (var i = 0; i < transform.childCount; ++i)
		{
			var rt = transform.GetChild(i).GetComponent<RectTransform>();
			origPos.Add(rt.anchoredPosition);
			rt.anchoredPosition = new Vector2(origPos[i].x + 960, origPos[i].y);
		}

		for (var i = 0; i < transform.childCount; ++i)
		{
			var rt = transform.GetChild(i).GetComponent<RectTransform>();
			var delay = Random.Range(0.05f, 0.2f);
			StartCoroutine(AnimateLetter(rt, origPos[i], 1.0f, delay));
			yield return new WaitForSeconds(delay);
		}
	}

	private IEnumerator AnimateLetter(RectTransform rt, Vector2 origPos, float time, float delay)
	{
		rt.anchoredPosition = new Vector2(origPos.x + 960, origPos.y);

		if (delay > 0.0001f)
			yield return new WaitForSeconds(delay);

		var dt = 1.0f;
		while (dt > 0.0f)
		{
			dt -= Time.deltaTime / time;
			rt.anchoredPosition = new Vector2(origPos.x + 960 * dt, origPos.y);
			rt.rotation = Quaternion.Euler(new Vector3(0, 0, -360 * 4 * dt));
			yield return 0;
		}

		rt.anchoredPosition = origPos;
	}
}