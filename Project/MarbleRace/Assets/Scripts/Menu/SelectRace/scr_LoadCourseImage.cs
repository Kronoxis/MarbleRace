using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

[RequireComponent(typeof(scr_LoadScene))]
[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(Dropdown))]

public class scr_LoadCourseImage : MonoBehaviour {

    private int m_PrevValue;
    private List<Sprite> m_Images = new List<Sprite>();
    private float m_MaxWidth = 400;
    private float m_MaxHeight = 400;
	// Use this for initialization
	void Start () {
        Object obj = Resources.Load("Images/Race_01");
        Texture2D tex = obj as Texture2D;
        Sprite image = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
        m_Images.Add(image);

        obj = Resources.Load("Images/Race_02");
        tex = obj as Texture2D;
        image = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
        m_Images.Add(image);

        obj = Resources.Load("Images/Race_03");
        tex = obj as Texture2D;
        image = Sprite.Create(tex, new Rect(0f, 0f, tex.width, tex.height), Vector2.zero);
        m_Images.Add(image);
	}
	
	// Update is called once per frame
	void Update () {
        int value = GetComponent<Dropdown>().value;
        if (m_PrevValue != value)
        {
            LoadImage(value);
            m_PrevValue = value;
            GameObject.Find("ScriptManager").GetComponent<scr_LoadScene>().SetSceneNr(value + 1);
            if (value > 0)
                DisableConfirm();
            else
                EnableConfirm();

        }
    }

    void LoadImage(int value)
    {
        GameObject.Find("Image").GetComponent<Image>().sprite = m_Images[value];
        float width = m_Images[value].rect.width;
        float height = m_Images[value].rect.height;
        float scaleFactor = 1;
        if (width > m_MaxWidth || height > m_MaxHeight)
        {
            if (width > height)
                scaleFactor = m_MaxWidth / width;
            else
                scaleFactor = m_MaxHeight / height;
        }

        width *= scaleFactor;
        height *= scaleFactor;

        GameObject.Find("Image").GetComponent<RectTransform>().sizeDelta = new Vector2(width, height);
    }

    void DisableConfirm()
    {
        GameObject.Find("Confirm").GetComponent<Button>().interactable = false;
    }

    void EnableConfirm()
    {
        GameObject.Find("Confirm").GetComponent<Button>().interactable = true;
    }
}
