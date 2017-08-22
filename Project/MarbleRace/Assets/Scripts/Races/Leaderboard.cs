using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Leaderboard : MonoBehaviour
{
    private List<Vector3> m_CheckpointsArr = new List<Vector3>();

    private List<string> m_UsersArr = new List<string>();
    private List<Color> m_ColorsArr = new List<Color>();
    private List<int> m_CPNextArr = new List<int>();
    private List<float> m_DistanceArr = new List<float>();

    private GameObject[] m_Checkpoints;

    private Text m_Ranking;
    private Text m_RankingNoRichText;

    private float m_TotalDist = 0;
    private bool m_IsCallingCoroutine = false;

    GlobalManager gManager;

    // Use this for initialization
    void Start()
    {
        gManager = GlobalManager.Instance();

        m_Checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");

        // Initialize CheckpointsArray
        for (int i = 0; i < m_Checkpoints.Length; ++i)
        {
            m_CheckpointsArr.Add(Vector3.zero);
        }

        foreach (GameObject cp in m_Checkpoints)
        {
            // Sort
            string name = cp.name;
            string number = name.Substring(10);
            int n = 0;
            bool isValid = int.TryParse(number, out n);
            if (!isValid)
            {
                Debug.Log("Number not found in checkpoint name!");
            }
            // Add checkpoint position
            Vector3 v = cp.GetComponent<Transform>().position;
            m_CheckpointsArr[n] = v;
        }
        
        // Add finish as last checkpoint that no one can trigger (prevent out of range errors)
        m_CheckpointsArr.Add(GameObject.Find("FinishTrigger").transform.position);

        m_Ranking = GameObject.Find("RichText").GetComponent<Text>();
        m_Ranking.text = null;
        m_Ranking.supportRichText = true;
        m_RankingNoRichText = GameObject.Find("NoRichText").GetComponent<Text>();
        m_RankingNoRichText.text = null;
    }

    // Update is called once per frame
    void Update()
    {
        // Pre-race: Add users when they join
        if (!gManager.gIsGiveawayClosed)
        {
            AddUsersToBoard();
        }

        // Race begins: Update leaderboard variables first, then update leaderboard text
        if (!m_IsCallingCoroutine && gManager.gIsGiveawayClosed)
        {
            StartCoroutine(UpdateBoard());
        }
    }

    public void AddUser(string name, Color color)
    {
        m_UsersArr.Add(name);
        m_ColorsArr.Add(color);
        m_DistanceArr.Add(m_TotalDist);
        m_CPNextArr.Add(0);
    }

    private void AddUsersToBoard()
    {
        m_Ranking.text = null;
        m_RankingNoRichText.text = null;
        for (int i = 0; i < m_UsersArr.Count; ++i)
        {
            // Clean text:
            string colorHex = "";
            int r = (int)(m_ColorsArr[i].r * 255);
            int g = (int)(m_ColorsArr[i].g * 255);
            int b = (int)(m_ColorsArr[i].b * 255);
            string rHex = r.ToString("X");
            string gHex = g.ToString("X");
            string bHex = b.ToString("X");
            rHex = rHex.Length == 1 ? rHex + "0" : rHex;
            gHex = gHex.Length == 1 ? gHex + "0" : gHex;
            bHex = bHex.Length == 1 ? bHex + "0" : bHex;
            colorHex = rHex + gHex + bHex;
            m_Ranking.text += "<color=#" + colorHex + ">" + (i + 1).ToString() + ". " + m_UsersArr[i] + "</color>\n";
            m_RankingNoRichText.text += (i + 1).ToString() + ". " + m_UsersArr[i] + "\n";
            // Debugging text:
            //m_Ranking.text += (i + 1).ToString() + ". " + m_UsersArr[i] + ":\n" + m_CPNextArr[i] + "; " + m_DistanceArr[i] + "\n";
        }
    }

    IEnumerator UpdateBoard()
    {
        m_IsCallingCoroutine = true;
        int lowest = 0;

        List<float> diffArr = new List<float>();

        for (int i = 0; i < m_UsersArr.Count; ++i)
        {
            // Update distance
            m_DistanceArr[i] = GetUserDist(i) + GetTotalDist(i);
        }

        for (int i = 0; i < m_UsersArr.Count; ++i)
        {
            // Find lowest
            if (m_DistanceArr[i] < m_DistanceArr[lowest])
                lowest = i;
        }

        for (int i = 0; i < m_UsersArr.Count; ++i)
        {
            // Calc difference
            diffArr.Add(m_DistanceArr[i] - m_DistanceArr[lowest]);
        }

        // Bubble sort
        int n = diffArr.Count;
        while (n > 0)
        {
            int newn = 0;
            for (int i = 1; i < n; ++i)
            {
                if (diffArr[i - 1] > diffArr[i])
                {
                    float copyDiff = diffArr[i - 1];
                    diffArr[i - 1] = diffArr[i];
                    diffArr[i] = copyDiff;

                    int copyCP = m_CPNextArr[i - 1];
                    m_CPNextArr[i - 1] = m_CPNextArr[i];
                    m_CPNextArr[i] = copyCP;

                    string copyUser = m_UsersArr[i - 1];
                    m_UsersArr[i - 1] = m_UsersArr[i];
                    m_UsersArr[i] = copyUser;

                    float copyDist = m_DistanceArr[i - 1];
                    m_DistanceArr[i - 1] = m_DistanceArr[i];
                    m_DistanceArr[i] = copyDist;

                    Color copyColor = m_ColorsArr[i - 1];
                    m_ColorsArr[i - 1] = m_ColorsArr[i];
                    m_ColorsArr[i] = copyColor;

                    newn = i;
                }
            }
            n = newn;
        }

        // Update board
        AddUsersToBoard();
        // Add update delay
        yield return new WaitForSeconds(gManager.gScoreboardUpdateDelay);
        m_IsCallingCoroutine = false;
    }
    private float GetTotalDist(int user)
    {
        float dist = 0;
        for (int i = m_CPNextArr[user]; i < m_CheckpointsArr.Count - 1; ++i)
        {
            Vector3 v1 = m_CheckpointsArr[i];
            float x1 = v1.x;
            float y1 = v1.y;
            Vector3 v2 = m_CheckpointsArr[i + 1];
            float x2 = v2.x;
            float y2 = v2.y;
            dist += Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        }
        return dist;
    }

    private float GetUserDist(int user)
    {
        float dist = 0;
        GameObject marble = GameObject.Find(m_UsersArr[user]);
        Vector3 pos = marble.transform.position;
        Vector3 posCP = m_CheckpointsArr[m_CPNextArr[user]];
        float x1 = pos.x;
        float y1 = pos.y;
        float x2 = posCP.x;
        float y2 = posCP.y;
        dist = Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        return dist;
    }

    public void UpdateCheckpoint(string userName, int checkpointNr)
    {
        // Get index of user in array
        int userNr = 0;
        for (int i = 0; i < m_UsersArr.Count; ++i)
        {
            if (m_UsersArr[i] == userName)
            {
                userNr = i;
                break;
            }
        }

        // Set checkpoint to next one
        m_CPNextArr[userNr] = checkpointNr + 1;
    }

    public Text GetRanking()
    {
        return m_RankingNoRichText;
    }
}
