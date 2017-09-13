using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_Divider : MonoBehaviour {

    public float AutoAdvanceTime = 5.0f;
    public int TouchCountPerAdvance = 1;
    public List<GameObject> PathCaps = new List<GameObject>();
    public List<GameObject> Walls = new List<GameObject>();

    int m_CurrentOpenPath = 0;
    float m_AdvanceTime = 0.0f;
    int m_TouchCount = 0;

	// Use this for initialization
	void Start ()
    {
        PathCaps[0].SetActive(false);
        foreach (var wall in Walls) wall.SetActive(true);
        m_AdvanceTime = AutoAdvanceTime;
    }

    void Update()
    {
        if (AutoAdvanceTime > 0)
        {
            m_AdvanceTime -= Time.deltaTime;
            if (m_AdvanceTime <= 0)
            {
                Advance();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Ignore if Collider is not a Marble
        if (other.tag != "Marble") return;
        ++m_TouchCount;
        if (m_TouchCount > TouchCountPerAdvance)
            Advance();
    }

    void Advance()
    {
        // Update Index
        ++m_CurrentOpenPath;
        m_CurrentOpenPath %= PathCaps.Count;

        // Open Path
        for (int i = 0; i < PathCaps.Count; ++i)
        {
            // Caps
            if (i == m_CurrentOpenPath)
                PathCaps[i].SetActive(false);
            else
                PathCaps[i].SetActive(true);

            // Walls
            if (i >= Walls.Count) continue;
            if (i < m_CurrentOpenPath)
                Walls[i].SetActive(false);
            else
                Walls[i].SetActive(true);
        }

        // Reset Timer
        m_AdvanceTime = AutoAdvanceTime;

        // Reset TouchCount
        m_TouchCount = 0;
    }
}
