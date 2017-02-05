using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class scr_SliderValueToText : MonoBehaviour {

    private Text m_Text;
    private Slider m_Slider;
    private Text m_InputFieldText;
    private float m_PrevValue = 1.0f;

	// Use this for initialization
	void Start () {
	    m_Text = gameObject.transform.FindChild("Handle Slide Area").FindChild("Handle").FindChild("Text").GetComponent<Text>();
        m_Slider = gameObject.GetComponent<Slider>();
        m_InputFieldText = gameObject.transform.parent.FindChild("SliderValue").FindChild("Placeholder").GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        float value = m_Slider.value;

        if (m_PrevValue != value)
        {
            m_Text.text = value.ToString();
            m_InputFieldText.text = value.ToString();
        }

        m_PrevValue = value;
	}
}
