using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class LeaderboardValue
{
    public string Name;
    public Color Color;
    public int Checkpoint;
    public float Distance;

    public LeaderboardValue(GameObject marble)
    {
        // Name
        Name = marble.name;

        // Color
        var mat = marble.GetComponent<Renderer>().material;
        if (mat.mainTexture == null)
        {
            Color = mat.color;
        }
        else
        {
            Texture2D tex = (Texture2D)mat.mainTexture;
            float r = 0, g = 0, b = 0;
            int rows = 6, cols = 6;
            for (int row = 0; row < rows; ++row)
            {
                for (int col = 0; col < cols; ++col)
                {
                    var c = tex.GetPixel(tex.width / 2 + col - col / 2, tex.height / 2 + row - row / 2);
                    r += c.r;
                    g += c.g;
                    b += c.b;
                }
            }
            Color = new Color(r / rows / cols, g / rows / cols, b / rows / cols);
        }

        // Checkpoint
        Checkpoint = 0;

        // Distance
        Distance = 0.0f;
    }
}

public class scr_Leaderboard : MonoBehaviour
{
    public Text Ranking;
    string m_RawRanking;

    List<Vector3> m_CheckpointLocations = new List<Vector3>();

    static List<LeaderboardValue> m_LeaderboardValues = new List<LeaderboardValue>();

    // Use this for initialization
    void Start()
    {
        // Clear Static Arrays
        m_LeaderboardValues.Clear();

        // Checkpoints
        var checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        for (int i = 0; i < checkpoints.Length; ++i)
        {
            m_CheckpointLocations.Add(Vector3.zero);
        }

        foreach (GameObject cp in checkpoints)
        {
            // Sort
            string name = cp.name;
            string number = name.Substring(12, name.Length - 13); // Naming Convention: Checkpoint (n)
            int n = 0;
            bool isValid = int.TryParse(number, out n);
            if (!isValid)
            {
                Debug.Log("Number not found in checkpoint name!");
                continue;
            }
            // Add checkpoint position
            Vector3 v = cp.GetComponent<Transform>().position;
            m_CheckpointLocations[n] = v;
        }
        // Add finish as last checkpoint that no one can trigger (prevent out of range errors)
        //m_CheckpointLocations.Add(GameObject.FindGameObjectWithTag("End").transform.position);

        // Clear texts
        //Ranking.text = null;
        //Ranking.supportRichText = true;
        m_RawRanking = null;

        // Leaderboard Update Loop
        //StartCoroutine(UpdateBoard());
    }

    public static void AddUserToLeaderboard(GameObject user)
    {
        LeaderboardValue v = new LeaderboardValue(user);
        m_LeaderboardValues.Add(v);
    }

    IEnumerator UpdateBoard()
    {
        while (true)
        {
            int lowest = 0;

            List<float> diffArr = new List<float>();

            for (int i = 0; i < m_LeaderboardValues.Count; ++i)
            {
                // Update distance
                m_LeaderboardValues[i].Distance = GetUserDist(i) + GetTotalDist(i);
            }

            for (int i = 0; i < m_LeaderboardValues.Count; ++i)
            {
                // Find lowest
                if (m_LeaderboardValues[i].Distance < m_LeaderboardValues[lowest].Distance)
                    lowest = i;
            }

            for (int i = 0; i < m_LeaderboardValues.Count; ++i)
            {
                // Calc difference
                diffArr.Add(m_LeaderboardValues[i].Distance - m_LeaderboardValues[lowest].Distance);
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

                        int copyCP = m_LeaderboardValues[i - 1].Checkpoint;
                        m_LeaderboardValues[i - 1].Checkpoint = m_LeaderboardValues[i].Checkpoint;
                        m_LeaderboardValues[i].Checkpoint = copyCP;

                        string copyName = m_LeaderboardValues[i - 1].Name;
                        m_LeaderboardValues[i - 1].Name = m_LeaderboardValues[i].Name;
                        m_LeaderboardValues[i].Name = copyName;

                        float copyDist = m_LeaderboardValues[i - 1].Distance;
                        m_LeaderboardValues[i - 1].Distance = m_LeaderboardValues[i].Distance;
                        m_LeaderboardValues[i].Distance = copyDist;

                        Color copyColor = m_LeaderboardValues[i - 1].Color;
                        m_LeaderboardValues[i - 1].Color = m_LeaderboardValues[i].Color;
                        m_LeaderboardValues[i].Color = copyColor;

                        newn = i;
                    }
                }
                n = newn;
            }

