using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scr_InputFieldToSliderValue : MonoBehaviour {

    public void ConvertValue()
    {
        float value = float.Parse(gameObject.transform.FindChild("Text").GetComponent<Text>().text);
        gameObject.transform.parent.GetChild(1).GetComponent<Slider>().value = value;
    }

    void Update()
    {
        if (!gameObject.GetComponent<InputField>().isFocused)
        {
            gameObject.GetComponent<InputField>().text = null;
        }
    }
}
