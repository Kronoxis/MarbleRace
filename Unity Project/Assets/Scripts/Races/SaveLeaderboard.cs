using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class SaveLeaderboard : MonoBehaviour {

    GlobalManager gManager;

    void Start()
    {
        gManager = GlobalManager.Instance();
    }

    public void Save()
    {
        StreamWriter sw = new StreamWriter(gManager.gSavePath + "Leaderboard_" + GetDateTime() + ".txt");
        string data = GameObject.Find("Canvas").transform.FindChild("Panel").GetComponent<Leaderboard>().GetRanking().text;
        sw.Write(data);
        sw.Close();
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

        return Format(day, month, year, hour, minutes, seconds);
     }

    string Format(string day, string month, string year, string hour, string minutes, string seconds)
    {
        string date = null;

        switch (gManager.gFormat)
        {
            case GlobalManager.LeaderboardFormat.HHMMSS_YYYYMMDD:
                date = hour + "h" + minutes + "m" + seconds + "_" + year + "-" + month + "-" + day;
                break;
            case GlobalManager.LeaderboardFormat.HHMMSS_YYYYDDMM:
                date = hour + "h" + minutes + "m" + seconds + "_" + year + "-" + day + "-" + month;
                break;
            case GlobalManager.LeaderboardFormat.HHMMSS_DDMMYYYY:
                date = hour + "h" + minutes + "m" + seconds + "_" + day + "-" + month + "-" + year;
                break;
            case GlobalManager.LeaderboardFormat.HHMMSS_MMDDYYYY:
                date = hour + "h" + minutes + "m" + seconds + "_" + month + "-" + day + "-" + year;
                break;
            case GlobalManager.LeaderboardFormat.HHMM_YYYYMMDD:
                date = hour + "h" + minutes + "_" + year + "-" + month + "-" + day;
                break;
            case GlobalManager.LeaderboardFormat.HHMM_YYYYDDMM:
                date = hour + "h" + minutes + "_" + year + "-" + day + "-" + month;
                break;
            case GlobalManager.LeaderboardFormat.HHMM_DDMMYYYY:
                date = hour + "h" + minutes + "_" + day + "-" + month + "-" + year;
                break;
            case GlobalManager.LeaderboardFormat.HHMM_MMDDYYYY:
                date = hour + "h" + minutes + "_" + month + "-" + day + "-" + year;
                break;
            default:
                date = "Error_PleaseContactThycon";
                break;
        }

        return date;
    }
}
