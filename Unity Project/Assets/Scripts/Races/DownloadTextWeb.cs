using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;

public class DownloadTextWeb : MonoBehaviour
{
    private string m_Reply;
    private string[] m_UsersArr;

    GlobalManager gManager;

    // Use this for initialization
    void Start()
    {
        gManager = GlobalManager.Instance();
        DownloadFile();
    }

    void DownloadFile()
    {
        WebClient client = new WebClient();
        client.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler(DownloadFileCompleted);
        client.DownloadFileAsync((new System.Uri("http://pixelbot.nl/giveaway")), gManager.gSavePath + "EnteredUsers.txt");
    }


    void DownloadFileCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {
        m_Reply = GetUsersFromFile(gManager.gSavePath + "EnteredUsers.txt");
        gManager.gIsGiveawayClosed = m_Reply.Contains("|");
        SplitUsers();
    }

    string GetUsersFromFile(string path)
    {
        StreamReader sr = new StreamReader(path);
        string data = sr.ReadToEnd();
        sr.Close();
        return data;
    }

    void SplitUsers()
    {
        char[] splits = { ' ' };
        m_UsersArr = m_Reply.Split(splits, System.StringSplitOptions.RemoveEmptyEntries);
    }

    public string[] GetUsersArr()
    {
        return m_UsersArr;
    }

    public void Restart()
    {
        Start();
    }
}