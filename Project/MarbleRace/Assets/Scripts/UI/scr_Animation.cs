using System.Collections;
using UnityEngine;

public class scr_Animation : MonoBehaviour
{
	public enum EDirection
	{
		Up,
		Right,
		Down,
		Left
	}

	public EDirection Direction = EDirection.Up;
	public bool AnimateIn = true;
	public float Time = 1.0f;
	public float Delay = 1.0f;
	public bool PlayOnStart = true;
	public bool HideOnStart = true;

	protected RectTransform m_Panel;
	protected Vector2 m_OrigPos;

	private const int WIDTH = 960;
	private const int HEIGHT = 540;

	public bool IsShowing = false;
	private Coroutine m_Coroutine;

	private void Start()
	{
		m_Panel = GetComponent<RectTransform>();
		m_OrigPos = m_Panel.anchoredPosition;

		if (HideOnStart)
			m_Panel.anchoredPosition = GetTargetPos(Direction);

		if (PlayOnStart)
			Animate(Direction, AnimateIn, Time, Delay);
	}

	public void Show(float delay)
	{
		Animate(Direction, true, Time, delay);
	}

	public void Hide(float delay)
	{
		Animate(Direction, false, Time, delay);
	}

	public void Play(bool animateIn)
	{
		Animate(Direction, animateIn, Time, Delay);
	}

	public void Animate(EDirection direction, bool animateIn, float time, float delay)
	{
		IsShowing = animateIn;
		var startPos = GetTargetPos(direction);
		var endPos = m_OrigPos;
		if (!animateIn)
		{
			endPos = startPos;
			startPos = m_OrigPos;
		}
		if (m_Coroutine != null) StopCoroutine(m_Coroutine);
		m_Coroutine = StartCoroutine(AnimatePanel(startPos, endPos, time, delay));
	}

	protected Vector2 GetTargetPos(EDirection dir)
	{
		return m_OrigPos + new Vector2(
									   dir == EDirection.Left
										   ? WIDTH
										   : dir == EDirection.Right
											   ? -WIDTH
											   : 0,
									   dir == EDirection.Down
										   ? HEIGHT
										   : dir == EDirection.Up
											   ? -HEIGHT
											   : 0);
	}

	protected IEnumerator AnimatePanel(Vector2 startPos, Vector2 endPos, float time, float delay)
	{
		m_Panel.anchoredPosition = startPos;

		if (delay > 0.0001f)
			yield return new WaitForSeconds(delay);

		var t = 0.0f;
		while (t < 1.0f)
		{
			t += UnityEngine.Time.deltaTime / time;
			m_Panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
			yield return 0;
		}

		m_Panel.anchoredPosition = endPos;
		m_Coroutine = null;
	}
}