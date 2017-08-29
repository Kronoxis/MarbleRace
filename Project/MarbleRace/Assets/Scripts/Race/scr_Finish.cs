using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_Finish : MonoBehaviour {

    Text m_CenterText = null;
    bool m_IsFinished = false;
    string m_WinnerName;
    string m_ColorHex;

    // Use this for initialization
    void Start()
    {
        m_CenterText = GameObject.FindGameObjectWithTag("CenterText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_IsFinished && !m_CenterText.gameObject.activeInHierarchy)
        {
            m_CenterText.text = "<color=#" + m_ColorHex + ">Winner:\n" + m_WinnerName + "</color>";
            m_CenterText.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Marble") return;

        if (!m_IsFinished)
        {
            m_IsFinished = true;
            m_ColorHex = scr_Leaderboard.GetHexColor(other.name);
            if (m_ColorHex == null) m_ColorHex = "FFFFFF";
            m_WinnerName = other.name;
        }
    }
}
