using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class scr_UpdateControls : MonoBehaviour {

    private bool m_IsSetKey = false;
    private Button m_Btn;
    private ColorBlock m_DefaultColorBlock;

    private GlobalManager gManager;

    void Start()
    {
        gManager = GlobalManager.Instance();
        m_Btn = GetComponent<Button>();
        m_DefaultColorBlock = m_Btn.colors;
    }

    // Update is called once per frame
    void Update()
    {
        m_Btn.onClick.AddListener(TaskOnClick);
        if (m_IsSetKey)
        {
            ColorBlock cb = m_Btn.colors;
            cb.highlightedColor = new Color(0.874f, 0.752f, 0.117f);
            m_Btn.colors = cb;
            WaitForKey();
        }
        else
        {
            m_Btn.colors = m_DefaultColorBlock;
        }
    }

    void TaskOnClick()
    {
        m_IsSetKey = true;
    }

    void WaitForKey()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode kc in System.Enum.GetValues(typeof(KeyCode)))
                if (Input.GetKeyDown(kc))
                {
                    SetNewKey(kc);
                    m_IsSetKey = false;
                }
        }
    }

    void SetNewKey(KeyCode key)
    {
        transform.FindChild("Text").GetComponent<Text>().text = gManager.KeyCodeToString(key);
    }
}
