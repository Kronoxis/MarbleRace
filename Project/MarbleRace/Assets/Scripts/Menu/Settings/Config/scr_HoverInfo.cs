using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class scr_HoverInfo : MonoBehaviour {

    GlobalManager gManager;

    GameObject[] m_Buttons;
    string[] m_ButtonNames = {
        "NameInputPath",
        "NameGiveawayURL",
        "NameSpritePath",
        "NameSavePath",
        "NameSaveFormat",
        "NameDownloadUsersDelay",
        "NameScoreboardUpdateDelay",
        "NameAddMarbleDelay",
        "NameManualStart",
        "NameStartDelay"
    };
    string[] m_InfoTexts = {
        "The path where your inputs will be saved. Inputs are controls and configs. If a relative address is given, it will be saved in AppData/MarbleRace/.",
        "The URL where the Application reads the users from.",
        "The path where sprites are saved. Sprites in here will be applied on a Marble if the name of the sprite is in this format: \"Sprite_Username\". If a relative address is given, it will be saved in AppData/MarbleRace/.",
        "The path where Leaderboards will be saved. The Leaderboard will be saved after a user has finished or after pressing the \'Save\' button in a Race (Top Right). This means that Marbles who have switched place after saving, will not be taken into account. If a relative address is given, it will be saved in AppData/MarbleRace/.",
        "The format in which the Leaderboard will be saved.",
        "The delay between each check for new users. Increasing this value may reduce lag. Decreasing this value drastically may result in Users being added twice.",
        "The delay between each Leaderboard update. Increasing this value may reduce lag, but will also be less convenient to track who's in first place.",
        "The delay between each added marble. Increasing this value may reduce lag. Decreasing this value drastically may result in an \'explosion\' of marbles.",
        "Whether !gstop triggers start or not. If Manual Start is enabled, you'll have to manually press the start key.",
        "Whether there should be a start delay or not. If Start Delay is enabled, there will be a 3 second countdown before actually dropping the marbles."
    };
    List<int> m_Heights = new List<int>();

    private float m_Scale = 0f;

    // Use this for initialization
    void Start () {
        gManager = GlobalManager.Instance();

        m_Buttons = GameObject.FindGameObjectsWithTag("Hoverable");

        m_Scale = Screen.width / gManager.gReferenceResolution.x;

        for (int i = 0; i < m_InfoTexts.Length; ++i)
        {
            m_Heights.Add(18 * (int)(m_InfoTexts[i].Length / 48.0));
        }
    }
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = Input.mousePosition;
        bool isHovering = false;
        foreach (GameObject btn in m_Buttons)
        {
            Vector3 buttonPos = btn.GetComponent<RectTransform>().position;

            float width = btn.GetComponent<RectTransform>().rect.width;
            float height = btn.GetComponent<RectTransform>().rect.height;

            width *= m_Scale;
            height *= m_Scale;

            if (mousePos.x > buttonPos.x - width / 2 && mousePos.x < buttonPos.x + width / 2 &&
                mousePos.y > buttonPos.y - height / 2 && mousePos.y < buttonPos.y + height / 2)
            {
                isHovering = true;

                for (int i = 0; i < m_ButtonNames.Length; ++i)
                    if (btn.name == m_ButtonNames[i])
                    {
                        // Set text
                        transform.Find("Text").GetComponent<Text>().text = m_InfoTexts[i];

                        // Set Height of Panel
                        GetComponent<RectTransform>().sizeDelta = new Vector2(
                            GetComponent<RectTransform>().rect.width,
                            m_Heights[i] + 5);

                        // Set Height of Text Box
                        transform.Find("Text").GetComponent<RectTransform>().sizeDelta = new Vector2(
                            transform.Find("Text").GetComponent<RectTransform>().rect.width,
                            m_Heights[i]);

                        // Set Position of HoverInfo
                        transform.position = new Vector3(
                            mousePos.x + GetComponent<RectTransform>().rect.width * m_Scale / 2,
                            mousePos.y + (mousePos.y > Screen.height / 2 - m_Heights[i] ? -1 : 1) * GetComponent<RectTransform>().rect.height * m_Scale/ 2,
                            mousePos.z);

                        break;
                    }
                break;
            }
        }

        if (!isHovering)
        {
            transform.position = new Vector3(-1000, -1000, 0);
        }
	}
}
