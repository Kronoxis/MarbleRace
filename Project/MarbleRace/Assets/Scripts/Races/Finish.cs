using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Finish : MonoBehaviour {

    private bool m_IsFinished = false;
    private bool m_IsNotified = false;
    private string m_WinnerName;

    private Text m_CenterText;

    private SaveLeaderboard m_SaveScript;

    // Use this for initialization
    void Start () {
        m_CenterText = GameObject.Find("CenterText").GetComponent<Text>();
        m_SaveScript = GameObject.Find("ScriptManager").GetComponent<SaveLeaderboard>();
	}
	
	// Update is called once per frame
	void Update () {
        if (m_IsFinished && !m_IsNotified)
        {
            Debug.Log("(On GUI): " + m_WinnerName + " has finished!");
            m_CenterText.text = "Winner:\n" + m_WinnerName;
            m_CenterText.enabled = true;
            m_IsNotified = true;
            m_SaveScript.Save();
        }
	}

    void OnTriggerStay(Collider other)
    {
        if (other.name.Contains("M.") && !m_IsFinished)
        {
            m_IsFinished = true;
            m_WinnerName = other.name.Substring(2);
            Debug.Log("Finished user = " + m_WinnerName);
        }
    }
}
