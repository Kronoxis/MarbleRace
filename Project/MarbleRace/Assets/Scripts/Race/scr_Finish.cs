using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_Finish : MonoBehaviour {

    public Text m_CenterText = null;
    string m_WinnerName;
    string m_ColorHex;

    void Start()
    {
        scr_InputManager.IsFinished = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (scr_InputManager.IsFinished && !m_CenterText.gameObject.activeInHierarchy)
        {
            m_CenterText.text = "<color=#" + m_ColorHex + ">Winner:\n" + m_WinnerName + "</color>";
            m_CenterText.gameObject.SetActive(true);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.tag != "Marble") return;

        if (!scr_InputManager.IsFinished)
        {
            scr_InputManager.IsFinished = true;
            m_ColorHex = scr_Leaderboard.GetHexColor(other.name);
            if (m_ColorHex == null) m_ColorHex = "FFFFFF";
            m_WinnerName = other.name;
        }
    }
}