            // Update board
            AddUsersToBoard();
            // Add update delay
            yield return new WaitForSeconds(scr_InputManager.UpdateDelay);
        }
    }

    void AddUsersToBoard()
    {
        Ranking.text = null;
        m_RawRanking = null;
        for (int i = 0; i < m_LeaderboardValues.Count; ++i)
        {
            // Clean text:
            string colorHex = "";
            int r = (int)(m_LeaderboardValues[i].Color.r * 255);
            int g = (int)(m_LeaderboardValues[i].Color.g * 255);
            int b = (int)(m_LeaderboardValues[i].Color.b * 255);
            string rHex = r.ToString("X");
            string gHex = g.ToString("X");
            string bHex = b.ToString("X");
            rHex = rHex.Length == 1 ? rHex + "0" : rHex;
            gHex = gHex.Length == 1 ? gHex + "0" : gHex;
            bHex = bHex.Length == 1 ? bHex + "0" : bHex;
            colorHex = rHex + gHex + bHex;
            Ranking.text += "<color=#" + colorHex + ">" + (i + 1).ToString() + ". " + m_LeaderboardValues[i].Name + "</color>\n";
            m_RawRanking += (i + 1).ToString() + ". " + m_LeaderboardValues[i].Name + "\n";
            // Debugging text:
            //Ranking.text += (i + 1).ToString() + ". " + m_LeaderboardValues[i].Name + ":\n" 
            //    + m_LeaderboardValues[i].Checkpoint + "; " + m_LeaderboardValues[i].Distance + "\n";
        }
    }

    private float GetTotalDist(int user)
    {
        float dist = 0;
        for (int i = m_LeaderboardValues[user].Checkpoint; i < m_CheckpointLocations.Count - 1; ++i)
        {
            Vector3 v1 = m_CheckpointLocations[i];
            float x1 = v1.x;
            float y1 = v1.y;
            Vector3 v2 = m_CheckpointLocations[i + 1];
            float x2 = v2.x;
            float y2 = v2.y;
            dist += Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2);
        }
        return dist;
    }

    private float GetUserDist(int user)
    {
        float dist = 0;
        GameObject marble = GameObject.Find(m_LeaderboardValues[user].Name);
        Vector3 pos = marble.transform.position;
        Vector3 posCP = m_CheckpointLocations[m_LeaderboardValues[user].Checkpoint];
        float x1 = pos.x;
        float y1 = pos.y;
        float x2 = posCP.x;
        float y2 = posCP.y;
        dist = Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2);
        return dist;
    }

    public static void UpdateCheckpoint(string userName, int checkpointNr)
    {
        // Get index of user in array
        int userNr = 0;
        for (int i = 0; i < m_LeaderboardValues.Count; ++i)
        {
            if (m_LeaderboardValues[i].Name == userName)
            {
                userNr = i;
                break;
            }
        }

        // Set checkpoint to next one
        m_LeaderboardValues[userNr].Checkpoint = checkpointNr + 1;
    }

    public static string GetHexColor(string user)
    {
        var value = m_LeaderboardValues.Find(x => x.Name == user);

        if (value == null) return null;

        int r = (int)(value.Color.r * 255);
        int g = (int)(value.Color.g * 255);
        int b = (int)(value.Color.b * 255);
        string rHex = r.ToString("X");
        string gHex = g.ToString("X");
        string bHex = b.ToString("X");
        rHex = rHex.Length == 1 ? rHex + "0" : rHex;
        gHex = gHex.Length == 1 ? gHex + "0" : gHex;
        bHex = bHex.Length == 1 ? bHex + "0" : bHex;

        return rHex + gHex + bHex;
    }

    public void SaveLeaderboard(bool saveIfUnfinished)
    {
        if (!saveIfUnfinished && !scr_InputManager.IsFinished) return;

        string fileName = "Leaderboard_" + GetDateTime() + ".txt";
        StreamWriter writer = new StreamWriter(scr_InputManager.DataPath + "saves/" + fileName);
        writer.Write(m_RawRanking);
        writer.Close();
    }

    string GetDateTime()
    {
        string day = System.DateTime.Now.Day.ToString();
        if (day.Length == 1)
            day = "0" + day;
        string month = System.DateTime.Now.Month.ToString();
        if (month.Length == 1)
            month = "0" + month;
        string year = System.DateTime.Now.Year.ToString();
        string hour = System.DateTime.Now.Hour.ToString();
        if (hour.Length == 1)
            hour = "0" + hour;
        string minutes = System.DateTime.Now.Minute.ToString();
        if (minutes.Length == 1)
            minutes = "0" + minutes;
        string seconds = System.DateTime.Now.Second.ToString();
        if (seconds.Length == 1)
            seconds = "0" + seconds;

        // Ordered correctly by name
        return year + month + day + "_" + hour + minutes + seconds;
    }
}
