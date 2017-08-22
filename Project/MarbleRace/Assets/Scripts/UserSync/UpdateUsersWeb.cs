using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[RequireComponent(typeof(scr_CreateMarble))]

public class UpdateUsersWeb : MonoBehaviour {

    private string[] m_PrevUsersArr = { };
    private string[] m_UsersArr = { "" };
    private List<Color> m_ColorsArr = new List<Color>();
    private scr_CreateMarble m_CreateScript;
    private Leaderboard m_L;
    private float m_AccuTime;
    private float m_MaxTime;
    private bool m_IsNotified = false;
    private bool m_IsUpdating = false;

    private Text m_CenterText;

    GlobalManager gManager;
    void Awake()
    {
        for (int r = 0; r <= 256; r+=32)
        {
            for (int g = 0; g <= 256; g+=32)
            {
                for (int b = 0; b <= 256; b+=32)
                {
                    m_ColorsArr.Add(new Color(r / 256.0f, g / 256.0f, b / 256.0f));
                }
            }
        }

        for (int i = 0; i < m_ColorsArr.Count; ++i)
        {
            Color c1 = m_ColorsArr[i];
            int c2 = Random.Range(0, m_ColorsArr.Count - 1);
            m_ColorsArr[i] = m_ColorsArr[c2];
            m_ColorsArr[c2] = c1;
        }
    }

    // Use this for initialization
    void Start() {
        gManager = GlobalManager.Instance();

        m_CreateScript = GetComponent<scr_CreateMarble>();
        m_L = GameObject.Find("Canvas").GetComponentInChildren<Leaderboard>();
        m_MaxTime = gManager.gAddMarbleDelay;
        m_AccuTime = Mathf.Max(m_MaxTime - 1.0f, 0);
        m_UsersArr = scr_UserManager.GetUsers();
        m_CenterText = GameObject.Find("CenterText").GetComponent<Text>();
        m_CenterText.enabled = true;
        m_CenterText.supportRichText = true;
        m_CenterText.text = "<color=#02EE00><size=50>Type \n!gjoin\nto join the Giveaway</size></color>\n\n\n\n<color=#FFFFFF><size=20>@Streamer\n Type !gstop to close the Giveaway before you start.</size></color>";
    }

    // Update is called once per frame
    void Update()
    {
        if (!gManager.gIsGiveawayClosed)
        {
            m_AccuTime += Time.deltaTime;
            if (m_AccuTime >= m_MaxTime && !m_IsUpdating)
            {
                // Reset timer
                m_AccuTime = 0f;
                Debug.Log("Checking for userlist update");

                DoUpdate();
            }
        }
        else if (!m_IsNotified)
        {
            m_CenterText.enabled = false;
            // Do Update one more time for those who entered between last check and giveaway close
            DoUpdate();
            Debug.Log("Giveaway has been closed");
            m_IsNotified = true;
        }
    }

    private void DoUpdate()
    {
        // Reload userlist
        m_UsersArr = scr_UserManager.GetUsers();
        // Add new users
        if (m_UsersArr != null)
        {
            if (m_UsersArr.Length > m_PrevUsersArr.Length)
            {
                Debug.Log("Updating userlist");
                StartCoroutine(AddMarbles());
            }
            // Warn if userlist is messed up
            else if (m_UsersArr.Length < m_PrevUsersArr.Length)
            {
                Debug.Log("Something went wrong while reading users! (#Users < #PrevUsers)");
            }
        }
    }

    IEnumerator AddMarbles()
    {
        m_IsUpdating = true;
        // Skip users that have already been added
        int start = m_PrevUsersArr.Length == 0 ? 0 : m_PrevUsersArr.Length;
        // Don't add the end marker as player
        int end = m_UsersArr[m_UsersArr.Length - 1] == "|" ? m_UsersArr.Length - 1 : m_UsersArr.Length;
        for (int i = start; i < end; ++i)
        {
            // Create marble
            int c = i;
            if (c >= m_ColorsArr.Count) c %= m_ColorsArr.Count;
            m_CreateScript.CreateMarble(m_UsersArr[i], m_ColorsArr[i], new Vector2(-5.5f, 5.0f), new Vector2(1.4f, 1.0f));
            // Add user to leaderboard
            m_L.AddUser(m_UsersArr[i], m_ColorsArr[i]);
            // Wait for specified time to add next marble
            yield return new WaitForSeconds(gManager.gAddMarbleDelay);
        }
        // Update amount of added users
        m_PrevUsersArr = m_UsersArr;
        Debug.Log("Finished updating userlist");
        m_IsUpdating = false;
    }
}
